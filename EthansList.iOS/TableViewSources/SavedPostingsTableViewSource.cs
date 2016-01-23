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
        private const string cellId = "listingCell";

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
            UITableViewCell cell = tableView.DequeueReusableCell(cellId);

            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, cellId);
            }

            var listing = savedListings[indexPath.Row];

            cell.TextLabel.Text = listing.PostTitle;
            cell.DetailTextLabel.Text = listing.Description;
            cell.BackgroundColor = ColorScheme.Clouds;

            if (listing.ImageLink != "-1")
            {
                cell.ImageView.SetImage(
                    url: new NSUrl(listing.ImageLink),
                    placeholder: UIImage.FromBundle("placeholder.png")
                );
            }
            else
            {
                cell.ImageView.Image = UIImage.FromBundle("placeholder.png");
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
//            var detailController = (PostingDetailsViewController)storyboard.InstantiateViewController("PostingDetailsViewController");
//            detailController.Post = savedListings[indexPath.Row];
//
//            detailController.ItemDeleted += async (sender, e) => {
//                await owner.DismissViewControllerAsync(true);
//                await AppDelegate.databaseConnection.DeletePostingAsync(savedListings[indexPath.Row]);
//                savedListings.RemoveAt(indexPath.Row);
//                tableView.DeleteRows(new [] { indexPath }, UITableViewRowAnimation.Fade);
//                Console.WriteLine(AppDelegate.databaseConnection.StatusMessage);
//            };
//            owner.PresentViewController(detailController, true, null);

            var detailController = (PostingInfoViewController)storyboard.InstantiateViewController("PostingInfoViewController");
            owner.PresentViewController(detailController, true, null);
        }
    }
}

