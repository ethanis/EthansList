
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
    public class SearchFragment : Fragment
    {
        public Location location { get; set; }
        TextView cityTextView;
        EditText searchTextField;
        Button searchButton;
        SeekBar minRentSeekBar;
        SeekBar maxRentSeekBar;
        TextView minRentTextView;
        TextView maxRentTextView;
        NumberPicker minBedroomPicker;
        NumberPicker minBathroomPicker;
        Button saveSearchButton;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Search, container, false);

            cityTextView = view.FindViewById<TextView>(Resource.Id.citySelectedText);
            cityTextView.Text = location.SiteName;

            searchTextField = view.FindViewById<EditText>(Resource.Id.searchTextField);
            searchButton = view.FindViewById<Button>(Resource.Id.searchButton);
            minRentSeekBar = view.FindViewById<SeekBar>(Resource.Id.minRentSeekBar);
            maxRentSeekBar = view.FindViewById<SeekBar>(Resource.Id.maxRentSeekBar);
            minRentTextView = view.FindViewById<TextView>(Resource.Id.minRentTextView);
            maxRentTextView = view.FindViewById<TextView>(Resource.Id.maxRentTextView);
            minBedroomPicker = view.FindViewById<NumberPicker>(Resource.Id.minBedroomPicker);
            minBathroomPicker = view.FindViewById<NumberPicker>(Resource.Id.minBathroomPicker);
            saveSearchButton = view.FindViewById<Button>(Resource.Id.saveSearchButton);

            minRentTextView.Text = FormatCurrency(1000);
            maxRentTextView.Text = FormatCurrency(5000);
            minRentSeekBar.Progress = 10;
            maxRentSeekBar.Progress = 50;

            minBedroomPicker.MinValue = 0;
            minBedroomPicker.MaxValue = 10;

            minBathroomPicker.MinValue = 0;
            minBathroomPicker.MaxValue = 10;

            minRentSeekBar.StopTrackingTouch += (object sender, SeekBar.StopTrackingTouchEventArgs e) => {
                minRentTextView.Text = FormatCurrency(minRentSeekBar.Progress * 100);
            };

            maxRentSeekBar.StopTrackingTouch += (object sender, SeekBar.StopTrackingTouchEventArgs e) => {
                maxRentTextView.Text = FormatCurrency(maxRentSeekBar.Progress * 100);
            };

            searchButton.Click += (sender, e) => {
                FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
                FeedResultsFragment feedResultsFragment = new FeedResultsFragment();
                feedResultsFragment.query = GenerateQuery();
                transaction.Replace(Resource.Id.frameLayout, feedResultsFragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };

            saveSearchButton.Click += SaveSearchButton_Click;

            return view;
        }

        void SaveSearchButton_Click (object sender, EventArgs e)
        {
            MainActivity.databaseConnection.AddNewSearchAsync(location.Url, location.SiteName, minRentTextView.Text, 
                maxRentTextView.Text, minBedroomPicker.Value.ToString(), minBathroomPicker.Value.ToString(), searchTextField.Text);
            Console.WriteLine(MainActivity.databaseConnection.StatusMessage);
        }

        protected string FormatCurrency(int i)
        {
            return String.Format("{0:C0}", i);
        }

        protected string GenerateQuery()
        {
            string query;
            query = String.Format("{0}/search/apa?format=rss&min_price={1}&max_price={2}&bedrooms={3}&bathrooms{4}&query={5}", 
                location.Url, minRentTextView.Text, maxRentTextView.Text, minBedroomPicker.Value, minBathroomPicker.Value, searchTextField.Text);

            Console.WriteLine(query);

            return query;
        }
    }
}

