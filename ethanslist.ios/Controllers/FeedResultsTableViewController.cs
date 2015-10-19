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
        CLFeedClient feedClient;

		public FeedResultsTableViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            feedClient = new CLFeedClient("apartments");

            this.Title = "Hello";

            tableView = new UITableView(this.View.Frame);

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

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return feedClient.postings.Count();
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = new UITableViewCell(CGRect.Empty);
            var item = feedClient.postings[indexPath.Row];

            cell.TextLabel.Text = item.Title;

            Console.WriteLine(item);

            return cell;
        }
	}
}
