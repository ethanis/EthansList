
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

namespace ethanslist.android
{
    public class SelectCityFragment : Fragment
    {
        AvailableLocations locations;
        ListView cityPickerListView;
        ListView statePickerListView;
        CityListAdapter cityAdapter;
        StateListAdapter stateAdapter;
        String state;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.SelectCity, container, false);

            locations = new AvailableLocations();
            state = locations.States.ElementAt(0);

            cityPickerListView = view.FindViewById<ListView>(Resource.Id.cityPickerListView);
            statePickerListView = view.FindViewById<ListView>(Resource.Id.statePickerListView);

            cityAdapter = new CityListAdapter(this.Activity, locations.PotentialLocations.Where(loc => loc.State == state));
            stateAdapter = new StateListAdapter(this.Activity, locations.States);

            cityPickerListView.Adapter = cityAdapter;
            statePickerListView.Adapter = stateAdapter;

            statePickerListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
                state = locations.States.ElementAt(e.Position);
                cityAdapter = new CityListAdapter(this.Activity, locations.PotentialLocations.Where(loc => loc.State == state));
                cityPickerListView.Adapter = cityAdapter;
            };

            cityPickerListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
                FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
                SearchFragment searchFragment = new SearchFragment();
                searchFragment.location = locations.PotentialLocations.Where(loc => loc.State == state).ElementAt(e.Position);
                transaction.Replace(Resource.Id.frameLayout, searchFragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };

            return view;
        }
    }
}

