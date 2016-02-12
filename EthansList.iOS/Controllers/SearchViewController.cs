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
using Newtonsoft.Json;

namespace ethanslist.ios
{
    public class SearchViewController : UIViewController
    {
        UIBarButtonItem saveButton;
        SearchOptionsTableSource tableSource;
        UIView holderView;
        UIButton SearchButton;
        UITableView SearchTableView;
        UIScrollView scrollView;
        UILabel SearchCityLabel;

        private float scroll_amount = 0.0f;    // amount to scroll 
        private float bottom = 0.0f;           // bottom point
        private float offset = 40.0f;          // extra offset
        private bool moveViewUp = false;           // which direction are we moving
        private bool keyboardSet = false;

        #region GeneratedFromSearchOptions
        public int MaxListings 
        { 
            get { return maxListings; } 
            set { maxListings = value; }
        }
        private int maxListings = 25;
        public int? WeeksOld { get; set; }
        public KeyValuePair<object,object> SubCategory { get; set; }
        public Dictionary<string, string> SearchItems { get; set;}
        public Dictionary<object, KeyValuePair<object, object>> Conditions { get; set; }
        #endregion

        public Location Location { get; set; }
        public KeyValuePair<string, string> Category {get;set;}
        public UIView FieldSelected { get; set; }
        public CGRect KeyboardBounds { get; set; }

        public SearchViewController()
        {
        }


        public override void LoadView()
        {
            base.LoadView();

            this.View.Layer.BackgroundColor = ColorScheme.Clouds.CGColor;


            SearchButton = new UIButton();
            holderView = new UIView(this.View.Frame);
            SearchTableView = new UITableView(new CGRect(), UITableViewStyle.Grouped);
            scrollView = new UIScrollView(this.View.Frame);
            SearchCityLabel = new UILabel(){TextAlignment = UITextAlignment.Center};

            SearchButton.Layer.BackgroundColor = ColorScheme.MidnightBlue.CGColor;
            SearchButton.SetTitleColor(ColorScheme.Clouds, UIControlState.Normal);
            SearchButton.Layer.CornerRadius = 10;
            SearchButton.ClipsToBounds = true;
            SearchButton.SetAttributedTitle(new NSAttributedString("Search", Constants.ButtonAttributes), UIControlState.Normal);
            SearchTableView.Layer.BackgroundColor = ColorScheme.Clouds.CGColor;

            holderView.AddSubviews(new UIView[]{SearchButton, SearchCityLabel, SearchTableView});
            scrollView.AddSubview(holderView);
            this.View.AddSubview(scrollView);

            AddLayoutConstraints();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            tableSource = new SearchOptionsTableSource(GetTableSetup(), this);
            SearchTableView.Source = tableSource;

            this.Title = "Options";

            SearchItems = new Dictionary<string, string>();
            Conditions = new Dictionary<object,  KeyValuePair<object, object>>();

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
                async (sender, e) => 
                {
                    SearchObject forSerial = new SearchObject();
                    forSerial.SearchLocation = Location;
                    forSerial.Category = SubCategory.Value != null ? new KeyValuePair<object,object>(SubCategory.Value, SubCategory.Key) : new KeyValuePair<object,object>(Category.Key, Category.Value);
                    forSerial.SearchItems = this.SearchItems;
                    forSerial.Conditions = this.Conditions;
                    forSerial.MaxListings = this.MaxListings;
                    forSerial.PostedDate = this.WeeksOld;

                    string serialized = JsonConvert.SerializeObject(forSerial);
                    await AppDelegate.databaseConnection.AddNewSearchAsync(Location.Url, serialized);

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
                var storyboard = UIStoryboard.FromName("Main", null);
                var feedViewController = (FeedResultsTableViewController)storyboard.InstantiateViewController("FeedResultsTableViewController");

                SearchObject forSerial = new SearchObject();
                forSerial.SearchLocation = Location;
                forSerial.Category = SubCategory.Value != null ? new KeyValuePair<object,object>(SubCategory.Value, SubCategory.Key) : new KeyValuePair<object,object>(Category.Key, Category.Value);
                forSerial.SearchItems = this.SearchItems;
                forSerial.Conditions = this.Conditions;

                var query = queryHelper.Generate(forSerial);

                feedViewController.Query = query;
                feedViewController.MaxListings = MaxListings;
                feedViewController.WeeksOld = WeeksOld;

                this.ShowViewController(feedViewController, this);
            };
        }


        void AddLayoutConstraints()
        {
            holderView.TranslatesAutoresizingMaskIntoConstraints = false;
            SearchCityLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            SearchButton.TranslatesAutoresizingMaskIntoConstraints = false;
            SearchTableView.TranslatesAutoresizingMaskIntoConstraints = false;

            SearchTableView.ScrollEnabled = true;
            SearchTableView.Bounces = false;
            SearchCityLabel.AttributedText = new NSAttributedString(String.Format("Search {0} for:", Location.SiteName), Constants.LabelAttributes);

            //Scrollview Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(holderView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, 1, 0),
                NSLayoutConstraint.Create(holderView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Height, 1, 0),
                NSLayoutConstraint.Create(holderView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Top, 1, 20),
                NSLayoutConstraint.Create(holderView, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Left, 1, 0),
            });

//            Seach CL Label Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(SearchCityLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, holderView, NSLayoutAttribute.Width, 0.9f, 0),
                NSLayoutConstraint.Create(SearchCityLabel, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, holderView, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(SearchCityLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, holderView, NSLayoutAttribute.Top, 1, 59),
            });

            //Seach Button Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(SearchButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, holderView, NSLayoutAttribute.Width, .90f, 0),
                NSLayoutConstraint.Create(SearchButton, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, holderView, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(SearchButton, NSLayoutAttribute.Top, NSLayoutRelation.Equal, SearchCityLabel, NSLayoutAttribute.Bottom, 1, 15),
                NSLayoutConstraint.Create(SearchButton, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, Constants.ButtonHeight),
            });

            //Seach Table Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(SearchTableView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, holderView, NSLayoutAttribute.Width, 1, 0),
                NSLayoutConstraint.Create(SearchTableView, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, holderView, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(SearchTableView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, SearchButton, NSLayoutAttribute.Bottom, 1, 15),
                NSLayoutConstraint.Create(SearchTableView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, holderView, NSLayoutAttribute.Bottom, 1, 0),
            });
           
            this.View.LayoutIfNeeded();
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
            //Added 180 for toolbar, navbar, constraints and padding height, the content offset for amount of scrolled down
            scroll_amount = (float)(KeyboardBounds.Height + 180 - SearchTableView.ContentOffset.Y - (View.Frame.Size.Height - bottom));

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

        private List<TableItemGroup> GetTableSetup()
        {
            List<TableItemGroup> tableItems = new List<TableItemGroup>();

            TableItemGroup searchterms = new TableItemGroup()
                { Name = "Search Terms"};
            TableItemGroup options = new TableItemGroup()
                { Name = "Options" };

            searchterms.Items.Add(new TableItem() { 
                Heading = "Search Terms",
                CellType = "SearchTermsCell",
            });
            if (Categories.Autos.Contains(Category.Key))
            {
                searchterms.Items.Add(new TableItem()
                    {
                        Heading = "Make/Model",
                        SubHeading = "make / model",
                        CellType = "MakeModelCell"
                    });
                searchterms.Items.Add(new TableItem()
                    {
                        Heading = "Year",
                        CellType = "MinMaxCell"
                    });
            }

            if (Categories.Groups.Find(x => x.Name == "Housing").Items.Contains(Category) || Categories.Groups.Find(x => x.Name == "For Sale").Items.Contains(Category))
            {
                searchterms.Items.Add(new TableItem()
                    {
                        Heading = "Price",
                        CellType = "PriceInputCell",
                    });
            }

            if (Categories.Groups.Find(x => x.Name == "Housing").Items.Contains(Category))
            {
                searchterms.Items.Add(new TableItem(){
                    Heading = "Sq Feet",
                    CellType = "MinMaxCell"
                });
            }

            if (Categories.Autos.Contains(Category.Key))
            {
                searchterms.Items.Add(new TableItem(){
                    Heading = "Odometer",
                    CellType = "MinMaxCell"
                });

            }

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

            if (Categories.Groups.Find(x => x.Name == "For Sale").Items.Contains(Category) || Categories.Autos.Contains(Category.Key))
            {
                options.Items.Add(new TableItem()
                    {
                        Heading = "Condition",
                        CellType = "ComboTableCell",
                        SubHeading = "condition",
                        PickerOptions = new List<PickerOptions>()
                            {
                                new PickerOptions()
                                {PickerWheelOptions = new List<KeyValuePair<object, object>>()
                                    {
                                        new KeyValuePair<object, object>("New", 10),
                                        new KeyValuePair<object, object>("Like New", 20),
                                        new KeyValuePair<object, object>("Excellent", 30),
                                        new KeyValuePair<object, object>("Good", 40),
                                        new KeyValuePair<object, object>("Fair", 50),
                                        new KeyValuePair<object, object>("Salvage", 60),
                                    }
                                }
                            },
                    });
            }

            if (Categories.Groups.Find(x => x.Name == "Jobs").Items.Contains(Category))
            {
                options.Items.Add(new TableItem()
                    {
                        Heading = "Job Type",
                        CellType = "ComboTableCell",
                        SubHeading = "employment_type",
                        PickerOptions = new List<PickerOptions>()
                            {
                                new PickerOptions()
                                {PickerWheelOptions = new List<KeyValuePair<object, object>>()
                                    {
                                        new KeyValuePair<object, object>("full time", 1),
                                        new KeyValuePair<object, object>("part time", 2),
                                        new KeyValuePair<object, object>("contract", 3),
                                        new KeyValuePair<object, object>("employee's choice", 4),
                                    }
                                }
                            },
                    });
            }

            if (Categories.Groups.Find(x => x.Name == "Gigs").Items.Contains(Category))
            {
                options.Items.Add(new TableItem()
                    {
                        Heading = "Paid",
                        CellType = "ComboTableCell",
                        SubHeading = "is_paid",
                        PickerOptions = new List<PickerOptions>()
                            {
                                new PickerOptions()
                                {PickerWheelOptions = new List<KeyValuePair<object, object>>()
                                    {
                                        new KeyValuePair<object, object>("All", null),
                                        new KeyValuePair<object, object>("Paid", "yes"),
                                        new KeyValuePair<object, object>("UnPaid", "no"),
                                    }
                                }
                            },
                    });
            }

            if (Categories.Autos.Contains(Category.Key))
            {
                options.Items.Add(new TableItem()
                    {
                        Heading = "Cylinders",
                        CellType = "ComboTableCell",
                        SubHeading = "auto_cylinders",
                        PickerOptions = new List<PickerOptions>()
                            {
                                new PickerOptions()
                                {PickerWheelOptions = new List<KeyValuePair<object, object>>()
                                    {
                                        new KeyValuePair<object, object>("3 cylinders", 1),
                                        new KeyValuePair<object, object>("4 cylinders", 2),
                                        new KeyValuePair<object, object>("5 cylinders", 3),
                                        new KeyValuePair<object, object>("6 cylinders", 4),
                                        new KeyValuePair<object, object>("8 cylinders", 5),
                                        new KeyValuePair<object, object>("10 cylinders", 6),
                                        new KeyValuePair<object, object>("12 cylinders", 7),
                                        new KeyValuePair<object, object>("other", 8),
                                    }
                                }
                            },
                    });
                options.Items.Add(new TableItem()
                    {
                        Heading = "Drive",
                        CellType = "ComboTableCell",
                        SubHeading = "auto_drivetrain",
                        PickerOptions = new List<PickerOptions>()
                            {
                                new PickerOptions()
                                {PickerWheelOptions = new List<KeyValuePair<object, object>>()
                                    {
                                        new KeyValuePair<object, object>("fwd", 1),
                                        new KeyValuePair<object, object>("rwd", 2),
                                        new KeyValuePair<object, object>("4wd", 3),
                                    }
                                }
                            },
                    });
                options.Items.Add(new TableItem()
                    {
                        Heading = "Fuel",
                        CellType = "ComboTableCell",
                        SubHeading = "auto_fuel_type",
                        PickerOptions = new List<PickerOptions>()
                            {
                                new PickerOptions()
                                {PickerWheelOptions = new List<KeyValuePair<object, object>>()
                                    {
                                        new KeyValuePair<object, object>("gas", 1),
                                        new KeyValuePair<object, object>("diesel", 2),
                                        new KeyValuePair<object, object>("hybrid", 3),
                                        new KeyValuePair<object, object>("electric", 4),
                                        new KeyValuePair<object, object>("other", 6),
                                    }
                                }
                            },
                    });
                options.Items.Add(new TableItem()
                    {
                        Heading = "Paint Color",
                        CellType = "ComboTableCell",
                        SubHeading = "auto_paint",
                        PickerOptions = new List<PickerOptions>()
                            {
                                new PickerOptions()
                                {PickerWheelOptions = new List<KeyValuePair<object, object>>()
                                    {
                                        new KeyValuePair<object, object>("black", 1),
                                        new KeyValuePair<object, object>("blue", 2),
                                        new KeyValuePair<object, object>("brown", 20),
                                        new KeyValuePair<object, object>("green", 3),
                                        new KeyValuePair<object, object>("grey", 4),
                                        new KeyValuePair<object, object>("orange", 5),
                                        new KeyValuePair<object, object>("purple", 6),
                                        new KeyValuePair<object, object>("red", 7),
                                        new KeyValuePair<object, object>("silver", 8),
                                        new KeyValuePair<object, object>("white", 9),
                                        new KeyValuePair<object, object>("yellow", 10),
                                        new KeyValuePair<object, object>("custom", 11),
                                    }
                                }
                            },
                    });
                options.Items.Add(new TableItem()
                    {
                        Heading = "Title Status",
                        CellType = "ComboTableCell",
                        SubHeading = "auto_title_status",
                        PickerOptions = new List<PickerOptions>()
                            {
                                new PickerOptions()
                                {PickerWheelOptions = new List<KeyValuePair<object, object>>()
                                    {
                                        new KeyValuePair<object, object>("clean", 1),
                                        new KeyValuePair<object, object>("salvage", 2),
                                        new KeyValuePair<object, object>("rebuilt", 3),
                                        new KeyValuePair<object, object>("parts only", 4),
                                        new KeyValuePair<object, object>("lien", 5),
                                        new KeyValuePair<object, object>("missing", 6),
                                    }
                                }
                            },
                    });
                options.Items.Add(new TableItem()
                    {
                        Heading = "Transmission",
                        CellType = "ComboTableCell",
                        SubHeading = "auto_transmission",
                        PickerOptions = new List<PickerOptions>()
                            {
                                new PickerOptions()
                                {PickerWheelOptions = new List<KeyValuePair<object, object>>()
                                    {
                                        new KeyValuePair<object, object>("manual", 1),
                                        new KeyValuePair<object, object>("automatic", 2),
                                        new KeyValuePair<object, object>("other", 3),
                                    }
                                }
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
            }
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
    }
}

