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

		public SearchViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.Title = "Ethan's List";

            var locations = new AvailableLocations();

            for (int i = 0; i < locations.PotentialLocations.Count; i++)
            {
                Console.WriteLine(locations.PotentialLocations[i].Url);
            }

            SearchButton.Enabled = true;
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
            query = String.Format("min_price={0}&max_price={1}&bedrooms={3}&bathrooms{4}&query=(5)", 
                MinLabel.Text, MaxLabel.Text, MinBedLabel.Text, MinBathLabel.Text, SearchField.Text);
            
            return query;
        }
	}
}
