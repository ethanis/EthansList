using System;
using Android.App;
using Android.OS;
using Android.Widget;

namespace ethanslist.android
{
    [Activity(Label = "Search", Icon = "@drawable/icon")]
    public class SearchActivity : Activity
    {
        String city;
        TextView cityTextView;

        public SearchActivity()
        {
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Search);
            city = Intent.GetStringExtra("city");

            cityTextView = FindViewById<TextView>(Resource.Id.citySelectedText);
            cityTextView.Text = city;
        }
    }
}

