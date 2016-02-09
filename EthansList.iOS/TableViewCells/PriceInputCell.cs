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
        public UITextField MinPrice {get{ return MinPriceField; }}
        public UITextField MaxPrice {get{ return MaxPriceField; }}
        public event EventHandler<EventArgs> PriceChanged;
        public UILabel ToField;

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
            ToField = new UILabel();
            ToField.Text = "to";
            ToField.TextAlignment = UITextAlignment.Center;
            ToField.TextColor = ColorScheme.Silver;
            this.AddSubview(ToField);

            ToField.TranslatesAutoresizingMaskIntoConstraints = false;
            MinPriceField.TranslatesAutoresizingMaskIntoConstraints = false;
            MaxPriceField.TranslatesAutoresizingMaskIntoConstraints = false;

            this.AddConstraints(new NSLayoutConstraint[]{
                NSLayoutConstraint.Create(MinPriceField, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1, 0),
                NSLayoutConstraint.Create(MaxPriceField, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1, 0),
                NSLayoutConstraint.Create(MinPriceField, NSLayoutAttribute.Height, NSLayoutRelation.Equal, this, NSLayoutAttribute.Height, 0.8f, 0),
                NSLayoutConstraint.Create(MaxPriceField, NSLayoutAttribute.Height, NSLayoutRelation.Equal, this, NSLayoutAttribute.Height, 0.8f, 0),
                NSLayoutConstraint.Create(MinPriceField, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 0.30f, 0),
                NSLayoutConstraint.Create(MaxPriceField, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 0.30f, 0),
                NSLayoutConstraint.Create(MaxPriceField, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 1, -15),

                NSLayoutConstraint.Create(ToField, NSLayoutAttribute.Right, NSLayoutRelation.Equal, MaxPriceField, NSLayoutAttribute.Left, 1, 0),
                NSLayoutConstraint.Create(ToField, NSLayoutAttribute.Left, NSLayoutRelation.Equal, MinPriceField, NSLayoutAttribute.Right, 1, 0),
                NSLayoutConstraint.Create(ToField, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, 25),
                NSLayoutConstraint.Create(ToField, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1, 0),
            });

            MaxPriceField.AccessibilityIdentifier = "MaxPriceField";
            MinPriceField.AccessibilityIdentifier = "MinPriceField";

            numberFormatter = new NSNumberFormatter();
            numberFormatter.NumberStyle = NSNumberFormatterStyle.Currency;

            MinPriceField.ClearButtonMode = UITextFieldViewMode.WhileEditing;
            MaxPriceField.ClearButtonMode = UITextFieldViewMode.WhileEditing;

            MinPriceField.KeyboardType = UIKeyboardType.NumberPad;
            MaxPriceField.KeyboardType = UIKeyboardType.NumberPad;

            MinPriceField.Delegate = new PriceTextDelegate();
            MaxPriceField.Delegate = new PriceTextDelegate();


            NSNotificationCenter.DefaultCenter.AddObserver (UITextField.TextFieldTextDidChangeNotification, (notification) =>
                {
//                    Console.WriteLine ("Character received! {0}", notification.Object == MinPriceField || notification.Object == MaxPriceField);
                    if (this.PriceChanged != null)
                        this.PriceChanged(this, new EventArgs());
                });
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

        public override void DidChange(NSKeyValueChange changeKind, NSIndexSet indexes, NSString forKey)
        {
            base.DidChange(changeKind, indexes, forKey);
        }

        public override void EditingStarted(UITextField textField)
        {
//            throw new System.NotImplementedException ();
        }
    }
}
