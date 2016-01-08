
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
        List<RecentCity> recentCityList;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
            var view = inflater.Inflate(Resource.Layout.RecentCities, container, false);

            recentCityList = MainActivity.databaseConnection.GetAllRecentCitiesAsync().Result;
            recentCityList.Sort((s1, s2)=>s2.Updated.CompareTo(s1.Updated));
            recentCitiesListView = view.FindViewById<ListView>(Resource.Id.recentCitiesList);

            recentCitiesListView.Adapter = new ArrayAdapter<String>(this.Activity, Android.Resource.Layout.SimpleListItem1, recentCityList.Select(x => x.City).ToList());

            recentCitiesListView.ItemClick += (sender, e) => {
                FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
                SearchFragment searchFragment = new SearchFragment();
                AvailableLocations locations = new AvailableLocations();
                searchFragment.location = locations.PotentialLocations.Where(loc => loc.SiteName.Equals(recentCityList[e.Position].City)).First();
                transaction.Replace(Resource.Id.frameLayout, searchFragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };

            return view;
        }
    }
}

