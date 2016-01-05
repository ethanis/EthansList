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
                myImageView.ContentMode = UIViewContentMode.Center;
                myImageView.SetImage(
                    new NSUrl(value),
                    UIImage.FromBundle("placeholder.png"),
                    SDWebImageOptions.HighPriority,
                    null,
                    (image,error,cachetype,NSNull) => {
                        myImageView.ContentMode = UIViewContentMode.Center;
                        myImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
                        CenterImage();
                    }
                );
                image = value;
            }
        }

        void CenterImage()
        {
            // center the image as it becomes smaller than the size of the screen
            CGSize boundsSize = this.View.Bounds.Size;
            CGRect frameToCenter = myImageView.Frame;

            // center horizontally
            if (frameToCenter.Size.Width < boundsSize.Width)
                frameToCenter.X = (boundsSize.Width - frameToCenter.Size.Width) / 2;
            else
                frameToCenter.X = 0;

            // center vertically
            if (frameToCenter.Size.Height < boundsSize.Height)
                frameToCenter.Y = (boundsSize.Height - frameToCenter.Size.Height) / 2;
            else
                frameToCenter.Y = 0;

            myImageView.Frame = frameToCenter;
        }

        public override void LoadView()
        {
            base.LoadView();

            this.View.BackgroundColor = ColorScheme.Clouds;
            myScrollView.BackgroundColor = ColorScheme.Clouds;
            myScrollView.BackgroundColor.ColorWithAlpha(0.7f);
            myImageView.ContentMode = UIViewContentMode.Center;
            myImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            CenterImage();
            if (ImageLinks[ImageIndex] != "-1")
            {
                Image = ImageLinks[ImageIndex];
            }
        }

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            UIApplication.SharedApplication.SetStatusBarHidden(true, false);

            myScrollView.Frame = UIScreen.MainScreen.Bounds;
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

            UISwipeGestureRecognizer dismissSwipe = new UISwipeGestureRecognizer(OnDismissSwipe)
            {
                Direction = UISwipeGestureRecognizerDirection.Down
            };

            UISwipeGestureRecognizer onSwipeNext = new UISwipeGestureRecognizer(OnSwipeNext)
            { 
                Direction = UISwipeGestureRecognizerDirection.Left
            };

            UISwipeGestureRecognizer onSwipePrevious = new UISwipeGestureRecognizer(OnSwipePrevious)
            {
                Direction = UISwipeGestureRecognizerDirection.Right
            };
            
            View.AddGestureRecognizer(doubletap);
            View.AddGestureRecognizer(dismissSwipe);
            View.AddGestureRecognizer(onSwipeNext);
            View.AddGestureRecognizer(onSwipePrevious);
        }

        private void OnDoubleTap (UIGestureRecognizer gesture) {
            myScrollView.SetZoomScale(1, true);
            myImageView.ContentMode = UIViewContentMode.Center;
        }

        private void OnDismissSwipe (UIGestureRecognizer gesture) {
            UIApplication.SharedApplication.SetStatusBarHidden(false, false);
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
