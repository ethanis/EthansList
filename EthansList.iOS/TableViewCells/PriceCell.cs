using System;
using UIKit;
using Foundation;
using CoreGraphics;

namespace ethanslist.ios
{
    public class PriceCell : CustomTableViewCell
    {
        internal static readonly string Key = "PriceCell";
        UILabel ToField;

        public event EventHandler<EventArgs> NumChanged;
        public UILabel HeaderLabel { get; set;}
        public UITextField MinPriceField {get;set;}
        public UITextField MaxPriceField { get; set; }

        public PriceCell()
            : base(Key)
        {
            HeaderLabel = new UILabel() { Text = "Price" };
            AddSubview (HeaderLabel);

            ToField = new UILabel() { Text = "to", TextColor = ColorScheme.Clouds, TextAlignment = UITextAlignment.Center };
            AddSubview(ToField);

            MinPriceField = new UITextField() {
                Placeholder = "min", 
                TextAlignment = UITextAlignment.Center, 
                BorderStyle = UITextBorderStyle.RoundedRect,
                KeyboardType = UIKeyboardType.NumberPad,
                ClearButtonMode = UITextFieldViewMode.WhileEditing,
                Delegate = new PriceTextDelegate(),
                AccessibilityIdentifier = "MinPriceField"
            };
            AddSubview (MinPriceField);

            MaxPriceField = new UITextField() {
                Placeholder = "max", 
                TextAlignment = UITextAlignment.Center, 
                BorderStyle = UITextBorderStyle.RoundedRect,
                KeyboardType = UIKeyboardType.NumberPad,
                ClearButtonMode = UITextFieldViewMode.WhileEditing,
                Delegate = new PriceTextDelegate(),
                AccessibilityIdentifier = "MaxPriceField"
            };
            AddSubview (MaxPriceField);
        }

        public override void LayoutSubviews()
        {
            var bounds = Bounds;

            HeaderLabel.Frame = new CGRect(
                20, 
                0, 
                bounds.Width * 0.25f, 
                bounds.Height
            );

            MinPriceField.Frame = new CGRect(
                (bounds.Width * 0.25f) + 20,
                bounds.Height * 0.1f,
                bounds.Width * 0.3f,
                bounds.Height * 0.8f
            );

            ToField.Frame = new CGRect(
                (bounds.Width * 0.55f) + 20,
                0,
                20,
                bounds.Height
            );

            MaxPriceField.Frame = new CGRect(
                (bounds.Width * 0.55f) + 40,
                bounds.Height * 0.1f,
                bounds.Width * 0.3f,
                bounds.Height * 0.8f
            );

            NSNotificationCenter.DefaultCenter.AddObserver (UITextField.TextFieldTextDidChangeNotification, (notification) =>
                {
                    if (this.NumChanged != null)
                        this.NumChanged(this, new EventArgs());
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

