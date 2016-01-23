﻿using System;

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

            this.Heading.Text = LabelText;

            MinPriceField.BackgroundColor = UIColor.White;
            MaxPriceField.BackgroundColor = UIColor.White;
        }
    }
}