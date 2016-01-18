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
        UIScrollView scrollView;
        protected CGSize scrollViewSize;
        protected SmallLoadingOverlay _loadingOverlay = null;

		public PostingDetailsViewController (IntPtr handle) : base (handle)
		{
		}

        public PostingDetailsViewController()
        {
        }
            
        public Posting Post { get; set; }
        public Boolean Saved { get; set; }

        string image;
        public string Image
        {
            get{ 
                return image;
            }
            set {
                postingImageView.ContentMode = UIViewContentMode.Center;
                postingImageView.SetImage(
                    new NSUrl(value),
                    UIImage.FromBundle("placeholder.png"),
                    SDWebImageOptions.HighPriority,
                    null,
                    (image,error,cachetype,NSNull) => {
                        postingImageView.ContentMode = UIViewContentMode.Center;
                        postingImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
                        CenterImage();
                    }
                );
                image = value;
            }
        }

        void CenterImage()
        {
            // center the image as it becomes smaller than the size of the screen
            CGSize boundsSize = scrollViewSize;
            CGRect frameToCenter = postingImageView.Frame;

            // center horizontally
            if (frameToCenter.Size.Width < boundsSize.Width)
                frameToCenter.X = (this.View.Bounds.Size.Width - (frameToCenter.Size.Width)) / 2;
            else
                frameToCenter.X = 0;

            // center vertically
            if (frameToCenter.Size.Height < boundsSize.Height)
                frameToCenter.Y = ((boundsSize.Height - frameToCenter.Size.Height) / 2) - UIApplication.SharedApplication.StatusBarFrame.Height;
            else
                frameToCenter.Y = imageViewPlaceholder.Frame.Y - UIApplication.SharedApplication.StatusBarFrame.Height;

            postingImageView.Frame = frameToCenter;
        }

        public int CurrentImageIndex { get; set; }
        public event EventHandler<EventArgs> ItemDeleted;

        public override void LoadView()
        {
            base.LoadView();
            
            this.View.Layer.BackgroundColor = ColorScheme.Clouds.CGColor;
            this.myNavBar.BarTintColor = ColorScheme.WetAsphalt;
            statusBarColorPlaceholder.BackgroundColor = UIColor.FromRGB(0.2745f, 0.3451f, 0.4157f);
            PostingDescription.BackgroundColor = ColorScheme.Clouds;
        }

        public override void ViewDidAppear(bool animated)
        {
            CenterImage();

            base.ViewDidAppear(animated);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            scrollView = new UIScrollView();
            scrollView.Frame = this.View.Bounds;
            scrollView.ContentSize = UIScreen.MainScreen.Bounds.Size;
            scrollView.AddSubviews(this.View.Subviews);
            this.View.InsertSubview(scrollView, 0);

            if (Post.ImageLink != "-1")
            {
                var bounds = imageCollectionView.Bounds;
                _loadingOverlay = new SmallLoadingOverlay(bounds);
                imageCollectionView.Add(_loadingOverlay);
            }
            scrollViewSize = imageViewPlaceholder.Bounds.Size;

            PostingTitle.Text = Post.PostTitle;
            PostingDescription.Text = Post.Description;
            CurrentImageIndex = 0;

            PostingTitle.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;

            imageHelper = new ListingImageDownloader(Post.Link, Post.ImageLink);

            imageHelper.loadingComplete += (object sender, EventArgs e) =>
                {
                    if (_loadingOverlay != null)
                        _loadingOverlay.Hide();
                    PostingDescription.Text = imageHelper.postingDescription;
                    imageCollectionView.RegisterClassForCell(typeof(ListingImageCell), "listingCell");
                    collectionSource = new ImageCollectionViewSource(this, imageHelper.images);
                    imageCollectionView.Source = collectionSource;
                };

            myNavBarItem.SetLeftBarButtonItem(null, true);

            //TODO: Handle case where posting is saved, but access again through feed results table
            if (!Saved)
            {
                saveButton = new UIBarButtonItem (
                    UIImage.FromFile ("save.png"),
                    UIBarButtonItemStyle.Plain,
                    SaveListing
                );
                saveButton.Enabled = true;
                myNavBarItem.LeftBarButtonItem = saveButton;
            }
            else
            {
                deleteButton = new UIBarButtonItem(UIBarButtonSystemItem.Trash, DeleteListing);
                myNavBarItem.SetLeftBarButtonItem(deleteButton, true);
            }

            dateLabel.Text = "Listed: " + Post.Date.ToShortDateString() + " at " + Post.Date.ToShortTimeString();

            if (Post.ImageLink != "-1")
            {
                Image = Post.ImageLink;
            }
            else
            {
                postingImageView.Image = UIImage.FromBundle("placeholder.png");
            }

            UITapGestureRecognizer singletap = new UITapGestureRecognizer(OnSingleTap) {
                NumberOfTapsRequired = 1
            };

            UISwipeGestureRecognizer swipeRight = new UISwipeGestureRecognizer(OnSwipeRight) { 
                Direction = UISwipeGestureRecognizerDirection.Right
            };
            UISwipeGestureRecognizer swipeLeft = new UISwipeGestureRecognizer(OnSwipeLeft) { 
                Direction = UISwipeGestureRecognizerDirection.Left
            };

            imageScrollView.AddGestureRecognizer(singletap); // detect when the scrollView is double-tapped
            imageScrollView.AddGestureRecognizer(swipeLeft);
            imageScrollView.AddGestureRecognizer(swipeRight);

            DoneButton.Clicked += (sender, e) => DismissViewController(true, null);
        }

        private void OnSingleTap (UIGestureRecognizer gesture) 
        {
            if (imageHelper.images.Count > 0)
            {
                var storyboard = UIStoryboard.FromName("Main", null);
                postingImageViewController postingImageVC = (postingImageViewController)storyboard.InstantiateViewController("postingImageViewController");
                postingImageVC.ImageLinks = imageHelper.images;
                postingImageVC.ImageIndex = CurrentImageIndex;

                this.ShowViewController(postingImageVC, this);
            }
        }

        private void OnSwipeRight (UIGestureRecognizer gesture)
        {
            if (CurrentImageIndex > 0)
            {
                CurrentImageIndex -= 1;
                Image = imageHelper.images[CurrentImageIndex];
            }
        }

        private void OnSwipeLeft (UIGestureRecognizer gesture)
        {
            if (CurrentImageIndex < imageHelper.images.Count - 1)
            {
                CurrentImageIndex += 1;
                Image = imageHelper.images[CurrentImageIndex];
            }
        }

        async void SaveListing(object sender, EventArgs e)
        {
            await AppDelegate.databaseConnection.AddNewListingAsync(Post.PostTitle, Post.Description, Post.Link, Post.ImageLink, Post.Date);
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
