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
//            ContentView.Frame = new CGRect(ContentView.Frame.X, ContentView.Frame.Y, ContentView.Bounds.Width, ContentView.Bounds.Height);
            base.LayoutSubviews();

            this.cityLabel.Text = Model.CityName;
            this.searchTermsLabel.Text = AppDelegate.databaseConnection.FormatSearchQuery(Model);

//            cityLabel.Frame = new CGRect (5, 4, ContentView.Bounds.Width, ContentView.Bounds.Width);
//            searchTermsLabel.Frame = new CGRect (100, 18, ContentView, ContentView.Bounds.Height);
//
//            cityLabel.Frame = new CGRect(ContentView.Frame.X, ContentView.Frame.Y, ContentView.Bounds.Width, ContentView.Bounds.Height);
//            searchTermsLabel.Frame = new CGRect(ContentView.Frame.X, ContentView.Frame.Y, ContentView.Bounds.Width, ContentView.Bounds.Height);
//            this.Frame = new CGRect(
        }
    }
}
