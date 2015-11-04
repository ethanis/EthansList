using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;

namespace ethanslist.android
{
    [Activity(Label = "ethanslist.android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        AvailableLocations locations;
        ListView cityPickerListView;
        ListView statePickerListView;
        CityListAdapter cityAdapter;
        StateListAdapter stateAdapter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            locations = new AvailableLocations();
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            cityPickerListView = FindViewById<ListView>(Resource.Id.cityPickerListView);
            statePickerListView = FindViewById<ListView>(Resource.Id.statePickerListView);

            cityAdapter = new CityListAdapter(this, locations.PotentialLocations);
            stateAdapter = new StateListAdapter(this, locations.States);

            cityPickerListView.Adapter = cityAdapter;
            statePickerListView.Adapter = stateAdapter;
        }
    }
}