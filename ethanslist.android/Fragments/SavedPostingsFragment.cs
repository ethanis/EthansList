
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

namespace ethanslist.android
{
    public class SavedPostingsFragment : Fragment
    {
        SavedListingListAdapter savedListingsAdapter;
        ListView savedListingsListView;
        List<Posting> savedListings;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.FeedResults, container, false);

            savedListings = MainActivity.databaseConnection.GetAllListingsAsync().Result;

            savedListingsListView = view.FindViewById<ListView>(Resource.Id.feedResultsListView);
            savedListingsAdapter = new SavedListingListAdapter(this.Activity, savedListings);
            savedListingsListView.Adapter = savedListingsAdapter;

            savedListingsListView.ItemClick += (sender, e) => {
                FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
                PostingDetailsFragment postingDetailsFragment = new PostingDetailsFragment();
                postingDetailsFragment.posting = savedListings[e.Position];
                transaction.Replace(Resource.Id.frameLayout, postingDetailsFragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };

            return view;
        }
    }
}

