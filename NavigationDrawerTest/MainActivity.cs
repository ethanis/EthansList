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

namespace EthansList.MaterialDroid
{
	[Activity(Label = "EthansList", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : AppCompatActivity
	{
		DrawerLayout drawerLayout;
        public static DatabaseConnection databaseConnection { get; set; }
        readonly Fragment[] fragments = { new SelectCityFragment() };//, new RecentCitiesFragment(), new SavedPostingsFragment(), new SavedSearchesFragment()};
        readonly string[] titles = { "Select City", "Recent Cities", "Saved Postings", "Saved Searches" };

        protected override void OnCreate(Bundle bundle)
		{			
			base.OnCreate(bundle);

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
                    position = 0;
                    break;
                case (Resource.Id.saved_postings):
                    position = 0;
                    break;
                case (Resource.Id.saved_searches):
                    position = 0;
                    break;   
            }

            base.FragmentManager.PopBackStack(null, PopBackStackFlags.Inclusive);
            // Show the selected Fragment to the user
            base.FragmentManager.BeginTransaction().Replace(Resource.Id.drawer_layout, fragments[position]).Commit();

            // Update the Activity title in the ActionBar
            this.Title = titles[position];

            // Close drawer
            drawerLayout.CloseDrawers();

            var builder = new Android.Support.V7.App.AlertDialog.Builder (this);

            builder.SetTitle (titles[position])
            .SetMessage ("Is this material design?")
            .SetPositiveButton ("Yes", delegate { Console.WriteLine("Yes"); })
            .SetNegativeButton ("No", delegate { Console.WriteLine("No"); }); 

            builder.Create().Show ();
		}
	}
}