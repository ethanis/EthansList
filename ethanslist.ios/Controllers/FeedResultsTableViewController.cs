using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using CoreGraphics;
using System.Linq;
using System.Threading;
using System.Diagnostics;

namespace ethanslist.ios
{
	partial class FeedResultsTableViewController : UITableViewController
	{
        UITableView tableView;
        FeedResultTableSource tableSource;
        string query;
        CLFeedClient feedClient;
        Stopwatch loadTimer;

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

            tableView = new UITableView(this.View.Frame);

            tableSource = new FeedResultTableSource(this, feedClient);
            tableView.Source = tableSource;

            this.Add(tableView);

            tableView.TranslatesAutoresizingMaskIntoConstraints = false;
            this.View.AddConstraint(NSLayoutConstraint.Create(tableView, NSLayoutAttribute.Top,
                NSLayoutRelation.Equal, this.View, NSLayoutAttribute.TopMargin, 1, 0));
            this.View.AddConstraint(NSLayoutConstraint.Create(tableView, NSLayoutAttribute.Left,
                NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Left, 1, 0));
            this.View.AddConstraint(NSLayoutConstraint.Create(tableView, NSLayoutAttribute.Width,
                NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, 1, 0));
            this.View.AddConstraint(NSLayoutConstraint.Create(tableView, NSLayoutAttribute.Height,
                NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Height, 1, 0));

            feedClient.loadingComplete += (object sender, EventArgs e) =>
            {
                    loadTimer.Stop();
                    tableView.ReloadData();
                    Console.WriteLine(loadTimer.Elapsed);
//                    feedClient.loadingComplete += Reload_Data;
            };

            feedClient.loadingProgressChanged += (object sender, EventArgs e) =>
            {
                    Console.WriteLine("Hi");
            };
        }

        void Reload_Data(object sender, EventArgs e)
        {
            loadTimer.Stop();
            tableView.ReloadData();
            Console.WriteLine(loadTimer.Elapsed);
        }
	}
}
