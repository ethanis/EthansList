using System;

using Foundation;
using UIKit;

namespace ethanslist.ios
{
    public partial class PostingDescriptionCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("PostingDescriptionCell");
        public static readonly UINib Nib;
        public UILabel PostingDescription {get{ return DescriptionLabel; }}

        static PostingDescriptionCell()
        {
            Nib = UINib.FromName("PostingDescriptionCell", NSBundle.MainBundle);
        }

        public PostingDescriptionCell(IntPtr handle)
            : base(handle)
        {
        }

        public PostingDescriptionCell()
        {
        }

        public static PostingDescriptionCell Create()
        {
            return (PostingDescriptionCell)Nib.Instantiate(null,null)[0];
        }
    }
}
