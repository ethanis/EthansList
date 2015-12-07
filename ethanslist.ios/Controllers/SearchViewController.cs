using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Reflection;
using System.IO;
using EthansList.Models;
using EthansList.Shared;
using System.Collections.Generic;

namespace ethanslist.ios
{
	partial class SearchViewController : UIViewController
	{
        int minBed = 0;
        int minBath = 1;
        string url;
        string city;
        Dictionary<string, string> searchTerms = new Dictionary<string, string>();


		public SearchViewController (IntPtr handle) : base (handle)
		{
            searchTerms.Add("min_price", null);
            searchTerms.Add("max_price", null);
            searchTerms.Add("bedrooms", null);
            searchTerms.Add("bathrooms", null);
            searchTerms.Add("query", null);
		}

        public String Url {
            get {
                return url;
            }
            set {
                url = value;
            }
        }

        public String City {
            get { 
                return city;
            }
            set { 
                city = value;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.Title = "Ethan's List";
            searchCLLabel.Text = String.Format("Search {0} for:", city);

            SearchButton.Enabled = true;
            saveSearchButton.Enabled = true;
            MinLabel.Text = String.Format("{0:C0}", MinRentSlider.Value);
            MaxLabel.Text = String.Format("{0:C0}", MaxRentSlider.Value);
            MinBedLabel.Text = FormatBeds(minBed);
            MinBathLabel.Text = FormatBeds(minBath);

            MinRentSlider.ValueChanged += (object sender, EventArgs e) => {
                MinLabel.Text = String.Format("{0:C0}", MinRentSlider.Value);
            };

            MaxRentSlider.ValueChanged += (object sender, EventArgs e) => {
                MaxLabel.Text = String.Format("{0:C0}", MaxRentSlider.Value);
            };

            MinBedCountStep.ValueChanged += (object sender, EventArgs e) => {
                MinBedLabel.Text = FormatBeds((int)MinBedCountStep.Value);
            };

            MinBathCountStep.ValueChanged += (object sender, EventArgs e) => {
                MinBathLabel.Text = FormatBeds((int)MinBathCountStep.Value);
            };

            SearchField.EditingDidBegin += delegate { SearchField.BecomeFirstResponder(); };

            SearchField.ShouldReturn += delegate {
                SearchField.ResignFirstResponder();
                return true;
            };

            saveSearchButton.TouchUpInside += SaveSearchButton_TouchUpInside;
        }

        async void SaveSearchButton_TouchUpInside (object sender, EventArgs e)
        {
            await AppDelegate.databaseConnection.AddNewSearchAsync(url, city, MinLabel.Text, MaxLabel.Text, 
                MinBedLabel.Text.ToString(), MinBathLabel.Text.ToString(), SearchField.Text);
            Console.WriteLine(AppDelegate.databaseConnection.StatusMessage);

            if (AppDelegate.databaseConnection.StatusCode == codes.ok)
            {
                UIAlertView alert = new UIAlertView();
                alert.Message = "Search Saved!";
                alert.AddButton("OK");
                alert.Show();

                saveSearchButton.Enabled = false;
            }
            else
            {
                UIAlertView alert = new UIAlertView();
                alert.Message = String.Format("Oops, something went wrong{0}Please try again...", Environment.NewLine);
                alert.AddButton("OK");
                alert.Show();

                saveSearchButton.Enabled = true;
            }
        }

        partial void SearchCL(UIButton sender)
        {
            var storyboard = UIStoryboard.FromName("Main", null);
            var feedViewController = (FeedResultsTableViewController)storyboard.InstantiateViewController("FeedResultsTableViewController");

            QueryGeneration helper = new QueryGeneration();
            searchTerms["min_price"] = MinRentSlider.Value.ToString();
            searchTerms["max_price"] = MaxRentSlider.Value.ToString();
            searchTerms["bedrooms"] = MinBedCountStep.Value.ToString();
            searchTerms["bathrooms"] = MinBathCountStep.Value.ToString();
            searchTerms["query"] = SearchField.Text;

            feedViewController.Query = helper.Generate(url, searchTerms);

            this.ShowViewController(feedViewController, this);
        }

        public string FormatBeds(int beds)
        {
            return String.Format("{0}+", beds);
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            SearchField.ResignFirstResponder();
        }
	}
}
