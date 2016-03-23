using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;
using EthansList.Shared;
using EthansList.Models;

namespace EthansList.Droid
{
	[Activity(Label = "EthansList", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : AppCompatActivity
	{
		DrawerLayout drawerLayout;
        public static DatabaseConnection databaseConnection { get; set; }
        readonly Android.Support.V4.App.Fragment[] fragments = { new SelectCityFragment() , new RecentCityFragment(), new SavedPostingsFragment(), new SavedSearchesFragment()};
        readonly string[] titles = { "Select City", "Recent Cities", "Saved Postings", "Saved Searches" };

        public IMenu Menu { get; private set;}
        public event EventHandler<OptionItemEventArgs> OptionItemSelected;

        public static Context Instance { get; private set;}

        protected override void OnCreate(Bundle bundle)
		{			
            base.OnCreate(bundle);
            Instance = this;

            string dbpath = FileAccessHelper.GetLocalFilePath("ethanslist.db3");
            databaseConnection = new DatabaseConnection(dbpath);

            // Create UI
            SetContentView(Resource.Layout.Main);
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            // Init toolbar
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);   

            // Attach item selected handler to navigation view
            var navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

            // Create ActionBarDrawerToggle button and add it to the toolbar
            var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.open_drawer, Resource.String.close_drawer);
            drawerLayout.SetDrawerListener(drawerToggle);
            drawerToggle.SyncState();

            base.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.frameLayout, fragments[0]).Commit();
		}

        public override void OnBackPressed()
        {
            //this clears the event handler for lingering subscriptions
            OptionItemSelected = null;
            base.OnBackPressed();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.Save, menu);
            Menu = menu;

            var save_button = Menu.FindItem(Resource.Id.save_action_button);
            save_button.SetVisible(false);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (this.OptionItemSelected != null)
                this.OptionItemSelected(this, new OptionItemEventArgs { Item = item });
            return base.OnOptionsItemSelected(item);
        }

		void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
		{
            int position = -1;
            switch (e.MenuItem.ItemId)
            {
                case (Resource.Id.city_picker):
                    position = 0;
                    break;
                case (Resource.Id.recent_cities):
                    position = 1;
                    break;
                case (Resource.Id.saved_postings):
                    position = 2;
                    break;
                case (Resource.Id.saved_searches):
                    position = 3;
                    break;   
            }

            base.SupportFragmentManager.PopBackStack(null, (int)PopBackStackFlags.Inclusive);
            // Show the selected Fragment to the user
            base.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.frameLayout, fragments[position]).Commit();

            // Update the Activity title in the ActionBar
            this.Title = titles[position];

            // Close drawer
            drawerLayout.CloseDrawers();
		}
	}

    public class OptionItemEventArgs : EventArgs
    {
        public IMenuItem Item { get; set; }

        //public static explicit operator OptionItemEventArgs(Delegate v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}