
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
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = new SavedPostingsView(Activity, true);

            return view;
        }
    }

    public class SavedPostingsView : ListView
    {
        readonly Context _context;
        FeedResultsAdapter adapter;
        bool _deleteable;

        public SavedPostingsView(Context context, bool deleteable = false)
            : base(context)
        {
            _context = context;
            _deleteable = deleteable;
            Initialize();
        }

        void Initialize()
        {
            Adapter = adapter = new FeedResultsAdapter(_context,
                                                new ObservableCollection<Posting>(MainActivity.databaseConnection.GetAllPostingsAsync().Result));

            ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
            {
                var transaction = ((MainActivity)_context).SupportFragmentManager.BeginTransaction();
                PostingDetailsFragment postingDetailsFragment = new PostingDetailsFragment();
                postingDetailsFragment.Posting = adapter.Postings[e.Position];
                transaction.Replace(Resource.Id.frameLayout, postingDetailsFragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };

            if (_deleteable)
            {
                ItemLongClick += (sender, e) =>
                {
                    PopupMenu menu = new PopupMenu(_context, GetChildAt(e.Position));
                    menu.Inflate(Resource.Menu.DeleteMenu);
                    menu.Show();

                    menu.MenuItemClick += (se, args) =>
                    {
                        Console.WriteLine("Deleting posting index: " + e.Position);
                        lock (adapter.Postings)
                        {
                            var del = MainActivity.databaseConnection.DeletePostingAsync(adapter.Postings[e.Position]).Result;

                            ((Activity)_context).RunOnUiThread(() => adapter.Postings.RemoveAt(e.Position));

                            adapter.NotifyDataSetChanged();
                        }
                        Console.WriteLine(MainActivity.databaseConnection.StatusMessage);
                    };
                };
            };
        }
    }

}


