using System;

using Foundation;
using UIKit;

namespace ethanslist.ios
{
    public partial class BedBathCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("BedBathCell");
        public static readonly UINib Nib;
        public string Title { get; set; }
        public UILabel MinimumLabel 
        {
            get { return MinLabel; }
        }

        static BedBathCell()
        {
            Nib = UINib.FromName("BedBathCell", NSBundle.MainBundle);
        }

        public BedBathCell(IntPtr handle)
            : base(handle)
        {
        }

        public BedBathCell() : base()
        {
        }

        public static BedBathCell Create()
        {
            return (BedBathCell)Nib.Instantiate(null, null)[0];
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            this.Heading.Text = Title;
        }
    }
}
