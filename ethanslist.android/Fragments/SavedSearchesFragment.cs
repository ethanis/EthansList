
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

namespace ethanslist.android
{
    public class SavedSearchesFragment : Fragment
    {
        ListView savedSearchesListView;
        List<Search> savedSearches;
        SavedSearchesListAdapter searchListAdapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
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

            searchListAdapter.dataDeleted += SearchList_DataDeleted;

            return view;
        }

        void SearchList_DataDeleted (object sender, EventArgs e)
        {
            savedSearches = MainActivity.databaseConnection.GetAllSearchesAsync().Result;
            searchListAdapter = new SavedSearchesListAdapter(this.Activity, savedSearches);

            searchListAdapter.dataDeleted += SearchList_DataDeleted;
        }

        void SavedSearchesListView_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
        {
            var transaction = this.FragmentManager.BeginTransaction();
            var feedResultsFragment = new FeedResultsFragment();
            feedResultsFragment.query = String.Format("{0}/search/apa?format=rss&min_price={1}&max_price={2}&bedrooms={3}&bathrooms{4}&query={5}", 
                savedSearches[e.Position].LinkUrl, savedSearches[e.Position].MinPrice, savedSearches[e.Position].MaxPrice, 
                savedSearches[e.Position].MinBedrooms, savedSearches[e.Position].MinBathrooms, savedSearches[e.Position].SearchQuery);

            transaction.Replace(Resource.Id.frameLayout, feedResultsFragment);
            transaction.AddToBackStack(null);
            transaction.Commit();
        }

    }
}

