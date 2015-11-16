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
        string query;
        CLFeedClient feedClient;
        Stopwatch loadTimer;
        int percentComplete = 0;

		public FeedResultsTableViewController (IntPtr handle) : base (handle)
		{
		}

        public String Query {
            get {
                return query;
            }
            set {
                query = value;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            loadTimer = new Stopwatch();
            loadTimer.Start();

            this.Title = "Craigslist Results";
            feedClient = new CLFeedClient(query);

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

            feedClient.loadingComplete += (object sender, EventArgs e) =>
            {
                    loadTimer.Stop();
                    TableView.ReloadData();
                    Console.WriteLine(loadTimer.Elapsed);
//                    feedClient.loadingComplete += Reload_Data;
            };

            feedClient.loadingProgressChanged += (object sender, EventArgs e) =>
            {
                    percentComplete += 10;
                    Console.WriteLine(percentComplete + "% Complete");
            };
        }

        void Reload_Data(object sender, EventArgs e)
        {
            loadTimer.Stop();
            TableView.ReloadData();
            Console.WriteLine(loadTimer.Elapsed);
        }
	}
}
