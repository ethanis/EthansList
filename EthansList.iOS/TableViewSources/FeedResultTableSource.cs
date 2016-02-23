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

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 80f;
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var cell = (FeedResultsCell)tableView.DequeueReusableCell(FeedResultsCell.Key);
            if (cell == null)
            {
                cell = new FeedResultsCell();
            }

            cell.BackgroundColor = ColorScheme.Clouds;

            Posting post = feedClient.postings[indexPath.Row];

            cell.PostingTitle.AttributedText = new NSAttributedString(post.PostTitle, Constants.HeaderAttributes);
            cell.PostingDescription.AttributedText = new NSAttributedString(post.Description, Constants.FeedDescriptionAttributes);

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

            detailController.ItemDeleted += async delegate
            {
                    await AppDelegate.databaseConnection.DeletePostingAsync(feedClient.postings[indexPath.Row].Link);
                    Console.WriteLine(AppDelegate.databaseConnection.StatusMessage);
            };

            owner.PresentModalViewController(detailController, true);
        }

        public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath)
        {
            var item = feedClient.postings [indexPath.Row];
            var saveListing = UITableViewRowAction.Create(UITableViewRowActionStyle.Normal, "Save", async delegate {
                await AppDelegate.databaseConnection.AddNewListingAsync (item.PostTitle, item.Description, item.Link, item.ImageLink, item.Date);
                //TODO: Make pattern image for background

                tableView.EndEditing (true);
                tableView.SetEditing (false, true);
                Console.WriteLine (AppDelegate.databaseConnection.StatusMessage);
            });
            saveListing.BackgroundColor = ColorScheme.Carrot;

            return new UITableViewRowAction[]{ saveListing };
        }
    }
}

