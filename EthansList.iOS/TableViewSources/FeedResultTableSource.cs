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
            var cell = (FeedResultsCell)tableView.DequeueReusableCell(FeedResultsCell.Key);
            if (cell == null)
            {
                cell = FeedResultsCell.Create();
            }

            cell.BackgroundColor = ColorScheme.Clouds;

            Posting post = feedClient.postings[indexPath.Row];

            cell.PostingTitle.AttributedText = new NSAttributedString(post.PostTitle, Constants.HeaderAttributes);
            cell.PostingDescription.AttributedText = new NSAttributedString(post.Description, Constants.LabelAttributes);

            if (post.ImageLink != "-1")
            {
                cell.PostingImage.SetImage(
                    url: new NSUrl(post.ImageLink),
                    placeholder: UIImage.FromBundle("placeholder.png")
                );
            }
            else
            {
                cell.PostingImage.Image = UIImage.FromBundle("placeholder.png");
            }

            return cell;
        }

        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var storyboard = UIStoryboard.FromName("Main", null);
            var detailController = (PostingInfoViewController)storyboard.InstantiateViewController("PostingInfoViewController");

            Posting post = feedClient.postings[indexPath.Row];
            detailController.Post = post;

            owner.PresentModalViewController(detailController, true);
        }
    }
}

