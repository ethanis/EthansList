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
            };
            AddSubview(PostingImage);

            PostingTitle = new UILabel(){
                
            };
            AddSubview(PostingTitle);

            PostingDescription = new UILabel(){
                TextColor = ColorScheme.Asbestos,
                Lines = 5,
                LineBreakMode = UILineBreakMode.WordWrap
            };
            AddSubview(PostingDescription);

            Separator = new UIView(){BackgroundColor = ColorScheme.Concrete};
            AddSubview(Separator);
        }

        public override void LayoutSubviews()
        {
            var bounds = Bounds;

            PostingImage.Frame = new CGRect(
                15,
                10,
                bounds.Width * 0.25f,
                bounds.Height - 20
            );

            PostingTitle.Frame = new CGRect(
                (bounds.Width * 0.25f) + 25,
                10,
                (bounds.Width * 0.75) - 40,
                20
            );

            PostingDescription.Frame = new CGRect(
                (bounds.Width * 0.25f) + 25,
                40,
                (bounds.Width * 0.75) - 40,
                30
            );

            Separator.Frame = new CGRect(
                15,
                bounds.Height-1,
                bounds.Width,
                1
            );
        }
    }
}
