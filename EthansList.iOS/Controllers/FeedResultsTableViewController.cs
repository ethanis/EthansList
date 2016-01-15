using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using CoreGraphics;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using EthansList.Shared;

namespace ethanslist.ios
{
	partial class FeedResultsTableViewController : UITableViewController
	{
        FeedResultTableSource tableSource;
        private CLFeedClient feedClient;
        Stopwatch loadTimer;
        int percentComplete = 0;
        protected LoadingOverlay _loadingOverlay = null;

		public FeedResultsTableViewController (IntPtr handle) : base (handle)
		{
		}

        public override void LoadView()
        {
            base.LoadView();

            this.View.Layer.BackgroundColor = ColorScheme.Clouds.CGColor;
            this.TableView.BackgroundColor = ColorScheme.Clouds;
        }

        public String Query { get; set;}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Console.WriteLine(Query);

//            loadTimer = new Stopwatch();
//            loadTimer.Start();

            this.Title = "Craigslist Results";
            feedClient = null;
            feedClient = new CLFeedClient(Query);
            feedClient.GetPostings();

            var bounds = UIScreen.MainScreen.Bounds; // portrait bounds
            if (UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeLeft || UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeRight) {
                bounds.Size = new CGSize(bounds.Size.Height, bounds.Size.Width);
            }
            // show the loading overlay on the UI thread using the correct orientation sizing
            this._loadingOverlay = new LoadingOverlay (bounds);
            this.View.Add ( this._loadingOverlay );

            tableSource = new FeedResultTableSource(this, feedClient);

            TableView.Source = tableSource;

            this.View.AddConstraint(NSLayoutConstraint.Create(TableView, NSLayoutAttribute.Top,
                NSLayoutRelation.Equal, this.View, NSLayoutAttribute.TopMargin, 1, 0));
            this.View.AddConstraint(NSLayoutConstraint.Create(TableView, NSLayoutAttribute.Left,
                NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Left, 1, 0));
            this.View.AddConstraint(NSLayoutConstraint.Create(TableView, NSLayoutAttribute.Width,
                NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, 1, 0));
            this.View.AddConstraint(NSLayoutConstraint.Create(TableView, NSLayoutAttribute.Height,
                NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Height, 1, 0));

            RefreshControl = new UIRefreshControl();

            RefreshControl.ValueChanged += (object sender, EventArgs e) => {
                feedClient.GetPostings();
            };

            feedClient.loadingComplete += feedClient_LoadingComplete;

            feedClient.emptyPostingComplete += (object sender, EventArgs e) => 
            {
                    feedClient = null;
                    this._loadingOverlay.Hide();
                    RefreshControl.EndRefreshing();
                    UIAlertView alert = new UIAlertView();
                    alert.Message = String.Format("No listings found.{0}Try another search", Environment.NewLine);
                    alert.AddButton("OK");
                    alert.Clicked += (s, ev) => {this.NavigationController.PopViewController(true);};
                    this.InvokeOnMainThread(() => alert.Show());
            };

//            feedClient.loadingProgressChanged += (object sender, EventArgs e) =>
//            {
//                    percentComplete += 10;
//                    Console.WriteLine(percentComplete + "% Complete");
//            };
        }

        void feedClient_LoadingComplete(object sender, EventArgs e)
        {
            this.InvokeOnMainThread(() => {
                this._loadingOverlay.Hide();
//                loadTimer.Stop();
                TableView.ReloadData();
//                Console.WriteLine(loadTimer.Elapsed);
//                percentComplete = 0;
                RefreshControl.EndRefreshing();
            });
        }
	}
}
