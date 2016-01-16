using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Collections.Generic;
using System.Threading.Tasks;
using EthansList.Shared;

namespace ethanslist.ios
{
	partial class SearchOptionsViewController : UIViewController
	{
        protected List<TableItemGroup> tableItems;

        public string MinBedrooms { get; set; }
        public string MinBathrooms { get; set; }
        public string MinPrice { get; set; }
        public string MaxPrice { get; set; }
        public string SearchTerms { get; set; }

        public Location Location { get; set; }

		public SearchOptionsViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            tableItems = new List<TableItemGroup>();

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

            tableItems.Add(searchterms);
            tableItems.Add(options);

            SearchTableView.Source = new SearchOptionsTableSource(tableItems, this);

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

                this.ShowViewController(feedViewController, this);
            };
        }
	}
}
