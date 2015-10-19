using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using CoreGraphics;
using System.Linq;

namespace ethanslist.ios
{
	partial class FeedResultsTableViewController : UITableViewController
	{
        UITableView tableView;
        string query;

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

            this.Title = "Craigslist Results";

            tableView = new UITableView(this.View.Frame);

            tableView.Source = new FeedResultTableSource(this, query);

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
        }
	}
}
