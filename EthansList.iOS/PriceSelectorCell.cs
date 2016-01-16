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
        public UITextField MinPrice
        {
            get { return MinPriceField; }
        }
        public UITextField MaxPrice
        {
            get { return MaxPriceField; }
        }

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

            this.Heading.Text = LabelText;

            this.MinPriceField.EditingDidBegin += delegate { this.MinPriceField.BecomeFirstResponder(); };

            this.MinPriceField.ShouldReturn += delegate {
                MinPriceField.ResignFirstResponder();
                return true;
            };

            this.MaxPriceField.EditingDidBegin += delegate { this.MinPriceField.BecomeFirstResponder(); };

            this.MaxPriceField.ShouldReturn += delegate {
                MaxPriceField.ResignFirstResponder();
                return true;
            };
        }
    }
}
