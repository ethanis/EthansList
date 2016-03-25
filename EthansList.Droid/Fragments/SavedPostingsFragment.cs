
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        FeedResultsAdapter adapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = new ListView(Activity);

            view.Adapter = adapter = new FeedResultsAdapter(Activity,
                                                            new ObservableCollection<Posting>(MainActivity.databaseConnection.GetAllPostingsAsync().Result),
                                                            true);

            view.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
            {
                var transaction = this.Activity.SupportFragmentManager.BeginTransaction();
                PostingDetailsFragment postingDetailsFragment = new PostingDetailsFragment();
                postingDetailsFragment.Posting = adapter.Postings[e.Position];
                transaction.Replace(Resource.Id.frameLayout, postingDetailsFragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };

            adapter.ItemDeleted += (sender, e) =>
            {
                Console.WriteLine("Deleting posting index: " + e.Index);
                lock (adapter.Postings)
                {
                    var del = MainActivity.databaseConnection.DeletePostingAsync(adapter.Postings[e.Index]).Result;

                    Activity.RunOnUiThread(() => adapter.Postings.RemoveAt(e.Index));

                    adapter.NotifyDataSetChanged();
                }
                Console.WriteLine(MainActivity.databaseConnection.StatusMessage);
            };

            return view;
        }
    }
}

