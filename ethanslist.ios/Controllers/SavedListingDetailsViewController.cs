using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Listings.Models;
using SDWebImage;

namespace ethanslist.ios
{
	partial class SavedListingDetailsViewController : UIViewController
	{
		public SavedListingDetailsViewController (IntPtr handle) : base (handle)
		{
		}

        Listing listing;
        public Listing Listing {
            get {
                return listing;
            }
            set {
                listing = value;
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            PostingTitle.Text = listing.Title;
            PostingDescription.Text = listing.Description;
            Console.WriteLine(listing.ImageLink);
            Console.WriteLine(listing.Date);

            dateLabel.Text = "Listed: " + listing.Date.ToShortDateString() + " at " + listing.Date.ToShortTimeString();

            if (listing.ImageLink != "-1")
            {
                postingImageView.SetImage(
                    url: new NSUrl(listing.ImageLink),
                    placeholder: UIImage.FromBundle("placeholder.png")
                );
            }
            DoneButton.Clicked += OnDismiss;

            DeleteButton.Clicked += DeleteListing;
        }


        void OnDismiss(object sender, EventArgs e)
        {
            DismissViewController(true, null);
        }

        void DeleteListing(object sender, EventArgs e)
        {
            AppDelegate.listingRepository.DeleteListingAsync(listing);
            Console.WriteLine(AppDelegate.listingRepository.StatusMessage);

            DismissViewController(true, null);
        }


        static UIImage FromUrl (string uri)
        {
            using (var url = new NSUrl (uri))
            using (var data = NSData.FromUrl (url))
                return UIImage.LoadFromData (data);
        }
	}
}
