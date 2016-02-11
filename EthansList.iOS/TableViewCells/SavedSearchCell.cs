using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ethanslist.ios
{
	partial class SavedSearchCell : UITableViewCell
	{
		public SavedSearchCell (IntPtr handle) : base (handle)
		{
            this.BackgroundColor = ColorScheme.Clouds;
		}

        public SavedSearchCell()
        {
            this.BackgroundColor = ColorScheme.Clouds;
        }

        public void SetCity(string city, string cat)
        {
            LabelCity.AttributedText = new NSAttributedString(city + ": " + cat, Constants.HeaderAttributes);
        }

        public void SetTerms(string terms)
        {
            LabelSearchTerms.AttributedText = new NSAttributedString(terms, Constants.CityPickerCellAttributes);
        }
	}
}
