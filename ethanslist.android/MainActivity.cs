using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Linq;
using EthansList.Shared;

namespace EthansList.Droid
{
    [Activity(Label = "ethanlist.android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        AvailableLocations locations;
        ListView cityPickerListView;
        ListView statePickerListView;
        CityListAdapter cityAdapter;
        StateListAdapter stateAdapter;
        String state;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            locations = new AvailableLocations();
            state = locations.States.ElementAt(0);

            SetContentView(Resource.Layout.Main);

            cityPickerListView = FindViewById<ListView>(Resource.Id.cityPickerListView);
            statePickerListView = FindViewById<ListView>(Resource.Id.statePickerListView);

            cityAdapter = new CityListAdapter(this, locations.PotentialLocations.Where(loc => loc.State == state));
            stateAdapter = new StateListAdapter(this, locations.States);

            cityPickerListView.Adapter = cityAdapter;
            statePickerListView.Adapter = stateAdapter;

            statePickerListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
                state = locations.States.ElementAt(e.Position);
                cityAdapter = new CityListAdapter(this, locations.PotentialLocations.Where(loc => loc.State == state));
                cityPickerListView.Adapter = cityAdapter;
            };

            cityPickerListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
                var intent = new Intent(this, typeof(SearchActivity));
                intent.PutExtra("city", locations.PotentialLocations.Where(loc => loc.State == state).ElementAt(e.Position).SiteName);
                StartActivity(intent);
            };
        }
    }
}