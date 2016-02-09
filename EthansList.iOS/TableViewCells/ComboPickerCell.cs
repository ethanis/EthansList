using System;

using Foundation;
using UIKit;

namespace ethanslist.ios
{
    public partial class ComboPickerCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("ComboPickerCell");
        public static readonly UINib Nib;
        public UILabel Title {get { return TitleLabel; }}
        public UILabel Display { get{ return DisplayLabel; }}

        static ComboPickerCell()
        {
            Nib = UINib.FromName("ComboPickerCell", NSBundle.MainBundle);
        }

        public ComboPickerCell(IntPtr handle)
            : base(handle)
        {
        }

        public static ComboPickerCell Create()
        {
            return (ComboPickerCell)Nib.Instantiate(null,null)[0];
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            this.AddConstraints(new NSLayoutConstraint[]{
                NSLayoutConstraint.Create(TitleLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Left, 1, 20),
                NSLayoutConstraint.Create(TitleLabel, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1, 0),
                NSLayoutConstraint.Create(DisplayLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 1, -20),
                NSLayoutConstraint.Create(DisplayLabel, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1, 0),
            });
        }
    }
}
