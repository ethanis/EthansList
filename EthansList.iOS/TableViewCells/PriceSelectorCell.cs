using System;

using Foundation;
using UIKit;

namespace ethanslist.ios
{
    public partial class PriceSelectorCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("PriceSelectorCell");
        public static readonly UINib Nib;
        public string LabelText { get; set;}
        public UILabel MinPrice
        {
            get { return MinPriceField; }
        }
        public UILabel MaxPrice
        {
            get { return MaxPriceField; }
        }

        public UILabel ToLabel {get{ return toLabel; }}
        public UITextField PickerField { get { return PickerTextField; } }

        static PriceSelectorCell()
        {
            Nib = UINib.FromName("PriceSelectorCell", NSBundle.MainBundle);
        }

        public PriceSelectorCell(IntPtr handle)
            : base(handle)
        {
        }

        public PriceSelectorCell() :base()
        {
        }

        public static PriceSelectorCell Create()
        {
            return (PriceSelectorCell)Nib.Instantiate(null, null)[0];
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            this.Heading.AttributedText = new NSAttributedString(LabelText, Constants.LabelAttributes);
            MaxPriceField.AttributedText = new NSAttributedString(MaxPriceField.Text, Constants.LabelAttributes);
            MinPriceField.AttributedText = new NSAttributedString(MinPriceField.Text, Constants.LabelAttributes);
            toLabel.AttributedText = new NSAttributedString(toLabel.Text, Constants.LabelAttributes);

            MinPriceField.BackgroundColor = UIColor.White;
            MaxPriceField.BackgroundColor = UIColor.White;
        }
    }
}
