
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using EthansList.Shared;

namespace EthansList.Droid
{
    public class SearchResultsFragment : Android.Support.V4.App.Fragment
    {
        public string Query { get; set; }
        public int MaxListings { get; set; }
        public int? WeeksOld { get; set; }

        CLFeedClient feedClient;
        FeedResultsAdapter feedAdapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = new ListView(this.Activity);

            Console.WriteLine("Max Listings: " + MaxListings + ", Weeks Old: " + WeeksOld);
            feedClient = new CLFeedClient(Query, MaxListings, WeeksOld);
            var connected = feedClient.GetAllPostingsAsync();

            if (!connected)
            {
                var builder = new Android.Support.V7.App.AlertDialog.Builder(this.Activity);

                builder.SetTitle("No internet connection")
                    .SetMessage("Please connect and try again")
                    .SetPositiveButton("Ok", delegate
                    {
                        this.FragmentManager.PopBackStack();
                    });

                builder.Create().Show();
            }
            else
            {
                var progressDialog = ProgressDialog.Show(this.Activity, "Please wait...", "Loading listings...", true);
                new Thread(new ThreadStart(delegate
                {
                    //HIDE PROGRESS DIALOG
                    feedClient.asyncLoadingComplete += (object sender, EventArgs e) =>
                        {
                            this.Activity.RunOnUiThread(() =>
                            {
                                progressDialog.Hide();
                            });
                            Console.WriteLine("NUM POSTINGS: " + feedClient.postings.Count);
                            feedAdapter = new FeedResultsAdapter(this.Activity, feedClient.postings);
                            this.Activity.RunOnUiThread(() =>
                            {
                                view.Adapter = feedAdapter;
                            });
                        };

                    feedClient.emptyPostingComplete += (object sender, EventArgs e) =>
                    {
                        this.Activity.RunOnUiThread(() => progressDialog.Hide());

                        var builder = new Android.Support.V7.App.AlertDialog.Builder(this.Activity);
                        builder.SetTitle("Error loading listings");
                        builder.SetMessage(String.Format("No listings found.{0}Try a different search", System.Environment.NewLine));
                        builder.SetPositiveButton("Ok", delegate
                        {
                            this.FragmentManager.PopBackStack();
                        });

                        this.Activity.RunOnUiThread(() =>
                        {
                            builder.Create().Show();
                        });
                    };

                })).Start();
            }

            view.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
            {
                var transaction = this.Activity.SupportFragmentManager.BeginTransaction();
                PostingDetailsFragment postingDetailsFragment = new PostingDetailsFragment();
                postingDetailsFragment.Posting = feedClient.postings[e.Position];
                transaction.Replace(Resource.Id.frameLayout, postingDetailsFragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };

            return view;
        }
    }
}

