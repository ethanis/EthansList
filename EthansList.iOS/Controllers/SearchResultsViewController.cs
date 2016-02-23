using System;
using UIKit;
using CoreGraphics;
using EthansList.Shared;
using iAd;

namespace ethanslist.ios
{
    public class SearchResultsViewController : UIViewController
    {
        FeedResultTableSource tableSource;
        private CLFeedClient feedClient;
        protected LoadingOverlay _loadingOverlay = null;
        UITableView feedResultTable;
        UIRefreshControl refreshControl;
        ADBannerView ads;
        NSLayoutConstraint searchTableBottom;

        public SearchResultsViewController ()
        {
        }

        public override void LoadView()
        {
            base.LoadView();

            feedResultTable = new UITableView ();
            View.AddSubview (feedResultTable);
            ads = new ADBannerView ();
            View.AddSubview (ads);

            this.View.Layer.BackgroundColor = ColorScheme.Clouds.CGColor;
            this.feedResultTable.BackgroundColor = ColorScheme.Clouds;

            AddLayoutConstraints();
        }

        public String Query { get; set;}
        public int MaxListings 
        { 
            get { return maxListings; } 
            set { maxListings = value; }
        }
        protected int maxListings = 25;

        public int? WeeksOld
        { 
            get { return weeksOld; } 
            set { weeksOld = value; }
        }
        protected int? weeksOld = null;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Console.WriteLine(Query);

            this.Title = "Search Results";

            var bounds = UIScreen.MainScreen.Bounds; // portrait bounds
            if (UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeLeft || UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeRight) {
                bounds.Size = new CGSize(bounds.Size.Height, bounds.Size.Width);
            }
            // show the loading overlay on the UI thread using the correct orientation sizing
            this._loadingOverlay = new LoadingOverlay (bounds);
            this.View.Add ( this._loadingOverlay );

            feedClient = new CLFeedClient(Query, MaxListings, WeeksOld);
            var result = feedClient.GetAllPostingsAsync();

            if (!result)
            {
                this._loadingOverlay.Hide();
                UIAlertView alert = new UIAlertView();
                alert.Message = String.Format("No network connection.{0}Please check your settings", Environment.NewLine);
                alert.AddButton("OK");
                alert.Clicked += (s, ev) => {
                    this.InvokeOnMainThread(() => this.NavigationController.PopViewController(true));
                };
                this.InvokeOnMainThread(() => alert.Show());   
            }

            tableSource = new FeedResultTableSource(this, feedClient);

            feedResultTable.Source = tableSource;
            feedResultTable.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            refreshControl = new UIRefreshControl();
            feedResultTable.AddSubview (refreshControl);
            refreshControl.ValueChanged += (object sender, EventArgs e) => {
                feedClient.GetAllPostingsAsync();
            };

            feedClient.asyncLoadingComplete += feedClient_LoadingComplete;
            feedClient.asyncLoadingPartlyComplete += feedClient_LoadingComplete;
            feedClient.emptyPostingComplete += (object sender, EventArgs e) => 
            {
                if (!this._loadingOverlay.AlreadyHidden)
                    this._loadingOverlay.Hide();

                refreshControl.EndRefreshing();
                UIAlertView alert = new UIAlertView();
                alert.Message = String.Format("No listings found.{0}Try another search", Environment.NewLine);
                alert.AddButton("OK");
                alert.Clicked += (s, ev) => {
                    this.InvokeOnMainThread(() => this.NavigationController.PopViewController(true));
                };
                this.InvokeOnMainThread(() => alert.Show());
            };

            ads.AdLoaded += (object sender, EventArgs e) => { 
                AddAdBanner (true);
            };
        }

        void AddAdBanner (bool show)
        {
            if (searchTableBottom != null)
                View.RemoveConstraint (searchTableBottom);

            if (show) 
            {
                ads.Hidden = false;

                //Ads Constraints
                this.View.AddConstraints (new NSLayoutConstraint [] {
                    NSLayoutConstraint.Create(ads, NSLayoutAttribute.Width, NSLayoutRelation.Equal, View, NSLayoutAttribute.Width, 1, 0),
                      NSLayoutConstraint.Create(ads, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, View, NSLayoutAttribute.CenterX, 1, 0),
                      NSLayoutConstraint.Create(ads, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, View, NSLayoutAttribute.Bottom, 1, 0),
                      //NSLayoutConstraint.Create(ads, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, 50),
                });

                searchTableBottom = NSLayoutConstraint.Create (feedResultTable, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, ads, NSLayoutAttribute.Bottom, 1, 0);
            }
            else
            {
                ads.Hidden = true;
                searchTableBottom = NSLayoutConstraint.Create (feedResultTable, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, View, NSLayoutAttribute.Bottom, 1, 0);
            }
            View.AddConstraint (searchTableBottom);
            this.View.LayoutIfNeeded ();
        }

        void feedClient_LoadingComplete(object sender, EventArgs e)
        {
            this.InvokeOnMainThread(() => {
                if (!this._loadingOverlay.AlreadyHidden)
                {
                    this._loadingOverlay.Hide();
                    this._loadingOverlay.AlreadyHidden = true;
                }
                feedResultTable.ReloadData();
                refreshControl.EndRefreshing();
                Console.WriteLine (feedClient.postings.Count);
            });

            Console.WriteLine(feedResultTable.NumberOfRowsInSection(0));
        }

        void AddLayoutConstraints()
        {
            feedResultTable.TranslatesAutoresizingMaskIntoConstraints = false;
            ads.TranslatesAutoresizingMaskIntoConstraints = false;

            this.View.AddConstraint(NSLayoutConstraint.Create(feedResultTable, NSLayoutAttribute.Top,
                                                          NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Top, 1, 0));
            this.View.AddConstraint(NSLayoutConstraint.Create(feedResultTable, NSLayoutAttribute.Left,
                                                          NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Left, 1, 0));
            this.View.AddConstraint(NSLayoutConstraint.Create(feedResultTable, NSLayoutAttribute.Width,
                                                          NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, 1, 0));
            this.View.AddConstraint(NSLayoutConstraint.Create(feedResultTable, NSLayoutAttribute.Height,
                                                          NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Height, 1, 0));

            AddAdBanner (false);
        }
    }
}

