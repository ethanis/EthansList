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
        private bool keyboardSet = false;

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
        public string SubCategory { get; set; }
        public Location Location { get; set; }
        public KeyValuePair<string, string> Category {get;set;}
        public UIView FieldSelected { get; set; }
        public CGRect KeyboardBounds { get; set; }

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
            SearchButton.SetAttributedTitle(new NSAttributedString(SearchButton.TitleLabel.Text, Constants.ButtonAttributes), UIControlState.Normal);

            SearchTableView.Layer.BackgroundColor = ColorScheme.Clouds.CGColor;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            AddLayoutConstraints();

            this.Title = "Options";
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
                    //TODO: Way to save selected category
                    await AppDelegate.databaseConnection.AddNewSearchAsync(Location.Url, Location.SiteName, MinPrice, MaxPrice, 
                    MinBedrooms, MinBathrooms, SearchTerms, WeeksOld, MaxListings, Category.Key, Category.Value);
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
                var cat = SubCategory != null ? SubCategory : Category.Key;
                var query = queryHelper.Generate(Location.Url, cat, new Dictionary<string, string>()
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
            searchterms.Items.Add(new TableItem(){
                Heading = "Price",
                CellType = "PriceInputCell",
            });
//            searchterms.Items.Add(new TableItem() {
//                Heading = "Price",
//                CellType = "PriceSelectorCell",
//                PickerOptions = new List<PickerOptions> ()
//                    {
//                        new PickerOptions(){Options = new Dictionary<object, object>()
//                            {
//                                {0, "Any"},
//                                {1, "400"},
//                                {2, "600"},
//                                {3, "800"},
//                                {4, "1000"},
//                                {5, "1200"},
//                                {6, "1400"},
//                                {7, "1600"},
//                                {8, "1800"},
//                                {9, "2000"},
//                                {10, "2200"},
//                                {11, "2400"},
//                                {12, "2600"},
//                            }},
//                        new PickerOptions(){Options = new Dictionary<object, object>()
//                            {
//                                {0, "Any"},
//                                {1, "1000"},
//                                {2, "1200"},
//                                {3, "1400"},
//                                {4, "1600"},
//                                {5, "1800"},
//                                {6, "2000"},
//                                {7, "2200"},
//                                {8, "2400"},
//                                {9, "2600"},
//                                {10, "2800"},
//                                {11, "3000"},
//                                {12, "3200"},
//                                {13, "3400"},
//                            }},
//                    },
//            });

            TableItemGroup options = new TableItemGroup()
                { 
                    Name = "Options",
                };

            if (Categories.SubCategories.ContainsKey(Category.Key))
            {
                options.Items.Add(new TableItem()
                    {
                        Heading = "Sub Category",
                        CellType = "PickerSelectorCell",
                        PickerOptions = new List<PickerOptions>()
                        {
                            new PickerOptions() {PickerWheelOptions = Categories.SubCategories[Category.Key]}
                        },
                    });
            }

            if (Categories.Housing.Contains(Category.Key))
            {
                options.Items.Add(new TableItem()
                    {
                        Heading = "Min Bedrooms",
                        CellType = "PickerSelectorCell",
                        PickerOptions = new List<PickerOptions>()
                        {
                            new PickerOptions()
                            {PickerWheelOptions = new List<KeyValuePair<object, object>>()
                                {
                                    new KeyValuePair<object, object>("Any", null),
                                    new KeyValuePair<object, object>("1+", "1"),
                                    new KeyValuePair<object, object>("2+", "2"),
                                    new KeyValuePair<object, object>("3+", "3"),
                                    new KeyValuePair<object, object>("4+", "4"),
                                }
                            }
                        },
                    });
                options.Items.Add(new TableItem()
                    {
                        Heading = "Min Bathrooms",
                        CellType = "PickerSelectorCell",
                        PickerOptions = new List<PickerOptions>()
                        {
                            new PickerOptions()
                            {PickerWheelOptions = new List<KeyValuePair<object, object>>()
                                {
                                    new KeyValuePair<object, object>("Any", null),
                                    new KeyValuePair<object, object>("1+", "1"),
                                    new KeyValuePair<object, object>("2+", "2"),
                                    new KeyValuePair<object, object>("3+", "3"),
                                    new KeyValuePair<object, object>("4+", "4"),
                                }
                            }
                        },
                    });
                options.Items.Add(new TableItem()
                    {
                        Heading = "Posted Date",
                        CellType = "PickerSelectorCell",
                        PickerOptions = new List<PickerOptions>()
                        {
                            new PickerOptions()
                            {PickerWheelOptions = new List<KeyValuePair<object, object>>()
                                {
                                    new KeyValuePair<object, object>("Any", null),
                                    new KeyValuePair<object, object>("Today", "-1"),
                                    new KeyValuePair<object, object>("1 Week Old", "1"),
                                    new KeyValuePair<object, object>("2 Weeks Old", "2"),
                                    new KeyValuePair<object, object>("3 Weeks Old", "3"),
                                    new KeyValuePair<object, object>("4 Weeks Old", "4"),
                                }
                            }
                        },
                    });
            }
            options.Items.Add(new TableItem() {
                Heading = "Max Listings",
                CellType = "PickerSelectorCell",
                PickerOptions = new List<PickerOptions> ()
                    {
                        new PickerOptions(){PickerWheelOptions = new List<KeyValuePair<object, object>>()
                            {
                                new KeyValuePair<object, object>(25, 25),
                                new KeyValuePair<object, object>(50, 50),
                                new KeyValuePair<object, object>(75, 75),
                                new KeyValuePair<object, object>(100, 100),
                            }}
                    },
            });

            tableItems.Add(searchterms);
            tableItems.Add(options);

            return tableItems;
        }

        void AddLayoutConstraints()
        {
            this.View.RemoveConstraints(scrollView.Constraints);

            SearchCityLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            SearchButton.TranslatesAutoresizingMaskIntoConstraints = false;
            SearchTableView.TranslatesAutoresizingMaskIntoConstraints = false;
            scrollView.TranslatesAutoresizingMaskIntoConstraints = false;

            SearchCityLabel.AttributedText = new NSAttributedString(String.Format("Search {0} for:", Location.SiteName), Constants.LabelAttributes);

            //Scrollview Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(scrollView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, 1, 0),
                NSLayoutConstraint.Create(scrollView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Height, 1, 0),
                NSLayoutConstraint.Create(scrollView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.TopMargin, 1, 20),
                NSLayoutConstraint.Create(scrollView, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Left, 1, 0),
            });

            //Seach CL Label Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(SearchCityLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.Width, .90f, 0),
                NSLayoutConstraint.Create(SearchCityLabel, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(SearchCityLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.TopMargin, 1, 0),
            });

            //Seach Button Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(SearchButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.Width, .90f, 0),
                NSLayoutConstraint.Create(SearchButton, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(SearchButton, NSLayoutAttribute.Top, NSLayoutRelation.Equal, SearchCityLabel, NSLayoutAttribute.Bottom, 1, 15),
                NSLayoutConstraint.Create(SearchButton, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, Constants.ButtonHeight),
            });

            //Seach Table Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(SearchTableView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.Width, 1, 0),
                NSLayoutConstraint.Create(SearchTableView, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, scrollView, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(SearchTableView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, SearchButton, NSLayoutAttribute.Bottom, 1, 15),
            });

            scrollView.ContentSize = new CoreGraphics.CGSize(this.View.Bounds.Width, this.View.Bounds.Height);
            scrollView.Frame = this.View.Frame;
        }

        private void KeyBoardUpNotification(NSNotification notification)
        {
            if (!keyboardSet)
            {
                var val = (NSValue)notification.UserInfo.ValueForKey(UIKeyboard.FrameEndUserInfoKey);
                KeyboardBounds = val.CGRectValue;
                keyboardSet = true;
            }
            if (FieldSelected == null)
                return;
            
            // Bottom of the controller = initial position + height + offset      
            bottom = (float)(FieldSelected.Frame.Y + FieldSelected.Frame.Height + offset);

            //Added 180 for toolbar, navbar, constraints and padding height
            scroll_amount = (float)(KeyboardBounds.Height + (180) - (View.Frame.Size.Height - bottom)) ;

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
            UIView.SetAnimationDuration (0.2);

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
