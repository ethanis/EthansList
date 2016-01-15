using System;

using Foundation;
using UIKit;

namespace ethanslist.ios
{
    public partial class SearchTermsCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("SearchTermsCell");
        public static readonly UINib Nib;

        static SearchTermsCell()
        {
            Nib = UINib.FromName("SearchTermsCell", NSBundle.MainBundle);
        }

        public SearchTermsCell(IntPtr handle)
            : base(handle)
        {
        }

        public SearchTermsCell() : base()
        {
        }

        public static SearchTermsCell Create()
        {
            return (SearchTermsCell)Nib.Instantiate(null, null)[0];
        }
    }
}
