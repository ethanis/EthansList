
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private List<SearchObject> deserialized;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = new ListView(this.Activity);

            savedSearches = MainActivity.databaseConnection.GetAllSearchesAsync().Result;
            deserialized = DeserializeSearches(savedSearches).Result;
            view.Adapter = new SavedSearchesAdapter(this.Activity, deserialized);

            view.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => { 
                QueryGeneration queryHelper = new QueryGeneration();

                var transaction = Activity.SupportFragmentManager.BeginTransaction();
                SearchResultsFragment resultsFragment = new SearchResultsFragment();
                resultsFragment.Query = queryHelper.Generate(deserialized[e.Position]);
                resultsFragment.MaxListings = deserialized[e.Position].MaxListings;
                resultsFragment.WeeksOld = deserialized[e.Position].PostedDate;

                transaction.Replace(Resource.Id.frameLayout, resultsFragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };

            return view;
        }

        private Task<List<SearchObject>> DeserializeSearches(List<Search> savedSearches)
        {
            List<SearchObject> searchObjects = new List<SearchObject>();
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
        readonly List<SearchObject> _searches;

        public SavedSearchesAdapter(Context context, List<SearchObject> searches)
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

            view.CityTitle.Text = _searches[position].SearchLocation.SiteName;

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
        public ImageButton DeleteButton { get; set; }

        public SavedSearchRow(Context context)
            :base(context)
        {
            _context = context;
            rowHeight = PixelConverter.DpToPixels(context.Resources.GetInteger(Resource.Integer.textLabelRowHeight));
            Initialize();
        }

        void Initialize()
        {
            Orientation = Orientation.Vertical;
            LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);

            LinearLayout CityButtonHolder = new LinearLayout(_context);
            CityButtonHolder.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);

            CityTitle = new TextView(_context);
            CityTitle.Gravity = GravityFlags.CenterVertical;
            CityTitle.SetTextSize(Android.Util.ComplexUnitType.Px, rowHeight * 0.50f);
            CityTitle.SetPadding((int)(rowHeight * 0.1), (int)(rowHeight * 0.15), (int)(rowHeight * 0.1), (int)(rowHeight * 0.15));
            CityButtonHolder.AddView(CityTitle);

            var buttonHolder = new LinearLayout(_context);
            buttonHolder.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
            buttonHolder.SetHorizontalGravity(GravityFlags.Right);

            DeleteButton = new ImageButton(_context);
            DeleteButton.SetImageResource(Android.Resource.Drawable.IcMenuDelete);
            DeleteButton.SetBackgroundResource(0);
            buttonHolder.AddView(DeleteButton);
            CityButtonHolder.AddView(buttonHolder);

            AddView(CityButtonHolder);

            SearchTerms = new TextView(_context);
            SearchTerms.Gravity = GravityFlags.CenterVertical;
            SearchTerms.SetTextSize(Android.Util.ComplexUnitType.Px, rowHeight * 0.40f);
            SearchTerms.SetPadding((int)(rowHeight * 0.1), (int)(rowHeight * 0.15), (int)(rowHeight * 0.1), (int)(rowHeight * 0.15));
            AddView(SearchTerms);
        }
    }
}

