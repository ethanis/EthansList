
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

        Dictionary<string, string> searchTerms;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            searchTerms = new Dictionary<string, string>();
            searchTerms.Add("min_price", null);
            searchTerms.Add("max_price", null);
            searchTerms.Add("bedrooms", null);
            searchTerms.Add("bathrooms", null);
            searchTerms.Add("query", null);
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
            saveSearchButton.Enabled = true;

            minRentSeekBar.Progress = 10;
            maxRentSeekBar.Progress = 50;
            minRentTextView.Text = FormatCurrency(minRentSeekBar.Progress * 100);
            maxRentTextView.Text = FormatCurrency(maxRentSeekBar.Progress * 100);

            minBedroomPicker.MinValue = 0;
            minBedroomPicker.MaxValue = 10;

            minBathroomPicker.MinValue = 0;
            minBathroomPicker.MaxValue = 10;

            minRentSeekBar.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) => {
                minRentTextView.Text = FormatCurrency(minRentSeekBar.Progress * 100);
            };
            maxRentSeekBar.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) => {
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

        async void SaveSearchButton_Click (object sender, EventArgs e)
        {
            await MainActivity.databaseConnection.AddNewSearchAsync(location.Url, location.SiteName, minRentTextView.Text, 
                maxRentTextView.Text, minBedroomPicker.Value.ToString(), minBathroomPicker.Value.ToString(), searchTextField.Text);
            Console.WriteLine(MainActivity.databaseConnection.StatusMessage);
            if (MainActivity.databaseConnection.StatusCode == EthansList.Models.codes.ok)
            {
                Toast.MakeText(this.Activity, "Search saved successfully", ToastLength.Short).Show();
                saveSearchButton.Enabled = false;
            }
            else
            {
                Toast.MakeText(this.Activity, "Unable to save search. Please try again.", ToastLength.Short).Show();
                saveSearchButton.Enabled = true;
            }
        }

        protected string FormatCurrency(int i)
        {
            return String.Format("{0:C0}", i);
        }

        protected string GenerateQuery()
        {
            QueryGeneration helper = new QueryGeneration();
            searchTerms["min_price"] = minRentTextView.Text.Substring(1);
            searchTerms["max_price"] = maxRentTextView.Text.Substring(1);
            searchTerms["bedrooms"] = minBedroomPicker.Value.ToString();
            searchTerms["bathrooms"] = minBathroomPicker.Value.ToString();
            searchTerms["query"] = searchTextField.Text;

            string query = helper.Generate(location.Url, searchTerms);

            Console.WriteLine(query);

            return query;
        }
    }
}

