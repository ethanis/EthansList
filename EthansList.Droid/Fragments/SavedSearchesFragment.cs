
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using EthansList.Models;
using EthansList.Shared;
using Newtonsoft.Json;

namespace EthansList.Droid
{
    public class SavedSearchesFragment : Android.Support.V4.App.Fragment
    {
        private List<Search> savedSearches;
        private SavedSearchesAdapter adapter;
        private ObservableCollection<SearchObject> deserialized;
        private event EventHandler<EventArgs> DeserializingComplete;

        public override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here

            savedSearches = await MainActivity.databaseConnection.GetAllSearchesAsync().ConfigureAwait(false);
            deserialized = await DeserializeSearches(savedSearches);
            if (deserialized != null)
            {
                if (DeserializingComplete != null)
                    DeserializingComplete(this, new EventArgs());
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = new ListView(this.Activity);

            this.DeserializingComplete += delegate
            {
                view.Adapter = adapter = new SavedSearchesAdapter(this.Activity, deserialized);
                deserialized = null;
            };

            view.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
            {
                QueryGeneration queryHelper = new QueryGeneration();

                var transaction = Activity.SupportFragmentManager.BeginTransaction();
                SearchResultsFragment resultsFragment = new SearchResultsFragment();
                resultsFragment.Query = queryHelper.Generate(adapter._searches[e.Position]);
                resultsFragment.MaxListings = adapter._searches[e.Position].MaxListings;
                resultsFragment.WeeksOld = adapter._searches[e.Position].PostedDate;

                transaction.Replace(Resource.Id.frameLayout, resultsFragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };

            view.ItemLongClick += (sender, e) =>
            {
                PopupMenu menu = new PopupMenu(this.Activity, view.GetChildAt(e.Position));
                menu.Inflate(Resource.Menu.DeleteMenu);
                menu.Show();

                menu.MenuItemClick += (se, args) =>
                {
                    var result = MainActivity.databaseConnection.DeleteSearchAsync(adapter._searches[e.Position].SearchLocation.Url, adapter._searches[e.Position]).Result;
                    if (MainActivity.databaseConnection.StatusCode == codes.ok && result)
                    {
                        lock (adapter._searches)
                        {
                            Activity.RunOnUiThread(() =>
                            {
                                adapter._searches.RemoveAt(e.Position);
                            });

                            adapter.NotifyDataSetChanged();
                        }
                    }
                    else
                    {
                    }
                    Console.WriteLine(MainActivity.databaseConnection.StatusMessage);
                };
            };

            return view;
        }

        private Task<ObservableCollection<SearchObject>> DeserializeSearches(List<Search> savedSearches)
        {
            ObservableCollection<SearchObject> searchObjects = new ObservableCollection<SearchObject>();
            return Task.Run(() =>
            {
                foreach (Search search in savedSearches)
                {
                    try
                    {
                        searchObjects.Add(JsonConvert.DeserializeObject<SearchObject>(search.SerializedSearch));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                return searchObjects;
            });

        }
    }

    public class SavedSearchesAdapter : BaseAdapter<SearchObject>
    {
        readonly Context _context;
        public ObservableCollection<SearchObject> _searches { get; set; }

        public SavedSearchesAdapter(Context context, ObservableCollection<SearchObject> searches)
        {
            _context = context;
            _searches = searches;
        }

        public override SearchObject this[int position]
        {
            get
            {
                return _searches[position];
            }
        }

        public override int Count
        {
            get
            {
                return _searches.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = (SavedSearchRow)convertView;
            if (view == null)
            {
                view = new SavedSearchRow(_context);
            }

            view.CityTitle.Text = _searches[position].SearchLocation.SiteName + ": " + _searches[position].Category.Value;

            view.SearchTerms.Text = MainActivity.databaseConnection.SecondFormatSearch(_searches[position]);

            return view;
        }
    }

    public class SavedSearchRow : LinearLayout
    {
        readonly Context _context;
        private int rowHeight;

        public TextView CityTitle { get; set; }
        public TextView SearchTerms { get; set; }

        public SavedSearchRow(Context context)
            : base(context)
        {
            _context = context;
            rowHeight = PixelConverter.DpToPixels(context.Resources.GetInteger(Resource.Integer.textLabelRowHeight));
            Initialize();
        }

        void Initialize()
        {
            Orientation = Orientation.Vertical;
            LayoutParameters = new ListView.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);

            CityTitle = new TextView(_context);
            CityTitle.Gravity = GravityFlags.CenterVertical;
            CityTitle.SetTextSize(Android.Util.ComplexUnitType.Px, rowHeight * 0.40f);
            CityTitle.SetTypeface(Android.Graphics.Typeface.DefaultBold, Android.Graphics.TypefaceStyle.Bold);
            CityTitle.SetPadding((int)(rowHeight * 0.1), (int)(rowHeight * 0.15), (int)(rowHeight * 0.1), (int)(rowHeight * 0.15));

            AddView(CityTitle);

            SearchTerms = new TextView(_context);
            SearchTerms.Gravity = GravityFlags.CenterVertical;
            SearchTerms.SetTextSize(Android.Util.ComplexUnitType.Px, rowHeight * 0.40f);
            SearchTerms.SetPadding((int)(rowHeight * 0.2), 0, (int)(rowHeight * 0.1), (int)(rowHeight * 0.15));
            AddView(SearchTerms);
        }
    }
}

