using System;

using Foundation;
using UIKit;

namespace ethanslist.ios
{
    public partial class PostingImageCollectionCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("PostingImageCollectionCell");
        public static readonly UINib Nib;
        public UICollectionView Collection { get{ return ImageCollection; }}

        static PostingImageCollectionCell()
        {
            Nib = UINib.FromName("PostingImageCollectionCell", NSBundle.MainBundle);
        }

        public PostingImageCollectionCell(IntPtr handle)
            : base(handle)
        {
        }

        public PostingImageCollectionCell()
        {
        }

        public static PostingImageCollectionCell Create()
        {
            return (PostingImageCollectionCell)Nib.Instantiate(null,null)[0];
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            ImageCollection.BackgroundColor = ColorScheme.Silver;
        }
    }
}
