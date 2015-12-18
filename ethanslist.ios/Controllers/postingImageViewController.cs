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
                    new NSUrl(value),
                    UIImage.FromBundle("placeholder.png"),
                    SDWebImageOptions.HighPriority,
                    null,
                    (image,error,cachetype,NSNull) => {
                        myImageView.ContentMode = UIViewContentMode.Center;
                        myImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
                    }
                );
                image = value;
            }
        }

        public override void LoadView()
        {
            base.LoadView();
            this.View.BackgroundColor = ColorScheme.Clouds;
            myScrollView.BackgroundColor = ColorScheme.Clouds;
            myScrollView.BackgroundColor.ColorWithAlpha(0.7f);
            myImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

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
