using System;

using Foundation;
using UIKit;

namespace ethanslist.ios
{
    public partial class PostingTitleCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("PostingTitleCell");
        public static readonly UINib Nib;
        public UITextView PostingTitle {get{ return TitleLabel; }}

        static PostingTitleCell()
        {
            Nib = UINib.FromName("PostingTitleCell", NSBundle.MainBundle);
        }

        public PostingTitleCell(IntPtr handle)
            : base(handle)
        {
        }

        public PostingTitleCell()
        {
        }

        public static PostingTitleCell Create()
        {
            return (PostingTitleCell)Nib.Instantiate(null, null)[0];
        }
    }
}
