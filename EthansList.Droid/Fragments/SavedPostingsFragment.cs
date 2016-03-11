
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

namespace EthansList.Droid
{
    public class SavedPostingsFragment : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = new ListView(Activity);

            var savedPostings = MainActivity.databaseConnection.GetAllPostingsAsync().Result;
            view.Adapter = new FeedResultsAdapter(Activity, savedPostings);

            //TODO: mechanism to delete saved postings

            view.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => { 
                var transaction = this.Activity.SupportFragmentManager.BeginTransaction();
                PostingDetailsFragment postingDetailsFragment = new PostingDetailsFragment();
                postingDetailsFragment.Posting = savedPostings[e.Position];
                transaction.Replace(Resource.Id.frameLayout, postingDetailsFragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };

            return view;
        }        
    }
}

