
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using EthansList.Models;
using System.Collections.ObjectModel;
using EthansList.Shared;
using Newtonsoft.Json;

namespace ethanslist.android
{
    public class SavedSearchesFragment : Fragment
    {
        ListView savedSearchesListView;
        List<Search> savedSearches;
        SavedSearchesListAdapter searchListAdapter;
        Dictionary<string, string> searchTerms;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            searchTerms = new Dictionary<string, string>();
            searchTerms.Add("min_price", null);
            searchTerms.Add("max_price", null);
            searchTerms.Add("bedrooms", null);
            searchTerms.Add("bathrooms", null);
            searchTerms.Add("query", null);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.SavedSearches, container, false);

            savedSearches = MainActivity.databaseConnection.GetAllSearchesAsync().Result;

//            savedSearches = new ObservableCollection<Search>(MainActivity.databaseConnection.GetAllSearchesAsync().Result);
            searchListAdapter = new SavedSearchesListAdapter(this.Activity, savedSearches);

            savedSearchesListView = view.FindViewById<ListView>(Resource.Id.savedSearchListView);
            savedSearchesListView.Adapter = searchListAdapter;

            savedSearchesListView.ItemClick += SavedSearchesListView_ItemClick;

            return view;
        }

        void SavedSearchesListView_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
        {
            var transaction = this.FragmentManager.BeginTransaction();
            var feedResultsFragment = new FeedResultsFragment();

            SearchObject search = JsonConvert.DeserializeObject<SearchObject>(savedSearches[e.Position].SerializedSearch);
            QueryGeneration helper = new QueryGeneration();
//            searchTerms["min_price"] = search.MinPrice;
//            searchTerms["max_price"] = search.MaxPrice;
//            searchTerms["bedrooms"] = search.MinBedrooms;
//            searchTerms["bathrooms"] = search.MinBathrooms;
//            searchTerms["query"] = search.SearchQuery;
//            feedResultsFragment.query = helper.Generate(search.LinkUrl, searchTerms);

            feedResultsFragment.query = helper.Generate(search);

            transaction.Replace(Resource.Id.frameLayout, feedResultsFragment);
            transaction.AddToBackStack(null);
            transaction.Commit();
        }

    }
}

