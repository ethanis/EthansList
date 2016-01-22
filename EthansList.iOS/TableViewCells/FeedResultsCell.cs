using System;

using Foundation;
using UIKit;

namespace ethanslist.ios
{
    public partial class FeedResultsCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("FeedResultsCell");
        public static readonly UINib Nib;
        public UILabel PostingTitle { get{ return TitleLabel; }}
        public UILabel PostingDescription { get{ return DescriptionLabel; }}
        public UIImageView PostingImage { get{ return PostingImageView; }}

        static FeedResultsCell()
        {
            Nib = UINib.FromName("FeedResultsCell", NSBundle.MainBundle);
        }

        public FeedResultsCell(IntPtr handle)
            : base(handle)
        {
        }

        public static FeedResultsCell Create()
        {
            return (FeedResultsCell)Nib.Instantiate(null, null)[0];
        }
    }
}
