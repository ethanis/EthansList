using System;

using Foundation;
using UIKit;

namespace ethanslist.ios
{
    public class SearchTermsCell : CustomTableViewCell
    {
        internal static readonly string Key = "SearchTermsCell";
        public UITextField TermsField { get; set;}

        public SearchTermsCell()
            :base(Key)
        {
            TermsField = new UITextField() { BorderStyle = UITextBorderStyle.RoundedRect};
            AddSubview(TermsField);

            this.TermsField.EditingDidBegin += delegate { this.TermsField.BecomeFirstResponder(); };
            this.TermsField.EditingDidEnd += delegate
                {
                    this.TermsField.ResignFirstResponder();
                };

            this.TermsField.ShouldReturn += delegate {
                TermsField.ResignFirstResponder();
                return true;
            };
        }

        public override void LayoutSubviews()
        {
            var bounds = Bounds;

            TermsField.Frame = new CoreGraphics.CGRect(
                15,
                bounds.Height * 0.1f,
                bounds.Width - 30,
                bounds.Height * 0.8f
            );
        }
    }
}
