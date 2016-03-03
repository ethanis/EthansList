
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

namespace EthansList.MaterialDroid
{
    public class SearchResultsFragment : Fragment
    {
        //TODO set max listings and posted date too
        public string Query { get; set; }
        public int MaxListings { get; set; }
        public int? WeeksOld { get; set; }

        CLFeedClient feedClient;
        FeedResultsAdapter feedAdapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = new ListView(this.Activity);

            Console.WriteLine("Max Listings: " + MaxListings + ", Weeks Old: " +WeeksOld);
            feedClient = new CLFeedClient(Query, MaxListings, WeeksOld);
            var connected = feedClient.GetAllPostingsAsync();

            if (!connected)
            { 
                var builder = new Android.Support.V7.App.AlertDialog.Builder (this.Activity);

                builder.SetTitle("No internet connectiong")
                    .SetMessage("Please connect and try again")
                    .SetPositiveButton("Ok", delegate { Console.WriteLine("Yes"); });
                
                builder.Create().Show ();
            }

            var progressDialog = ProgressDialog.Show(this.Activity, "Please wait...", "Loading listings...", true);
            new Thread(new ThreadStart(delegate
            {
                //HIDE PROGRESS DIALOG
                feedClient.asyncLoadingComplete += (object sender, EventArgs e) => {
                    this.Activity.RunOnUiThread(() => {
                        progressDialog.Hide();
                    });
                    Console.WriteLine("NUM POSTINGS: " + feedClient.postings.Count);
                    feedAdapter = new FeedResultsAdapter(this.Activity, feedClient.postings);
                    this.Activity.RunOnUiThread(() => {
                        view.Adapter = feedAdapter;
                    });
                };

                feedClient.emptyPostingComplete += (object sender, EventArgs e) => {
                    this.Activity.RunOnUiThread(() => progressDialog.Hide());

                    var builder = new Android.Support.V7.App.AlertDialog.Builder(this.Activity);
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

            return view;
        }
    }
}

