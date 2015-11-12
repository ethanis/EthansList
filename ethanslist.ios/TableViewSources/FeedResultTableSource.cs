using System;
using UIKit;
using EthansList.Shared;
using Foundation;
using SDWebImage;

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
            if (post.ImageLink != "-1")
            {
//                cell.ImageView.Image = FromUrl(post.ImageLink);
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

            owner.ShowDetailViewController(detailController, owner);
        }

        static UIImage FromUrl (string uri)
        {
            using (var url = new NSUrl (uri))
            using (var data = NSData.FromUrl (url))
                return UIImage.LoadFromData (data);
        }
    }
}

