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

        public void SetCity(string city)
        {
            LabelCity.Text = city;
        }

        public void SetTerms(string terms)
        {
            LabelSearchTerms.Text = terms;
        }
	}
}
