using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Listings.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ethanslist.ios
{
	partial class SavedListingsTableViewController : UITableViewController
	{
        SavedListingsTableViewSource tableSource;

		public SavedListingsTableViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Console.WriteLine(AppDelegate.listingRepository.GetAllListingsAsync().Result.Count);

            tableSource = new SavedListingsTableViewSource(this);
            TableView.Source = tableSource;
        }
	}
}
