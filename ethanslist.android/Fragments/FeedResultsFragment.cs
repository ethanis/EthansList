
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
using EthansList.Shared;
using System.Threading;

namespace ethanslist.android
{
    public class FeedResultsFragment : Fragment
    {
        ListView feedResultsListView;
        PostingListAdapter postingListAdapter;
        CLFeedClient feedClient;
        public String query { get; set;}


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.FeedResults, container, false);

            feedClient = new CLFeedClient(query);
            var progressDialog = ProgressDialog.Show(this.Activity, "Please wait...", "Loading listings...", true);
            new Thread(new ThreadStart(delegate
                {
                    //HIDE PROGRESS DIALOG
                    feedClient.loadingComplete += (object sender, EventArgs e) => {
                        this.Activity.RunOnUiThread(() => {
                            progressDialog.Hide();
                        });
                        Console.WriteLine(feedClient.postings.Count);
                        postingListAdapter = new PostingListAdapter(this.Activity, feedClient.postings);
                        feedResultsListView.Adapter = postingListAdapter;
                    };

                    feedClient.emptyPostingComplete += (object sender, EventArgs e) => {
                        this.Activity.RunOnUiThread(() => progressDialog.Hide());

                        AlertDialog.Builder builder = new AlertDialog.Builder(this.Activity);
                        Dialog dialog;
                        builder.SetTitle("Error loading listings");
                        builder.SetMessage(String.Format("No listings found.{0}Try a different search", System.Environment.NewLine));
                        builder.SetPositiveButton("Ok", delegate {
                            this.FragmentManager.PopBackStack();
                        });
                        dialog = builder.Create();

                        this.Activity.RunOnUiThread(() => {
                            dialog.Show();
                        });
                    };

                })).Start();

            feedResultsListView = view.FindViewById<ListView>(Resource.Id.feedResultsListView);

            feedResultsListView.ItemClick += (sender, e) => {
                FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
                PostingDetailsFragment postingDetailsFragment = new PostingDetailsFragment();
                postingDetailsFragment.posting = feedClient.postings[e.Position];
                transaction.Replace(Resource.Id.frameLayout, postingDetailsFragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };

            return view;
        }
    }
}

