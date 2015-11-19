using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using EthansList.Shared;
using SDWebImage;
using CoreGraphics;

namespace ethanslist.ios
{
	partial class PostingDetailsViewController : UIViewController
	{
//        UIScrollView scrollView;

		public PostingDetailsViewController (IntPtr handle) : base (handle)
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

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            PostingTitle.Text = post.Title;
            PostingDescription.Text = post.Description;
            Console.WriteLine(post.ImageLink);
            Console.WriteLine(post.Date);

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

            UITapGestureRecognizer doubletap = new UITapGestureRecognizer(OnDoubleTap) {
                NumberOfTapsRequired = 2 // double tap
            };

            scrollView.AddGestureRecognizer(doubletap); // detect when the scrollView is double-tapped

            DoneButton.Clicked += OnDismiss;

            SaveButton.Clicked += SaveListing;
        }

        private void OnDoubleTap (UIGestureRecognizer gesture) 
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

        void SaveListing(object sender, EventArgs e)
        {
            AppDelegate.databaseConnection.AddNewListingAsync(post.Title, post.Description, post.Link, post.ImageLink, post.Date);
            Console.WriteLine(AppDelegate.databaseConnection.StatusMessage);
        }


        static UIImage FromUrl (string uri)
        {
            using (var url = new NSUrl (uri))
            using (var data = NSData.FromUrl (url))
                return UIImage.LoadFromData (data);
        }
	}
}
