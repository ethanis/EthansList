using System;
using UIKit;
using Foundation;
using CoreGraphics;
using SDWebImage;
using System.Drawing;

namespace ethanslist.ios
{
    public class ListingImageCell : UICollectionViewCell
    {
        public UIImageView imageView;

        [Export ("initWithFrame:")]
        public ListingImageCell (CGRect frame) : base (frame)
        {
            ContentView.BackgroundColor = UIColor.White;

            imageView = new UIImageView (UIImage.FromBundle ("placeholder.png"));
//            imageView.Transform = CGAffineTransform.MakeScale (0.2f, 0.2f);
            imageView.Bounds = ContentView.Bounds;
            imageView.Center = ContentView.Center;

            ContentView.AddSubview (imageView);
        }

        public string Image {
            set {
                imageView.SetImage(
                    url: new NSUrl(value),
                    placeholder: UIImage.FromBundle("placeholder.png")
                );
            }
        }
    }
}

