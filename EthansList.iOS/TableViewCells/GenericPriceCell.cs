using System;
using UIKit;
using CoreGraphics;
using Foundation;

namespace ethanslist.ios
{
    public class GenericPriceCell : CustomTableViewCell
    {
        internal static readonly string Key = "DoubleCell";
        UILabel ToField;

        public event EventHandler<EventArgs> NumChanged;
        public UILabel HeaderLabel { get; set;}
        public UITextField MinField {get;set;}
        public UITextField MaxField { get; set; }

        public GenericPriceCell()
            :base(Key)
        {
            HeaderLabel = new UILabel() { Text = "Price" };
            AddSubview (HeaderLabel);

            ToField = new UILabel() { Text = "to", TextColor = ColorScheme.Clouds, TextAlignment = UITextAlignment.Center };
            AddSubview(ToField);

            MinField = new UITextField() {
                Placeholder = "min", 
                TextAlignment = UITextAlignment.Center, 
                BorderStyle = UITextBorderStyle.RoundedRect,
                KeyboardType = UIKeyboardType.NumberPad,
                ClearButtonMode = UITextFieldViewMode.WhileEditing,
                AccessibilityIdentifier = "MinField"
            };
            AddSubview (MinField);

            MaxField = new UITextField() {
                Placeholder = "max", 
                TextAlignment = UITextAlignment.Center, 
                BorderStyle = UITextBorderStyle.RoundedRect,
                KeyboardType = UIKeyboardType.NumberPad,
                ClearButtonMode = UITextFieldViewMode.WhileEditing,
                AccessibilityIdentifier = "MaxField"
            };
            AddSubview (MaxField);
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

            MinField.Frame = new CGRect(
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

            MaxField.Frame = new CGRect(
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
}

