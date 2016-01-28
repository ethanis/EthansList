using System;

using Foundation;
using UIKit;

namespace ethanslist.ios
{
    public partial class PickerSelectorCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("PickerSelectorCell");
        public static readonly UINib Nib;
        public UILabel Title { get { return TitleLabel; }}
        public UILabel Display { get { return NumberLabel; }}
        public UITextField InputTextField { get{ return TextField; }}

        static PickerSelectorCell()
        {
            Nib = UINib.FromName("PickerSelectorCell", NSBundle.MainBundle);
        }

        public PickerSelectorCell(IntPtr handle)
            : base(handle)
        {
        }

        public PickerSelectorCell()
        {
        }

        public static PickerSelectorCell Create()
        {
            return (PickerSelectorCell)Nib.Instantiate(null, null)[0];
        }
    }
}
