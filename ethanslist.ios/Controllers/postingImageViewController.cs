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

        string image;
        private string Image
        {
            get{ 
                return image;
            }
            set {
                myImageView.SetImage(
                    url: new NSUrl(value),
                    placeholder: UIImage.FromBundle("placeholder.png")
                );
                myImageView.Center = myScrollView.Center;
                myImageView.ContentMode = UIViewContentMode.Center;
                image = value;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var bounds = UIScreen.MainScreen.Bounds; // portrait bounds
            if (UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeLeft || UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeRight) {
                bounds.Size = new CGSize(bounds.Size.Height, bounds.Size.Width);
            }
               
            myScrollView.BackgroundColor = UIColor.LightGray;
            myScrollView.Frame = bounds;
            myImageView.Center = myScrollView.Center;
            myImageView.ContentMode = UIViewContentMode.Center;

            if (ImageLinks[ImageIndex] != "-1")
            {
                Image = ImageLinks[ImageIndex];
            }

            myScrollView.MaximumZoomScale = 4f;
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
            myImageView.ContentMode = UIViewContentMode.Center;
        }

        private void OnDismissSwipe (UIGestureRecognizer gesture) {
            this.DismissViewController(true, null);
        }

        private void OnSwipeNext (UIGestureRecognizer gesture) {
            if (ImageIndex >= ImageLinks.Count - 1)
                return;
            
            ImageIndex += 1;
            Image = ImageLinks[ImageIndex];
        }

        private void OnSwipePrevious (UIGestureRecognizer gesture) {
            if (ImageIndex == 0)
                return;
            
            ImageIndex -= 1;
            Image = ImageLinks[ImageIndex];
        }
	}
}
