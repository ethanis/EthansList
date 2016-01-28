
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
using PullToRefresharp.Android.Views;

namespace ethanslist.android
{
    public class FeedResultsFragment : Fragment
    {
        private IPullToRefresharpView ptr_list_view;
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
            feedResultsListView = view.FindViewById<ListView>(Resource.Id.feedResultsListView);

            if (feedResultsListView is IPullToRefresharpView)
                ptr_list_view = (IPullToRefresharpView)feedResultsListView;

            feedClient = new CLFeedClient(query);
            var result = feedClient.GetAllPostingsAsync();
            Console.WriteLine(result);
            var progressDialog = ProgressDialog.Show(this.Activity, "Please wait...", "Loading listings...", true);
            new Thread(new ThreadStart(delegate
                {
                    //HIDE PROGRESS DIALOG
                    feedClient.asyncLoadingComplete += (object sender, EventArgs e) => {
                        this.Activity.RunOnUiThread(() => {
                            progressDialog.Hide();
                        });
                        Console.WriteLine("NUM POSTINGS: " + feedClient.postings.Count);
                        postingListAdapter = new PostingListAdapter(this.Activity, feedClient.postings);
                        this.Activity.RunOnUiThread(() => {
                            feedResultsListView.Adapter = postingListAdapter;
                        });
                    };

                    //TODO: Reload data on asyncLoadingPartlyComplete

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

            ptr_list_view.RefreshActivated += (object sender, EventArgs e) => {
                feedClient = new CLFeedClient(query);
                feedClient.asyncLoadingComplete += FeedCompletedRefreshing;
            };

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

        void FeedCompletedRefreshing(object s, EventArgs e)
        {
            postingListAdapter = new PostingListAdapter(this.Activity, feedClient.postings);
            feedResultsListView.Adapter = postingListAdapter;
            ptr_list_view.OnRefreshCompleted();
        }
    }
}

