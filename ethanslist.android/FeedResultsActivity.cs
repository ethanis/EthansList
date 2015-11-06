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
using EthansList.Shared;

namespace EthansList.Droid
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

            feedResultsListView.ItemClick += (sender, e) => {
                var intent = new Intent(this, typeof(PostingDetailsActivity));
                intent.PutExtra("title", feedClient.postings[e.Position].Title);
                intent.PutExtra("description", feedClient.postings[e.Position].Description);
                StartActivity(intent);
            };
        }
    }
}

