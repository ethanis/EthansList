using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Collections.Generic;
using System.Threading.Tasks;
using EthansList.Shared;
using EthansList.Models;

namespace ethanslist.ios
{
	partial class SearchOptionsViewController : UIViewController
	{
        public string MinBedrooms { get; set; }
        public string MinBathrooms { get; set; }
        public string MinPrice { get; set; }
        public string MaxPrice { get; set; }
        public string SearchTerms { get; set; }
        public int MaxListings { get; set; }

        public Location Location { get; set; }

		public SearchOptionsViewController (IntPtr handle) : base (handle)
		{
		}

        public override void LoadView()
        {
            base.LoadView();

            this.View.Layer.BackgroundColor = ColorScheme.Clouds.CGColor;

            SearchButton.Layer.BackgroundColor = ColorScheme.MidnightBlue.CGColor;
            SearchButton.SetTitleColor(ColorScheme.Clouds, UIControlState.Normal);
            SearchButton.Layer.CornerRadius = 10;
            SearchButton.ClipsToBounds = true;

            SearchTableView.Layer.BackgroundColor = ColorScheme.Clouds.CGColor;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            AddLayoutConstraints();

            this.Title = "Options";
            SearchCityLabel.Text = String.Format("Search {0} for:", Location.SiteName);

            SearchTableView.Source = new SearchOptionsTableSource(GetTableSetup(), this);

            var g = new UITapGestureRecognizer(() => View.EndEditing(true));
            View.AddGestureRecognizer(g);

            this.NavigationItem.SetRightBarButtonItem(
                new UIBarButtonItem(UIBarButtonSystemItem.Save, async (sender, e) => {
                    await AppDelegate.databaseConnection.AddNewSearchAsync(Location.Url, Location.SiteName, MinPrice, MaxPrice, 
                        MinBedrooms, MinBathrooms, SearchTerms);
                    Console.WriteLine(AppDelegate.databaseConnection.StatusMessage);

                    if (AppDelegate.databaseConnection.StatusCode == codes.ok)
                    {
                        UIAlertView alert = new UIAlertView();
                        alert.Message = "Search Saved!";
                        alert.AddButton("OK");
                        alert.Show();

                        this.NavigationItem.RightBarButtonItem.Enabled = false;
                    }
                    else
                    {
                        UIAlertView alert = new UIAlertView();
                        alert.Message = String.Format("Oops, something went wrong{0}Please try again...", Environment.NewLine);
                        alert.AddButton("OK");
                        alert.Show();

                        this.NavigationItem.RightBarButtonItem.Enabled = true;
                    }
                }),
                true);

            SearchButton.TouchUpInside += (sender, e) => {
                QueryGeneration queryHelper = new QueryGeneration();
                var query = queryHelper.Generate(Location.Url, new Dictionary<string, string>()
                    {
                        {"min_price", MinPrice},
                        {"max_price", MaxPrice},
                        {"bedrooms", MinBedrooms},
                        {"bathrooms", MinBathrooms},
                        {"query", SearchTerms}
                    }
                );
                Console.WriteLine (query);

                var storyboard = UIStoryboard.FromName("Main", null);
                var feedViewController = (FeedResultsTableViewController)storyboard.InstantiateViewController("FeedResultsTableViewController");

                feedViewController.Query = query;
                feedViewController.MaxListings = MaxListings;

                this.ShowViewController(feedViewController, this);
            };
        }

        private List<TableItemGroup> GetTableSetup()
        {
            List<TableItemGroup> tableItems = new List<TableItemGroup>();

            TableItemGroup searchterms = new TableItemGroup()
                { Name = "Search Terms"};
            searchterms.Items.Add(new TableItem() { 
                Heading = "Search Terms",
                CellType = "SearchTermsCell",
            });
            searchterms.Items.Add(new TableItem() {
                Heading = "Price",
                CellType = "PriceSelectorCell"
            });

            TableItemGroup options = new TableItemGroup()
                { 
                    Name = "Options",
                };
            options.Items.Add(new TableItem() {
                Heading = "Min Bedrooms",
                CellType = "BedBathCell",
                ActionOptions = new Dictionary<string, string>() 
                {
                    {"Any", "Any"},
                    {"Studio", "0"},
                    {"1+", "1"},
                    {"2+", "2"},
                    {"3+", "3"},
                    {"4+", "4"},
                }
            });
            options.Items.Add(new TableItem() {
                Heading = "Min Bathrooms",
                CellType = "BedBathCell",
                ActionOptions = new Dictionary<string, string>() 
                {
                    {"Any", "Any"},
                    {"1+", "1"},
                    {"2+", "2"},
                    {"3+", "3"},
                }
            });
            options.Items.Add(new TableItem() {
                Heading = "Max Listings",
                CellType = "BedBathCell",
                SubHeading = "25",
                ActionOptions = new Dictionary<string, string>() 
                {
                    {"25", "25"},
                    {"50", "50"},
                    {"75", "75"},
                    {"100", "100"},
                }
            });

            tableItems.Add(searchterms);
            tableItems.Add(options);

            return tableItems;
        }

        void AddLayoutConstraints()
        {
            scrollView.TranslatesAutoresizingMaskIntoConstraints = false;
            SearchCityLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            SearchButton.TranslatesAutoresizingMaskIntoConstraints = false;
            SearchTableView.TranslatesAutoresizingMaskIntoConstraints = false;

            //Scroll View Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(scrollView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, 1, 0),
                NSLayoutConstraint.Create(scrollView, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(scrollView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Top, 1, 20),
                NSLayoutConstraint.Create(scrollView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Bottom, 1, 0),
            });

            //Seach CL Label Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(SearchCityLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.Width, .90f, 0),
                NSLayoutConstraint.Create(SearchCityLabel, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(SearchCityLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.Top, 1, 20),
            });

            //Seach Button Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(SearchButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.Width, .90f, 0),
                NSLayoutConstraint.Create(SearchButton, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(SearchButton, NSLayoutAttribute.Top, NSLayoutRelation.Equal, SearchCityLabel, NSLayoutAttribute.Bottom, 1, 20),
            });

            //Seach Table Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(SearchTableView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.Width, 1, 0),
                NSLayoutConstraint.Create(SearchTableView, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(SearchTableView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, SearchButton, NSLayoutAttribute.Bottom, 1, 20),
                NSLayoutConstraint.Create(SearchTableView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.Bottom, 1, 0),
            });
        }
	}
}
