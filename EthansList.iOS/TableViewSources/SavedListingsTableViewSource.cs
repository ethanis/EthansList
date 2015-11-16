using System;
using EthansList.Shared;
using UIKit;
using Listings.Models;
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

            cell.TextLabel.Text = listing.Title;
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

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            Console.WriteLine("Title: " + savedListings[indexPath.Row].Title);
            Console.WriteLine("Description: " + savedListings[indexPath.Row].Description);
            Console.WriteLine("ImageLink: " + savedListings[indexPath.Row].ImageLink);
            Console.WriteLine("Date: " + savedListings[indexPath.Row].Date);
        }
    }
}

