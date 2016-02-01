using System;
using EthansList.Shared;
using UIKit;
using EthansList.Models;
using System.Collections.Generic;
using SDWebImage;
using Foundation;

namespace ethanslist.ios
{
    public class SavedPostingsTableViewSource : UITableViewSource
    {
        UIViewController owner;
        public List<Posting> savedListings;

        public SavedPostingsTableViewSource(UIViewController owner, List<Posting> savedListings)
        {
            this.owner = owner;
            this.savedListings = savedListings;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return savedListings.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var cell = (FeedResultsCell)tableView.DequeueReusableCell(FeedResultsCell.Key);

            if (cell == null)
            {
                cell = FeedResultsCell.Create();
            }

            cell.BackgroundColor = ColorScheme.Clouds;

            Posting post = savedListings[indexPath.Row];

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

        public override async void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath)
        {
            switch (editingStyle) {
                case UITableViewCellEditingStyle.Delete:
                    await AppDelegate.databaseConnection.DeletePostingAsync(savedListings[indexPath.Row]);
                    savedListings.RemoveAt(indexPath.Row);
                    tableView.DeleteRows(new [] { indexPath }, UITableViewRowAnimation.Fade);
                    break;
                case UITableViewCellEditingStyle.None:
                    Console.WriteLine ("CommitEditingStyle:None called");
                    break;
            }
        }
        public override bool CanEditRow (UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            return true;
        }
        public override string TitleForDeleteConfirmation (UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            return "Delete";
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var storyboard = UIStoryboard.FromName("Main", null);
            var detailController = (PostingInfoViewController)storyboard.InstantiateViewController("PostingInfoViewController");
            detailController.Post = savedListings[indexPath.Row];

            detailController.ItemDeleted += async (sender, e) => {
                await owner.DismissViewControllerAsync(true);
                await AppDelegate.databaseConnection.DeletePostingAsync(savedListings[indexPath.Row]);
                savedListings.RemoveAt(indexPath.Row);
                tableView.DeleteRows(new [] { indexPath }, UITableViewRowAnimation.Fade);
                Console.WriteLine(AppDelegate.databaseConnection.StatusMessage);
            };

            owner.PresentViewController(detailController, true, null);
        }
    }
}

