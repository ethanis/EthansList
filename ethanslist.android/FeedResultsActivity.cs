using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ethanslist.android
{
    [Activity(Label = "Craigslist Feed Results")]			
    public class FeedResultsActivity : Activity
    {
        ListView feedResultsListView;
        PostingListAdapter postingListAdapter;
        CLFeedClient feedClient;
        String query;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.FeedResults);
            query = Intent.GetStringExtra("query");
                
            feedClient = new CLFeedClient(query);

            feedResultsListView = FindViewById<ListView>(Resource.Id.feedResultsListView);

            feedClient.loadingComplete += (object sender, EventArgs e) =>
            {
                    Console.WriteLine(feedClient.postings.Count);
                postingListAdapter = new PostingListAdapter(this, feedClient.postings);
                feedResultsListView.Adapter = postingListAdapter;
            };

        }
    }
}

