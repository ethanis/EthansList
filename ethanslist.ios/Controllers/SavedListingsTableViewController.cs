using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using EthansList.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Threading;

namespace ethanslist.ios
{
	partial class SavedListingsTableViewController : UITableViewController
	{
        SavedListingsTableViewSource tableSource;
        List<Listing> savedListings;

		public SavedListingsTableViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            savedListings = AppDelegate.databaseConnection.GetAllListingsAsync().Result;
            tableSource = new SavedListingsTableViewSource(this, savedListings);
            TableView.Source = tableSource;

//            PostingDetailsViewController postingDetailsVC = new PostingDetailsViewController();
//            postingDetailsVC.ItemDeleted += (object sender, EventArgs e) => {
//                this.DismissViewController(true, null);
//            };
        }
	}
}
