using System;
using UIKit;

namespace ethanslist.ios
{
    public class TitledSearchCell : CustomTableViewCell
    {
        internal static readonly string Key = "DoubleCell";
        public UILabel Title { get; set;}
        public UITextField TermsField { get; set;}

        public TitledSearchCell()
            :base(Key)
        {
            Title = new UILabel();
            AddSubview(Title);

            TermsField = new UITextField() {BorderStyle = UITextBorderStyle.RoundedRect};
            AddSubview(TermsField);
        }

        public override void LayoutSubviews()
        {
            var bounds = Bounds;

            Title.Frame = new CoreGraphics.CGRect(
                20, 
                0, 
                bounds.Width * 0.30f, 
                bounds.Height
            );

            TermsField.Frame = new CoreGraphics.CGRect(
                (bounds.Width * 0.30f) + 35,
                bounds.Height * 0.1f,
                (bounds.Width * 0.70f) - 50,
                bounds.Height * 0.8f
            );
        }
    }
}

