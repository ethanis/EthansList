using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using EthansList.Shared;
using CoreGraphics;
using SDWebImage;
using System.Collections.Generic;

namespace ethanslist.ios
{
	partial class postingImageViewController : UIViewController
	{
		public postingImageViewController (IntPtr handle) : base (handle)
		{
		}

        public List<string> ImageLinks { get; set; }
        public int ImageIndex { get; set; }

        Posting post;
        public Posting Post
        {
            get { 
                return post;
            }
            set { 
                post = value;
            }
        }

        string image;
        public string Image
        {
            get{ 
                return image;
            }
            set {
                myImageView.SetImage(
                    url: new NSUrl(value),
                    placeholder: UIImage.FromBundle("placeholder.png")
                );
                image = value;
            }
        }

        public string ImageLink { get; set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.View.BackgroundColor = UIColor.DarkGray;
            myScrollView.Frame = new CGRect(0, 0, View.Frame.Width, View.Frame.Height);

            if (post.ImageLink != "-1")
            {
//                myImageView.SetImage(
//                    url: new NSUrl(ImageLink),
//                    placeholder: UIImage.FromBundle("placeholder.png")
//                );

                Image = ImageLink;
            }

            myScrollView.MaximumZoomScale = 3f;
            myScrollView.MinimumZoomScale = .1f;
            myScrollView.ViewForZoomingInScrollView += (UIScrollView sv) => { return myImageView; };

            UITapGestureRecognizer doubletap = new UITapGestureRecognizer(OnDoubleTap) 
            {
                NumberOfTapsRequired = 2 // double tap
            };
            myScrollView.AddGestureRecognizer(doubletap); // detect when the scrollView is double-tapped

            UISwipeGestureRecognizer dismissSwipe = new UISwipeGestureRecognizer(OnDismissSwipe)
            {
                    Direction = UISwipeGestureRecognizerDirection.Down
            };
            View.AddGestureRecognizer(dismissSwipe); // detect when the scrollView is swiped down

            UISwipeGestureRecognizer onSwipeNext = new UISwipeGestureRecognizer(OnSwipeNext)
                { 
                    Direction = UISwipeGestureRecognizerDirection.Left
                };
            View.AddGestureRecognizer(onSwipeNext);

            UISwipeGestureRecognizer onSwipePrevious = new UISwipeGestureRecognizer(OnSwipePrevious)
            {
                Direction = UISwipeGestureRecognizerDirection.Right
            };
            View.AddGestureRecognizer(onSwipePrevious);
        }

        private void OnDoubleTap (UIGestureRecognizer gesture) {
            myScrollView.SetZoomScale(1, true);
        }

        private void OnDismissSwipe (UIGestureRecognizer gesture) {
            this.DismissViewController(true, null);
        }

        private void OnSwipeNext (UIGestureRecognizer gesture) {
            if (ImageIndex >= ImageLinks.Count - 1)
                return;
            ImageIndex += 1;

//            myImageView.SetImage(
//                url: new NSUrl(ImageLinks[ImageIndex]),
//                placeholder: UIImage.FromBundle("placeholder.png")
//            );

            Image = ImageLinks[ImageIndex];
        }

        private void OnSwipePrevious (UIGestureRecognizer gesture) {
            if (ImageIndex == 0)
                return;
            ImageIndex -= 1;
//
//            myImageView.SetImage(
//                url: new NSUrl(ImageLinks[ImageIndex]),
//                placeholder: UIImage.FromBundle("placeholder.png")
//            );
            Image = ImageLinks[ImageIndex];
        }
	}
}
