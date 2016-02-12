using System;
using UIKit;
using Foundation;

namespace ethanslist.ios
{
    public class CustomTableViewCell : UITableViewCell
    {
        private readonly NSString cellId;


        public CustomTableViewCell (string cellId)
        {
            this.cellId = new NSString(cellId);
        }

        public override NSString ReuseIdentifier
        {
            get { return cellId; }
        }
    }
}

