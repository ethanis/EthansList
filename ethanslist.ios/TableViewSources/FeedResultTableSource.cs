using System;
using UIKit;

namespace ethanslist.ios
{
    public class FeedResultTableSource : UITableViewSource
    {
        UIViewController owner;
        CLFeedClient feedClient;
        String CellId = "postCell";

        public FeedResultTableSource(UIViewController owner, CLFeedClient client)
        {
            this.owner = owner;
            this.feedClient = client;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            //TODO: Figure out weird cell lines and display

            return feedClient.postings.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(CellId);

            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, CellId);   
            }

            Posting post = feedClient.postings[indexPath.Row];

            cell.TextLabel.Text = post.Title;
            cell.DetailTextLabel.Text = post.Description;

            return cell;
        }

        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var storyboard = UIStoryboard.FromName("Main", null);
            var detailController = (PostingDetailsViewController)storyboard.InstantiateViewController("PostingDetailsViewController");

            Posting post = feedClient.postings[indexPath.Row];
            detailController.Post = post;

            owner.ShowDetailViewController(detailController, owner);
        }
    }
}

