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
        List<Listing> savedListings;

		public SavedListingsTableViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Console.WriteLine(AppDelegate.listingRepository.GetAllListingsAsync().Result.Count);

            savedListings = AppDelegate.listingRepository.GetAllListingsAsync().Result;
            tableSource = new SavedListingsTableViewSource(this, savedListings);
            TableView.Source = tableSource;

            TableDelegate tableDelegate = new TableDelegate(this, savedListings);
            TableView.Delegate = tableDelegate;

            tableDelegate.ItemDeleted += OnItemDeleted;
        }

        void OnItemDeleted(object sender, EventArgs e)
        {
            savedListings = AppDelegate.listingRepository.GetAllListingsAsync().Result;
            tableSource = new SavedListingsTableViewSource(this, savedListings);
            TableView.Source = tableSource;
            TableDelegate tableDelegate = new TableDelegate(this, savedListings);
            TableView.Delegate = tableDelegate;
            tableDelegate.ItemDeleted += OnItemDeleted;
        }

        public class TableDelegate : UITableViewDelegate
        {
            List<Listing> savedListings;
            UIViewController owner;
            public event EventHandler<EventArgs> ItemDeleted;

            #region Constructors
            public TableDelegate (UIViewController owner, List<Listing> savedListings)
            {
                this.owner = owner;
                this.savedListings = savedListings;
            }

            public TableDelegate (IntPtr handle) : base (handle)
            {
            }

            public TableDelegate (NSObjectFlag t) : base (t)
            {
            }

            #endregion

            #region Override Methods
            public override UITableViewRowAction[] EditActionsForRow (UITableView tableView, NSIndexPath indexPath)
            {
                UITableViewRowAction deleteButton = UITableViewRowAction.Create (
                    UITableViewRowActionStyle.Destructive,
                    "Delete",
                    delegate {
                    AppDelegate.listingRepository.DeleteListingAsync(savedListings[indexPath.Row]);
                    Console.WriteLine(AppDelegate.listingRepository.StatusMessage);
                    if (this.ItemDeleted != null)
                        this.ItemDeleted(this, new EventArgs());
                });
                return new UITableViewRowAction[] { deleteButton };
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                var storyboard = UIStoryboard.FromName("Main", null);
                var detailController = (SavedListingDetailsViewController)storyboard.InstantiateViewController("SavedListingDetailsViewController");
                detailController.Listing = savedListings[indexPath.Row];
                Console.WriteLine("Hello");
                owner.PresentModalViewController(detailController, true);
            }
            #endregion
        }
	}
}
