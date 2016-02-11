using System;

using Foundation;
using UIKit;

namespace ethanslist.ios
{
    public partial class DoubleInputCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("DoubleInputCell");
        public static readonly UINib Nib;
        public UITextField MinField {get{ return MinLabel; }}
        public UITextField MaxField {get{ return MaxLabel; }}
        public UILabel HeaderLabel {get{ return TitleLabel; }}
        private UILabel ToField;
        public event EventHandler<EventArgs> NumChanged;

        static DoubleInputCell()
        {
            Nib = UINib.FromName("DoubleInputCell", NSBundle.MainBundle);
        }

        public DoubleInputCell(IntPtr handle)
            : base(handle)
        {
        }

        public static DoubleInputCell Create()
        {
            return (DoubleInputCell)Nib.Instantiate(null,null)[0];
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            ToField = new UILabel();
            ToField.Text = "to";
            ToField.TextAlignment = UITextAlignment.Center;
            ToField.TextColor = ColorScheme.Silver;
            this.AddSubview(ToField);

            MinLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            MaxLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            ToField.TranslatesAutoresizingMaskIntoConstraints = false;

            this.AddConstraints(new NSLayoutConstraint[]{
                NSLayoutConstraint.Create(MinLabel, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1, 0),
                NSLayoutConstraint.Create(MinLabel, NSLayoutAttribute.Height, NSLayoutRelation.Equal, this, NSLayoutAttribute.Height, 0.8f, 0),
                NSLayoutConstraint.Create(MinLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 0.30f, 0),

                NSLayoutConstraint.Create(MaxLabel, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1, 0),
                NSLayoutConstraint.Create(MaxLabel, NSLayoutAttribute.Height, NSLayoutRelation.Equal, this, NSLayoutAttribute.Height, 0.8f, 0),
                NSLayoutConstraint.Create(MaxLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 0.30f, 0),
                NSLayoutConstraint.Create(MaxLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 1, -15),

                NSLayoutConstraint.Create(ToField, NSLayoutAttribute.Right, NSLayoutRelation.Equal, MaxLabel, NSLayoutAttribute.Left, 1, 0),
                NSLayoutConstraint.Create(ToField, NSLayoutAttribute.Left, NSLayoutRelation.Equal, MinLabel, NSLayoutAttribute.Right, 1, 0),
                NSLayoutConstraint.Create(ToField, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, 25),
                NSLayoutConstraint.Create(ToField, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1, 0),
            });

            MaxLabel.AccessibilityIdentifier = "MaxField";
            MinLabel.AccessibilityIdentifier = "MinField";

            MinLabel.ClearButtonMode = UITextFieldViewMode.WhileEditing;
            MaxLabel.ClearButtonMode = UITextFieldViewMode.WhileEditing;

            MinLabel.KeyboardType = UIKeyboardType.NumberPad;
            MaxLabel.KeyboardType = UIKeyboardType.NumberPad;

            NSNotificationCenter.DefaultCenter.AddObserver (UITextField.TextFieldTextDidChangeNotification, (notification) =>
                {
                    if (this.NumChanged != null)
                        this.NumChanged(this, new EventArgs());
                });
        }
    }
//
//    public class TextDelegate : UITextFieldDelegate
//    {
//        public override bool ShouldChangeCharacters(UITextField textField, NSRange range, string replacementString)
//        {
//            return true;
//        }
//
//        public override void DidChange(NSKeyValueChange changeKind, NSIndexSet indexes, NSString forKey)
//        {
//            base.DidChange(changeKind, indexes, forKey);
//        }
//
//        public override void EditingStarted(UITextField textField)
//        {
//        }
//    }
}
