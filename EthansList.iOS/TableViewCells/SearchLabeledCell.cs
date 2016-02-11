using System;

using Foundation;
using UIKit;

namespace ethanslist.ios
{
    public partial class SearchLabeledCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("SearchLabeledCell");
        public static readonly UINib Nib;
        public UILabel Title {get{ return TitleLabel; }}
        public UITextField TermsField { get{ return TermsTextField; }}

        static SearchLabeledCell()
        {
            Nib = UINib.FromName("SearchLabeledCell", NSBundle.MainBundle);
        }

        public SearchLabeledCell(IntPtr handle)
            : base(handle)
        {
        }

        public static SearchLabeledCell Create()
        {
            return (SearchLabeledCell)Nib.Instantiate(null,null)[0];
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            TitleLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            TermsTextField.TranslatesAutoresizingMaskIntoConstraints = false;

            this.AddConstraints(new NSLayoutConstraint[]{
                NSLayoutConstraint.Create(Title, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Left, 1, 20),
                NSLayoutConstraint.Create(Title, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1, 0),
                NSLayoutConstraint.Create(Title, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1, 0),
                NSLayoutConstraint.Create(Title, NSLayoutAttribute.Right, NSLayoutRelation.Equal, TermsTextField, NSLayoutAttribute.Left, 1, -10),

                NSLayoutConstraint.Create(TermsTextField, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 0.60f, 0),
                NSLayoutConstraint.Create(TermsTextField, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 1, -20),
                NSLayoutConstraint.Create(TermsTextField, NSLayoutAttribute.Height, NSLayoutRelation.Equal, this, NSLayoutAttribute.Height, 0.90f, 0),
                NSLayoutConstraint.Create(TermsTextField, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1, 0),
            });
        }
    }
}
