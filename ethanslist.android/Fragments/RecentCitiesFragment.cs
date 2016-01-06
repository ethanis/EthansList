
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
using EthansList.Models;
using EthansList.Shared;

namespace ethanslist.android
{
    public class RecentCitiesFragment : Fragment
    {
        ListView recentCitiesListView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var view = inflater.Inflate(Resource.Layout.RecentCities, container, false);

            var recentCities = MainActivity.databaseConnection.GetAllRecentCitiesAsync().Result.Select(x => x.City).ToList();
            recentCities.Reverse();
            recentCitiesListView = view.FindViewById<ListView>(Resource.Id.recentCitiesList);

            recentCitiesListView.Adapter = new ArrayAdapter<String>(this.Activity, Android.Resource.Layout.SimpleListItem1, recentCities);

            recentCitiesListView.ItemClick += (sender, e) => {
                FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
                SearchFragment searchFragment = new SearchFragment();
                AvailableLocations locations = new AvailableLocations();
                searchFragment.location = locations.PotentialLocations.Where(loc => loc.SiteName == recentCities[e.Position]).First();
                transaction.Replace(Resource.Id.frameLayout, searchFragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };

            return view;
        }
    }
}

