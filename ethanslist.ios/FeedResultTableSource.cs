using System;
using UIKit;

namespace ethanslist.ios
{
    public class FeedResultTableSource : UITableViewSource
    {
        CLFeedClient feedClient;
        String CellId = "postCell";

        public FeedResultTableSource(String query)
        {
            feedClient = new CLFeedClient(query);
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return feedClient.postings.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(CellId);

            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, "postCell");   
            }

            Posting post = feedClient.postings[indexPath.Row];

            cell.TextLabel.Text = post.Title;
            cell.DetailTextLabel.Text = post.Description;

            return cell;
        }
    }
}

