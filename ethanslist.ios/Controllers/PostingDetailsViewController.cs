using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using EthansList.Shared;
using SDWebImage;
using CoreGraphics;
using EthansList.Models;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ethanslist.ios
{
	partial class PostingDetailsViewController : UIViewController
	{
        UIBarButtonItem saveButton;
        UIBarButtonItem deleteButton;
        ImageCollectionViewSource collectionSource;
        ListingImageDownloader imageHelper;

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

        string image;
        public string Image
        {
            get{ 
                return image;
            }
            set {
                postingImageView.SetImage(
                    url: new NSUrl(value),
                    placeholder: UIImage.FromBundle("placeholder.png")
                );
                image = value;
            }
        }

        public int CurrentImageIndex { get; set; }

        public event EventHandler<EventArgs> ItemDeleted;

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            PostingTitle.Text = post.Title;
            PostingDescription.Text = post.Description;
            CurrentImageIndex = 0;

            imageHelper = new ListingImageDownloader(post.Link);

            imageHelper.loadingComplete += (object sender, EventArgs e) =>
            {
                imageCollectionView.RegisterClassForCell(typeof(ListingImageCell), "listingCell");
                collectionSource = new ImageCollectionViewSource(this, imageHelper.images);
                imageCollectionView.Source = collectionSource;
            };


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
                Image = post.ImageLink;
            }
            else
            {
                postingImageView.Image = UIImage.FromBundle("placeholder.png");
            }

            UITapGestureRecognizer singletap = new UITapGestureRecognizer(OnSingleTap) {
                NumberOfTapsRequired = 1
            };

            imageScrollView.AddGestureRecognizer(singletap); // detect when the scrollView is double-tapped

            DoneButton.Clicked += OnDismiss;
        }

        private void OnSingleTap (UIGestureRecognizer gesture) 
        {
            var storyboard = UIStoryboard.FromName("Main", null);
            postingImageViewController postingImageVC = (postingImageViewController)storyboard.InstantiateViewController("postingImageViewController");
            postingImageVC.ImageLinks = imageHelper.images;
            postingImageVC.ImageIndex = CurrentImageIndex;
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
