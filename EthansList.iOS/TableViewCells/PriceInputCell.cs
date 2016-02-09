using System;

using Foundation;
using UIKit;

namespace ethanslist.ios
{
    public partial class PriceInputCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("PriceInputCell");
        public static readonly UINib Nib;
        private NSNumberFormatter numberFormatter;

        static PriceInputCell()
        {
            Nib = UINib.FromName("PriceInputCell", NSBundle.MainBundle);
        }

        public PriceInputCell(IntPtr handle)
            : base(handle)
        {
        }

        public static PriceInputCell Create()
        {
            return (PriceInputCell)Nib.Instantiate(null,null)[0];
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            PriceLabel.AttributedText = new NSAttributedString("Price", Constants.LabelAttributes);
//            MinPriceField = new UITextField() { Delegate = new PriceTextDelegate() };
//            this.AddSubview(MinPriceField);
//
//            MinPriceField.BorderStyle = UITextBorderStyle.RoundedRect;
//            MinPriceField.Placeholder = "Min";
//
            MinPriceField.TranslatesAutoresizingMaskIntoConstraints = false;
            MaxPriceField.TranslatesAutoresizingMaskIntoConstraints = false;

            this.AddConstraints(new NSLayoutConstraint[]{
                NSLayoutConstraint.Create(MinPriceField, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1, this.Bounds.Height * 0.1f),
                NSLayoutConstraint.Create(MaxPriceField, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1, this.Bounds.Height * 0.1f),
                NSLayoutConstraint.Create(MinPriceField, NSLayoutAttribute.Height, NSLayoutRelation.Equal, this, NSLayoutAttribute.Height, 0.8f, 0),
                NSLayoutConstraint.Create(MaxPriceField, NSLayoutAttribute.Height, NSLayoutRelation.Equal, this, NSLayoutAttribute.Height, 0.8f, 0),
                NSLayoutConstraint.Create(MinPriceField, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 0.30f, 0),
                NSLayoutConstraint.Create(MaxPriceField, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 0.30f, 0),
                NSLayoutConstraint.Create(MinPriceField, NSLayoutAttribute.Right, NSLayoutRelation.Equal, MaxPriceField, NSLayoutAttribute.Left, 1, -15),
                NSLayoutConstraint.Create(MaxPriceField, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 1, -15),
            });

            numberFormatter = new NSNumberFormatter();
            numberFormatter.NumberStyle = NSNumberFormatterStyle.Currency;

            MinPriceField.ClearButtonMode = UITextFieldViewMode.WhileEditing;
            MaxPriceField.ClearButtonMode = UITextFieldViewMode.WhileEditing;

            MinPriceField.KeyboardType = UIKeyboardType.NumberPad;
            MaxPriceField.KeyboardType = UIKeyboardType.NumberPad;

            MinPriceField.Delegate = new PriceTextDelegate();
            MaxPriceField.Delegate = new PriceTextDelegate();
        }

    }

    public class PriceTextDelegate : UITextFieldDelegate
    {
        public override bool ShouldChangeCharacters(UITextField textField, NSRange range, string replacementString)
        {
            //TODO: This needs improvement
            string text = textField.Text;
            if (text.Length > 0 && text.Substring(0, 1) == "$")
                textField.Text = text;
            else
                textField.Text = "$" + text;

            return true;
        }
    }
}
