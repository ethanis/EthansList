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
        List<Listing> savedListings;
        private const string cellId = "listingCell";
        public event EventHandler<EventArgs> ItemDeleted;

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

        public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewRowAction deleteButton = UITableViewRowAction.Create (
                UITableViewRowActionStyle.Destructive,
                "Delete",
                delegate {
                AppDelegate.databaseConnection.DeleteListingAsync(savedListings[indexPath.Row]);
                Console.WriteLine(AppDelegate.databaseConnection.StatusMessage);
                if (this.ItemDeleted != null)
                    this.ItemDeleted(this, new EventArgs());
            });
            return new UITableViewRowAction[] { deleteButton };
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            Console.WriteLine("Title: " + savedListings[indexPath.Row].PostTitle);
            Console.WriteLine("Description: " + savedListings[indexPath.Row].Description);
            Console.WriteLine("ImageLink: " + savedListings[indexPath.Row].ImageLink);
            Console.WriteLine("Date: " + savedListings[indexPath.Row].Date);

            var storyboard = UIStoryboard.FromName("Main", null);
            var detailController = (SavedListingDetailsViewController)storyboard.InstantiateViewController("SavedListingDetailsViewController");
            detailController.Listing = savedListings[indexPath.Row];
            Console.WriteLine("Hello");
            owner.PresentModalViewController(detailController, true);
        }
    }
}

