using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Reflection;
using System.IO;

namespace ethanslist.ios
{
	partial class SearchViewController : UIViewController
	{
        int minBed = 0;
        int minBath = 1;
        string url;
        string city;

		public SearchViewController (IntPtr handle) : base (handle)
		{
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

            SearchField.EditingDidBegin += delegate
            {
                    SearchField.ResignFirstResponder();
            };

            saveSearchButton.TouchUpInside += SaveSearchButton_TouchUpInside;
        }

        void SaveSearchButton_TouchUpInside (object sender, EventArgs e)
        {
            AppDelegate.databaseConnection.AddNewSearchAsync(url, city, MinLabel.Text, MaxLabel.Text, 
                MinBedLabel.Text.ToString(), MinBathLabel.Text.ToString(), SearchField.Text);
            Console.WriteLine(AppDelegate.databaseConnection.StatusMessage);
            saveSearchButton.Enabled = false;
        }

        partial void SearchCL(UIButton sender)
        {
            var storyboard = UIStoryboard.FromName("Main", null);
            var feedViewController = (FeedResultsTableViewController)storyboard.InstantiateViewController("FeedResultsTableViewController");

            feedViewController.Query = GenerateQuery();

            this.ShowViewController(feedViewController, this);
        }

        public string FormatBeds(int beds)
        {
            return String.Format("{0}+", beds);
        }

        public string GenerateQuery()
        {
            string query;
            query = String.Format("{0}/search/apa?format=rss&min_price={1}&max_price={2}&bedrooms={3}&bathrooms{4}&query={5}", 
                url, MinLabel.Text, MaxLabel.Text, MinBedLabel.Text, MinBathLabel.Text, SearchField.Text);

            Console.WriteLine(query);

            return query;
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            SearchField.ResignFirstResponder();
        }
	}
}
