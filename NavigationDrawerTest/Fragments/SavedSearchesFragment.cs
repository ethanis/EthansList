
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

namespace EthansList.MaterialDroid
{
    public class SavedSearchesFragment : Android.Support.V4.App.Fragment
    {
        private List<Search> savedSearches;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //var view = new TextView(this.Activity) { Text = "Saved Searches" };
            var view = new ListView(this.Activity);

            savedSearches = MainActivity.databaseConnection.GetAllSearchesAsync().Result;
            //TODO: make this into an async call
            view.Adapter = new SavedSearchesAdapter(this.Activity, DeserializeSearches(savedSearches));

            return view;
        }

        private List<SearchObject> DeserializeSearches(List<Search> savedSearches)
        {
            List<SearchObject> searchObjects = new List<SearchObject>();
            //await Task.Run(() =>
            //{
                foreach (Search search in savedSearches)
                {
                    searchObjects.Add(JsonConvert.DeserializeObject<SearchObject>(search.SerializedSearch));
                }
            //});

            return searchObjects;
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

        public TextView CityTitle { get; set; }
        public TextView SearchTerms { get; set; }

        public SavedSearchRow(Context context)
            :base(context)
        {
            _context = context;
            Initialize();
        }

        void Initialize()
        {
            Orientation = Orientation.Vertical;
            //LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);

            CityTitle = new TextView(_context);
            AddView(CityTitle);

            SearchTerms = new TextView(_context);
            AddView(SearchTerms);
        }
    }
}

