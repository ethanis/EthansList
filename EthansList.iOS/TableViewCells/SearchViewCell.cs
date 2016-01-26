using System;

using Foundation;
using UIKit;
using EthansList.Models;
using Mono;
using CoreGraphics;

namespace ethanslist.ios
{
    public partial class SearchViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("SearchViewCell");
        public static readonly UINib Nib;

        public Search Model { get; set;}
        public int Row { get; set; }
        public UILabel CityLabel { get { return cityLabel; }}
        public UILabel SearchTermsLabel { get { return searchTermsLabel; }}

        static SearchViewCell()
        {
            Nib = UINib.FromName("SearchViewCell", NSBundle.MainBundle);
        }

        public SearchViewCell(IntPtr handle)
            : base(handle)
        {
        }

        public SearchViewCell() : base()
        {
        }

        public static SearchViewCell Create()
        {
            return (SearchViewCell)Nib.Instantiate(null, null)[0];
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            this.cityLabel.Text = Model.CityName;
            this.searchTermsLabel.Text = AppDelegate.databaseConnection.SecondFormatSearch(Model);
        }
    }
}
