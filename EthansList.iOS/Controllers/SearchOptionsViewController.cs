using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Collections.Generic;
using System.Threading.Tasks;
using EthansList.Shared;
using EthansList.Models;
using System.Drawing;
using CoreGraphics;

namespace ethanslist.ios
{
	partial class SearchOptionsViewController : UIViewController
	{
        UIBarButtonItem saveButton;
        SearchOptionsTableSource tableSource;

        private float scroll_amount = 0.0f;    // amount to scroll 
        private float bottom = 0.0f;           // bottom point
        private float offset = 40.0f;          // extra offset
        private bool moveViewUp = false;           // which direction are we moving

        public string MinBedrooms { get; set; }
        public string MinBathrooms { get; set; }
        public string MinPrice { get; set; }
        public string MaxPrice { get; set; }
        public string SearchTerms { get; set; }
        public int MaxListings 
        { 
            get { return maxListings; } 
            set { maxListings = value; }
        }
        private int maxListings = 25;
        public int? WeeksOld { get; set; }
        public Location Location { get; set; }

        public UIView PickerPicked { get; set; }
        public CGRect PickerBounds { get; set; }

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
            tableSource = new SearchOptionsTableSource(GetTableSetup(), this);
            SearchTableView.Source = tableSource;

            var g = new UITapGestureRecognizer(() => View.EndEditing(true));
            View.AddGestureRecognizer(g);
            // Keyboard popup
            NSNotificationCenter.DefaultCenter.AddObserver
            (UIKeyboard.DidShowNotification,KeyBoardUpNotification);

            // Keyboard Down
            NSNotificationCenter.DefaultCenter.AddObserver
            (UIKeyboard.WillHideNotification,KeyBoardDownNotification);

            saveButton = new UIBarButtonItem (
                UIImage.FromFile ("save.png"),
                UIBarButtonItemStyle.Plain,
                async (sender, e) => {
                    await AppDelegate.databaseConnection.AddNewSearchAsync(Location.Url, Location.SiteName, MinPrice, MaxPrice, 
                    MinBedrooms, MinBathrooms, SearchTerms, WeeksOld, MaxListings);
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
                }
            );

            NavigationItem.RightBarButtonItem = saveButton;

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
                feedViewController.WeeksOld = WeeksOld;

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
                CellType = "PriceSelectorCell",
                PickerOptions = new List<PickerOptions> ()
                    {
                        new PickerOptions(){Options = new Dictionary<object, object>()
                            {
                                {0, "Any"},
                                {1, "400"},
                                {2, "600"},
                                {3, "800"},
                                {4, "1000"},
                                {5, "1200"},
                                {6, "1400"},
                                {7, "1600"},
                                {8, "1800"},
                                {9, "2000"},
                                {10, "2200"},
                                {11, "2400"},
                                {12, "2600"},
                            }},
                        new PickerOptions(){Options = new Dictionary<object, object>()
                            {
                                {0, "Any"},
                                {1, "1000"},
                                {2, "1400"},
                                {3, "1600"},
                                {4, "1800"},
                                {5, "2000"},
                                {6, "2200"},
                                {7, "2400"},
                                {8, "2600"},
                                {9, "2800"},
                                {10, "3000"},
                                {11, "3200"},
                                {12, "3400"},
                            }},
                    },
            });

            TableItemGroup options = new TableItemGroup()
                { 
                    Name = "Options",
                };
            options.Items.Add(new TableItem() {
                Heading = "Min Bedrooms",
                CellType = "PickerSelectorCell",
                PickerOptions = new List<PickerOptions> ()
                    {
                        new PickerOptions(){PickerWheelOptions = new Dictionary<object, KeyValuePair<object, object>>()
                            {
                                {0, new KeyValuePair<object, object>("Any", null)},
                                {1, new KeyValuePair<object, object>("1+","1")},
                                {2, new KeyValuePair<object, object>("2+","2")},
                                {3, new KeyValuePair<object, object>("3+","3")},
                                {4, new KeyValuePair<object, object>("4+","4")},
                            }}
                    },
            });
            options.Items.Add(new TableItem() {
                Heading = "Min Bathrooms",
                CellType = "PickerSelectorCell",
                PickerOptions = new List<PickerOptions> ()
                    {
                        new PickerOptions(){PickerWheelOptions = new Dictionary<object, KeyValuePair<object, object>>()
                            {
                                {0, new KeyValuePair<object, object>("Any", null)},
                                {1, new KeyValuePair<object, object>("1+","1")},
                                {2, new KeyValuePair<object, object>("2+","2")},
                                {3, new KeyValuePair<object, object>("3+","3")},
                                {4, new KeyValuePair<object, object>("4+","4")},
                            }}
                    },
            });
            options.Items.Add(new TableItem() {
                Heading = "Posted Date",
                CellType = "PickerSelectorCell",
                PickerOptions = new List<PickerOptions> ()
                    {
                        new PickerOptions(){PickerWheelOptions = new Dictionary<object, KeyValuePair<object, object>>()
                            {
                                {0, new KeyValuePair<object, object>("Any", null)},
                                {1, new KeyValuePair<object, object>("Today","-1")},
                                {2, new KeyValuePair<object, object>("1 Week Old","1")},
                                {3, new KeyValuePair<object, object>("2 Weeks Old","2")},
                                {4, new KeyValuePair<object, object>("3 Weeks Old","3")},
                                {5, new KeyValuePair<object, object>("4 Weeks Old","4")},
                            }}
                    },
            });
            options.Items.Add(new TableItem() {
                Heading = "Max Listings",
                CellType = "PickerSelectorCell",
                PickerOptions = new List<PickerOptions> ()
                    {
                        new PickerOptions(){PickerWheelOptions = new Dictionary<object, KeyValuePair<object, object>>()
                            {
                                {0, new KeyValuePair<object, object>(25, 25)},
                                {1, new KeyValuePair<object, object>(50, 50)},
                                {2, new KeyValuePair<object, object>(75, 75)},
                                {3, new KeyValuePair<object, object>(100, 100)},
                            }}
                    },
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
            scrollView.ContentSize = new CoreGraphics.CGSize(this.View.Bounds.Width, this.View.Bounds.Height);// + 280f);
            scrollView.Frame = this.View.Frame;

            //Seach CL Label Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(SearchCityLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .90f, 0),
                NSLayoutConstraint.Create(SearchCityLabel, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(SearchCityLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.TopMargin, 1, 79),
            });

            //Seach Button Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(SearchButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .90f, 0),
                NSLayoutConstraint.Create(SearchButton, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(SearchButton, NSLayoutAttribute.Top, NSLayoutRelation.Equal, SearchCityLabel, NSLayoutAttribute.Bottom, 1, 15),
            });

            //Seach Table Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(SearchTableView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, 1, 0),
                NSLayoutConstraint.Create(SearchTableView, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(SearchTableView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, SearchButton, NSLayoutAttribute.Bottom, 1, 15),
            });
        }

        private void KeyBoardUpNotification(NSNotification notification)
        {
            // Bottom of the controller = initial position + height + offset      
            bottom = (float)(PickerPicked.Frame.Y + PickerPicked.Frame.Height + offset);
            Console.WriteLine("Bottom: " + bottom);
            Console.WriteLine("Picker Input Y: " + PickerPicked.Frame.Y);
            Console.WriteLine("Picker Frame Height: " + PickerPicked.Frame.Height);
            Console.WriteLine("View Height: " + this.View.Frame.Height);

            //Added 180 for toolbar, navbar, constraints and padding height
            scroll_amount = (float)(PickerBounds.Height + (180) - (View.Frame.Size.Height - bottom)) ;
            Console.WriteLine("PickerBoungs Height: " + PickerBounds.Height);
            Console.WriteLine("View Frame Height: " + View.Frame.Size.Height);
            Console.WriteLine("Scroll Amount: " + scroll_amount);

            // Perform the scrolling
            if (scroll_amount > 0) {
                moveViewUp = true;
                ScrollTheView (moveViewUp);
            } else {
                moveViewUp = false;
            }

        }

        private void KeyBoardDownNotification(NSNotification notification)
        {
            if(moveViewUp){ScrollTheView(false);}
        }

        private void ScrollTheView(bool move)
        {
            // scroll the view up or down
            UIView.BeginAnimations (string.Empty, System.IntPtr.Zero);
            UIView.SetAnimationDuration (0.3);

            CGRect frame = View.Frame;

            if (move) {
                frame.Y -= scroll_amount;
            } else {
                frame.Y += scroll_amount;
                scroll_amount = 0;
            }

            View.Frame = frame;
            UIView.CommitAnimations();
        }
	}
}
