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

            scrollViewSize = imageViewPlaceholder.Bounds.Size;

            PostingTitle.Text = Post.PostTitle;
            PostingDescription.Text = Post.Description;
            CurrentImageIndex = 0;

            PostingTitle.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;

            imageHelper = new ListingImageDownloader(Post.Link, Post.ImageLink);

            imageHelper.loadingComplete += (object sender, EventArgs e) =>
                {
                    PostingDescription.Text = imageHelper.postingDescription;
                    imageCollectionView.RegisterClassForCell(typeof(ListingImageCell), "listingCell");
                    collectionSource = new ImageCollectionViewSource(this, imageHelper.images);
                    imageCollectionView.Source = collectionSource;
                };


            myNavBarItem.SetLeftBarButtonItem(null, true);

            if (!Saved)
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

            DoneButton.Clicked += OnDismiss;
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

        void OnDismiss(object sender, EventArgs e)
        {
            DismissViewController(true, null);
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
//
//        void AddLayoutConstraints()
//        {
//            this.View.RemoveConstraints(constraints: this.View.Constraints);
//
//            myNavBar.TranslatesAutoresizingMaskIntoConstraints = false;
//            statusBarColorPlaceholder.TranslatesAutoresizingMaskIntoConstraints = false;
//            PostingTitle.TranslatesAutoresizingMaskIntoConstraints = false;
//            imageScrollView.TranslatesAutoresizingMaskIntoConstraints = false;
//            postingImageView.TranslatesAutoresizingMaskIntoConstraints = false;
//            imageCollectionView.TranslatesAutoresizingMaskIntoConstraints = false;
//            PostingDescription.TranslatesAutoresizingMaskIntoConstraints = false;
//            dateLabel.TranslatesAutoresizingMaskIntoConstraints = false;
//
//            //NavBar Constraints
//            this.View.AddConstraints(new NSLayoutConstraint[] {
////                NSLayoutConstraint.Create(myNavBar, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, 1, 0),
//                NSLayoutConstraint.Create(myNavBar, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Top, 1, 20),
//                NSLayoutConstraint.Create(myNavBar, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Left, 1, 0),
//                NSLayoutConstraint.Create(myNavBar, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Right, 1, 0),
//            });
//            //Status Bar Color Placeholder Constraints
//            this.View.AddConstraints(new NSLayoutConstraint[] {
//                NSLayoutConstraint.Create(statusBarColorPlaceholder, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, 1, 0),
//                NSLayoutConstraint.Create(statusBarColorPlaceholder, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Top, 1, 0),
//                NSLayoutConstraint.Create(statusBarColorPlaceholder, NSLayoutAttribute.Height, NSLayoutRelation.Equal, myNavBar, NSLayoutAttribute.Height, 1, 0),
//                NSLayoutConstraint.Create(statusBarColorPlaceholder, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1, 0),
//            });
//            //Title Label Constraints
//            this.View.AddConstraints(new NSLayoutConstraint[] {
//                NSLayoutConstraint.Create(PostingTitle, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, 1, 0),
//                NSLayoutConstraint.Create(PostingTitle, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1, 0),
//                NSLayoutConstraint.Create(PostingTitle, NSLayoutAttribute.Top, NSLayoutRelation.Equal, myNavBar, NSLayoutAttribute.Bottom, 1, 75),
//            });
//            //ImageScrollView Constraints
//            this.View.AddConstraints(new NSLayoutConstraint[] {
//                NSLayoutConstraint.Create(imageScrollView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, 1, 0),
//                NSLayoutConstraint.Create(imageScrollView, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1, 0),
//                NSLayoutConstraint.Create(imageScrollView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, PostingTitle, NSLayoutAttribute.Bottom, 1, 5),
//                NSLayoutConstraint.Create(imageScrollView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, 100),
//            });
//            imageScrollView.AddSubview(postingImageView);
//            //Posting Image View Constraints
////            this.View.AddConstraints(new NSLayoutConstraint[] {
////                NSLayoutConstraint.Create(postingImageView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, imageScrollView, NSLayoutAttribute.Width, 1, 0),
////                NSLayoutConstraint.Create(postingImageView, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, imageScrollView, NSLayoutAttribute.CenterX, 1, 0),
////                NSLayoutConstraint.Create(postingImageView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, imageScrollView, NSLayoutAttribute.Top, 1, 0),
////                NSLayoutConstraint.Create(postingImageView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, imageScrollView, NSLayoutAttribute.Height, 1, 0),
////            });
//            //Image CollectionView Constraints
//            this.View.AddConstraints(new NSLayoutConstraint[] {
//                NSLayoutConstraint.Create(imageCollectionView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .50f, 0),
//                NSLayoutConstraint.Create(imageCollectionView, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 0.50f, 0),
//                NSLayoutConstraint.Create(imageCollectionView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, imageScrollView, NSLayoutAttribute.Bottom, 1, 0),
//                NSLayoutConstraint.Create(imageCollectionView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, 25),
//            });
//            //Posting Description Constraints
//            this.View.AddConstraints(new NSLayoutConstraint[] {
//                NSLayoutConstraint.Create(PostingDescription, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .50f, 0),
//                NSLayoutConstraint.Create(PostingDescription, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1.5f, 0),
//                NSLayoutConstraint.Create(PostingDescription, NSLayoutAttribute.Top, NSLayoutRelation.Equal, imageCollectionView, NSLayoutAttribute.Bottom, 1, 10),
//                NSLayoutConstraint.Create(PostingDescription, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, 50),
//            });
//            //Date Label Constraints
//            this.View.AddConstraints(new NSLayoutConstraint[] {
//                NSLayoutConstraint.Create(dateLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .50f, 0),
//                NSLayoutConstraint.Create(dateLabel, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 0.50f, 0),
//                NSLayoutConstraint.Create(dateLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, PostingDescription, NSLayoutAttribute.Bottom, 1, 10),
//            });
//
//            this.View.LayoutIfNeeded();
//        }
	}
}
