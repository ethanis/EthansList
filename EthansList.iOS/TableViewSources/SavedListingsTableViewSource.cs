using System;
using EthansList.Shared;
using UIKit;
using EthansList.Models;
using System.Collections.Generic;
using SDWebImage;
using Foundation;

namespace ethanslist.ios
{
    public class SavedListingsTableViewSource : UITableViewSource
    {
        UIViewController owner;
        public List<Listing> savedListings;
        private const string cellId = "listingCell";

        public SavedListingsTableViewSource(UIViewController owner, List<Listing> savedListings)
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

        public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath)
        {
            switch (editingStyle) {
                case UITableViewCellEditingStyle.Delete:
                    AppDelegate.databaseConnection.DeleteListingAsync(savedListings[indexPath.Row]);
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
            var detailController = (PostingDetailsViewController)storyboard.InstantiateViewController("PostingDetailsViewController");
            detailController.Listing = savedListings[indexPath.Row];
            var listing = savedListings[indexPath.Row];
            detailController.Post = new Posting()
            { Title = listing.PostTitle, Description = listing.Description, 
                Link = listing.Link, ImageLink = listing.ImageLink, Date = listing.Date
            };

            detailController.Saved = true;
            detailController.ItemDeleted += async (sender, e) => {
                await owner.DismissViewControllerAsync(true);
                await AppDelegate.databaseConnection.DeleteListingAsync(savedListings[indexPath.Row]);
                savedListings.RemoveAt(indexPath.Row);
                tableView.DeleteRows(new [] { indexPath }, UITableViewRowAnimation.Fade);
                Console.WriteLine(AppDelegate.databaseConnection.StatusMessage);
            };
            owner.PresentViewController(detailController, true, null);
        }
    }
}

