using System;

using Foundation;
using UIKit;

namespace ethanslist.ios
{
    public partial class PostingImageCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("PostingImageCell");
        public static readonly UINib Nib;
        public UIImageView MainImage {get{ return PostingImage; }}

        static PostingImageCell()
        {
            Nib = UINib.FromName("PostingImageCell", NSBundle.MainBundle);
        }

        public PostingImageCell(IntPtr handle)
            : base(handle)
        {
        }

        public PostingImageCell()
        {
        }

        public static PostingImageCell Create()
        {
            return (PostingImageCell)Nib.Instantiate(null, null)[0];
        }
    }
}
