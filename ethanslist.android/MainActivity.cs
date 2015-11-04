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
        ArrayAdapter<Location> locationAdapter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            locations = new AvailableLocations();
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            ListView cityPickerListView = FindViewById<ListView>(Resource.Id.cityPickerListView);
            ListView statePickerListView = FindViewById<ListView>(Resource.Id.statePickerListView);
            locationAdapter = new ArrayAdapter<Location>(this, Android.Resource.Layout.SimpleListItem1, locations.PotentialLocations);

            cityPickerListView.Adapter = locationAdapter;
            statePickerListView.Adapter = locationAdapter;
        }
    }
}