using System;

using Foundation;
using UIKit;

namespace ethanslist.ios
{
    public partial class SearchTermsCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("SearchTermsCell");
        public static readonly UINib Nib;
        public UITextField TermsField { get { return this.SearchTermField; }}

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

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            this.SearchTermField.EditingDidBegin += delegate { this.SearchTermField.BecomeFirstResponder(); };
            this.SearchTermField.EditingDidEnd += delegate
            {
                    this.SearchTermField.ResignFirstResponder();
            };

            this.SearchTermField.ShouldReturn += delegate {
                SearchTermField.ResignFirstResponder();
                return true;
            };
        }
    }
}
