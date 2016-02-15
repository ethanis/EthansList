using System;
using EthansList.Shared;
using CoreGraphics;
using SDWebImage;
using System.Collections.Generic;
using UIKit;
using Foundation;

namespace ethanslist.ios
{
    public class FullScreenImageViewController : UIViewController
    {
        UIImageView closeIcon;
        UIImageView CurrentImage;
        UIScrollView scrollview;
        UIView holderView;

        public List<string> ImageLinks { get; set; }
        public int ImageIndex { get; set; }

        string image;
        private string Image
        {
            get { return image; }
            set {
                CurrentImage.ContentMode = UIViewContentMode.Center;
                CurrentImage.SetImage(
                    new NSUrl(value),
                    UIImage.FromBundle("placeholder.png"),
                    SDWebImageOptions.HighPriority,
                    null,
                    (image,error,cachetype,NSNull) => {
                    CurrentImage.ContentMode = UIViewContentMode.ScaleAspectFit;
                }
                );
                image = value;
            }
        }

        public FullScreenImageViewController()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            UIApplication.SharedApplication.SetStatusBarHidden(true, false);

            if (ImageLinks[ImageIndex] != "-1")
                Image = ImageLinks[ImageIndex];

            scrollview.MaximumZoomScale = 4f;
            scrollview.MinimumZoomScale = .1f;
            scrollview.ViewForZoomingInScrollView += (UIScrollView sv) => { return CurrentImage; };


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
            closeIcon.AddGestureRecognizer(new UITapGestureRecognizer(CloseIconTap));
            holderView.AddGestureRecognizer(doubletap);
            holderView.AddGestureRecognizer(dismissSwipe);
            holderView.AddGestureRecognizer(onSwipeNext);
            holderView.AddGestureRecognizer(onSwipePrevious);
        }

        private void CloseIconTap (UITapGestureRecognizer gesture)
        {
            UIApplication.SharedApplication.SetStatusBarHidden(false, false);
            this.DismissViewController(true, null);
        }

        private void OnDoubleTap (UIGestureRecognizer gesture) {
            scrollview.SetZoomScale(1, true);
            CurrentImage.ContentMode = UIViewContentMode.Center;
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

        public override void LoadView()
        {
            base.LoadView();

            this.View.Layer.BackgroundColor = ColorScheme.Clouds.CGColor;
            closeIcon = new UIImageView(UIImage.FromBundle("Delete-50.png"));
            CurrentImage = new UIImageView();
            scrollview = new UIScrollView(this.View.Frame);
            holderView = new UIView(this.View.Frame);

            holderView.AddSubviews(new UIView[]{closeIcon, CurrentImage});
            scrollview.AddSubview(holderView);
            this.View.AddSubview(scrollview);

            AddLayoutConstraints();

            this.View.BackgroundColor = ColorScheme.Clouds;
            scrollview.BackgroundColor = ColorScheme.Clouds;
            scrollview.BackgroundColor.ColorWithAlpha(0.7f);
            CurrentImage.ContentMode = UIViewContentMode.ScaleAspectFit;
            closeIcon.UserInteractionEnabled = true;
        }

        void AddLayoutConstraints()
        {
            holderView.TranslatesAutoresizingMaskIntoConstraints = false;
            closeIcon.TranslatesAutoresizingMaskIntoConstraints = false;
            CurrentImage.TranslatesAutoresizingMaskIntoConstraints = false;

            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(holderView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, 1, 0),
                NSLayoutConstraint.Create(holderView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Height, 1, 0),
                NSLayoutConstraint.Create(holderView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Top, 1, 0),
                NSLayoutConstraint.Create(holderView, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Left, 1, 0),
            });

            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(closeIcon, NSLayoutAttribute.Width, NSLayoutRelation.Equal, holderView, NSLayoutAttribute.Width, 0.1f, 0),
                NSLayoutConstraint.Create(closeIcon, NSLayoutAttribute.Height, NSLayoutRelation.Equal, holderView, NSLayoutAttribute.Width, 0.1f, 0),
                NSLayoutConstraint.Create(closeIcon, NSLayoutAttribute.Top, NSLayoutRelation.Equal, holderView, NSLayoutAttribute.Top, 1, 5),
                NSLayoutConstraint.Create(closeIcon, NSLayoutAttribute.Right, NSLayoutRelation.Equal, holderView, NSLayoutAttribute.Right, 1, -5),
            });

            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(CurrentImage, NSLayoutAttribute.Width, NSLayoutRelation.Equal, holderView, NSLayoutAttribute.Width, 1, 0),
                NSLayoutConstraint.Create(CurrentImage, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, holderView, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(CurrentImage, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, holderView, NSLayoutAttribute.CenterY, 1, 0),
            });

            this.View.LayoutIfNeeded();
        }
    }
}

