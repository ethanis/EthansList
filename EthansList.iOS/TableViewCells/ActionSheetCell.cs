using System;

using Foundation;
using UIKit;

namespace ethanslist.ios
{
    public partial class ActionSheetCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("ActionSheetCell");
        public static readonly UINib Nib;
        public string Title { get; set; }
        public UILabel MinimumLabel 
        {
            get { return MinLabel; }
        }

        static ActionSheetCell()
        {
            Nib = UINib.FromName("ActionSheetCell", NSBundle.MainBundle);
        }

        public ActionSheetCell(IntPtr handle)
            : base(handle)
        {
        }
            

        public ActionSheetCell() : base()
        {
        }

        public static ActionSheetCell Create()
        {
            return (ActionSheetCell)Nib.Instantiate(null, null)[0];
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            this.Heading.Text = Title;
        }
    }
}
