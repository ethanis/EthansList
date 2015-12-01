using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using EthansList.Shared;
using SDWebImage;
using CoreGraphics;
using EthansList.Models;
using System.Xml;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ethanslist.ios
{
	partial class PostingDetailsViewController : UIViewController
	{
        UIBarButtonItem saveButton;
        UIBarButtonItem deleteButton;

		public PostingDetailsViewController (IntPtr handle) : base (handle)
		{
		}

        public PostingDetailsViewController()
        {
        }

        Posting post;
        public Posting Post {
            get {
                return post;
            }
            set {
                post = value;
            }
        }

        Boolean saved = false;
        public Boolean Saved {
            get { 
                return saved;
            }
            set { 
                saved = value;
            }
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

        public event EventHandler<EventArgs> ItemDeleted;

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            PostingTitle.Text = post.Title;
            PostingDescription.Text = post.Description;
//            Console.WriteLine(post.ImageLink);
//            Console.WriteLine(post.Date);
//            Console.WriteLine(post.Link);

            ListingImageDownloader imageHelper = new ListingImageDownloader(post.Link);
//            Console.WriteLine(imageHelper.GetAllImages());
            foreach (string image in imageHelper.GetAllImages())
            {
//                postingImageView.SetImage(
//                    url: new NSUrl(image.ToString()),
//                    placeholder: UIImage.FromBundle("placeholder.png")
//                );
                Console.WriteLine(image);
            }

            myNavBarItem.SetLeftBarButtonItem(null, true);

            if (!saved)
            {
                saveButton = new UIBarButtonItem(UIBarButtonSystemItem.Save, SaveListing);
                saveButton.Enabled = true;
                myNavBarItem.SetLeftBarButtonItem(saveButton, true);
            }
            else
            {
                deleteButton = new UIBarButtonItem(UIBarButtonSystemItem.Trash, DeleteListing);
                myNavBarItem.SetLeftBarButtonItem(deleteButton, true);
            }

            dateLabel.Text = "Listed: " + post.Date.ToShortDateString() + " at " + post.Date.ToShortTimeString();

            if (post.ImageLink != "-1")
            {
                postingImageView.SetImage(
                    url: new NSUrl(post.ImageLink),
                    placeholder: UIImage.FromBundle("placeholder.png")
                );
            }
            else
            {
                postingImageView.Image = UIImage.FromBundle("placeholder.png");
            }

            UITapGestureRecognizer singletap = new UITapGestureRecognizer(OnSingleTap) {
                NumberOfTapsRequired = 1
            };

            scrollView.AddGestureRecognizer(singletap); // detect when the scrollView is double-tapped

            DoneButton.Clicked += OnDismiss;
        }

        private void OnSingleTap (UIGestureRecognizer gesture) 
        {
            var storyboard = UIStoryboard.FromName("Main", null);
            postingImageViewController postingImageVC = (postingImageViewController)storyboard.InstantiateViewController("postingImageViewController");
            postingImageVC.Post = this.post;

            this.ShowViewController(postingImageVC, this);
        }


        void OnDismiss(object sender, EventArgs e)
        {
            DismissViewController(true, null);
        }

        async void SaveListing(object sender, EventArgs e)
        {
            await AppDelegate.databaseConnection.AddNewListingAsync(post.Title, post.Description, post.Link, post.ImageLink, post.Date);
            Console.WriteLine(AppDelegate.databaseConnection.StatusMessage);

            if (AppDelegate.databaseConnection.StatusCode == codes.ok)
            {
                UIAlertView alert = new UIAlertView();
                alert.Message = "Listing Saved!";
                alert.AddButton("OK");
                alert.Show();

                saveButton.Enabled = false;
            }
            else
            {
                UIAlertView alert = new UIAlertView();
                alert.Message = String.Format("Oops, something went wrong{0}Please try again...", Environment.NewLine);
                alert.AddButton("OK");
                alert.Show();

                saveButton.Enabled = true;
            }
        }

        void DeleteListing(object sender, EventArgs e)
        {
            if (this.ItemDeleted != null)
                this.ItemDeleted(this, new EventArgs());
        }
	}
}
