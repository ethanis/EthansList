using System;

using Foundation;
using UIKit;
using MapKit;

namespace ethanslist.ios
{
    public partial class PostingMapCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("PostingMapCell");
        public static readonly UINib Nib;
        public MKMapView PostingMap {get{ return PostingMapView; }}

        static PostingMapCell()
        {
            Nib = UINib.FromName("PostingMapCell", NSBundle.MainBundle);
        }

        public PostingMapCell(IntPtr handle)
            : base(handle)
        {
        }

        public PostingMapCell()
        {
        }

        public static PostingMapCell Create()
        {
            return (PostingMapCell)Nib.Instantiate(null,null)[0];
        }
    }
}
