using System;

using Foundation;
using UIKit;
using CoreGraphics;

namespace ethanslist.ios
{
    public partial class FeedResultsCell : CustomTableViewCell
    {
        internal static readonly string Key = "FeedResultsCell";
        public UILabel PostingTitle { get; set;}
        public UILabel PostingDescription { get; set;}
        public UIImageView PostingImage { get; set;}
        public UIView Separator;

        public FeedResultsCell()
            : base(Key)
        {
            PostingImage = new UIImageView(){
                ContentMode = UIViewContentMode.ScaleAspectFill,
                ClipsToBounds = true
            };
            AddSubview(PostingImage);

            PostingTitle = new UILabel(){
                
            };
            AddSubview(PostingTitle);

            PostingDescription = new UILabel(){
                TextColor = ColorScheme.Asbestos,
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap,
            };
            AddSubview(PostingDescription);

            Separator = new UIView(){BackgroundColor = ColorScheme.Concrete};
            AddSubview(Separator);
        }

        public override void LayoutSubviews()
        {
            var bounds = Bounds;

            PostingImage.Frame = new CGRect(
                5,
                5,
                bounds.Width * 0.25f,
                bounds.Height - 10
            );

            PostingTitle.Frame = new CGRect(
                (bounds.Width * 0.25f) + 15,
                5,
                (bounds.Width * 0.75) - 20,
                16
            );

            PostingDescription.Frame = new CGRect(
                (bounds.Width * 0.25f) + 15,
                21,
                (bounds.Width * 0.75) - 20,
                54
            );

            Separator.Frame = new CGRect(
                5,
                bounds.Height-1,
                bounds.Width -5,
                1
            );
        }
    }
}
