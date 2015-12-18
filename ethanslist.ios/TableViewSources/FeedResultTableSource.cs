using System;
using UIKit;
using EthansList.Shared;
using Foundation;
using SDWebImage;
using EthansList.Models;

namespace ethanslist.ios
{
    public class FeedResultTableSource : UITableViewSource
    {
        UIViewController owner;
        CLFeedClient feedClient;
        private readonly String CellId = "postCell";

        public FeedResultTableSource(UIViewController owner, CLFeedClient client)
        {
            this.owner = owner;
            this.feedClient = client;
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
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, CellId);   
            }

            cell.BackgroundColor = ColorScheme.Clouds;

            Posting post = feedClient.postings[indexPath.Row];

            cell.TextLabel.Text = post.PostTitle;
            cell.DetailTextLabel.Text = post.Description;
            if (post.ImageLink != "-1")
            {
                cell.ImageView.SetImage(
                    url: new NSUrl(post.ImageLink),
                    placeholder: UIImage.FromBundle("placeholder.png")
                );
            }
            else
            {
                cell.ImageView.Image = UIImage.FromBundle("placeholder.png");
            }

            return cell;
        }

        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var storyboard = UIStoryboard.FromName("Main", null);
            var detailController = (PostingDetailsViewController)storyboard.InstantiateViewController("PostingDetailsViewController");

            Posting post = feedClient.postings[indexPath.Row];
            detailController.Post = post;
            detailController.Saved = false;

            owner.PresentModalViewController(detailController, true);
        }
    }
}

