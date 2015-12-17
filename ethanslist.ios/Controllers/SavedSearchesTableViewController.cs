using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using EthansList.Models;
using System.Collections.Generic;

namespace ethanslist.ios
{
	partial class SavedSearchesTableViewController : UITableViewController
	{
        List<Search> savedSearches;
        SavedSearchesTableViewSource searchTableViewSource;

		public SavedSearchesTableViewController (IntPtr handle) : base (handle)
		{
		}

        public override void LoadView()
        {
            base.LoadView();

            this.View.Layer.BackgroundColor = ColorScheme.Clouds.CGColor;
            this.TableView.BackgroundColor = ColorScheme.Clouds;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            savedSearches = AppDelegate.databaseConnection.GetAllSearchesAsync().Result;
            searchTableViewSource = new SavedSearchesTableViewSource(this, savedSearches);
            TableView.Source = searchTableViewSource;
            TableView.RowHeight = 135;
//            TableView.RowHeight = UITableView.AutomaticDimension;
//            TableView.EstimatedRowHeight = 130;

//            searchTableViewSource.ItemDeleted += SearchTableViewSource_ItemDeleted;
        }

//        void SearchTableViewSource_ItemDeleted (object sender, EventArgs e)
//        {
//            savedSearches = AppDelegate.databaseConnection.GetAllSearchesAsync().Result;
//            searchTableViewSource = new SavedSearchesTableViewSource(this, savedSearches);
//            TableView.Source = searchTableViewSource;
//
//            searchTableViewSource.ItemDeleted += SearchTableViewSource_ItemDeleted;
//        }
	}
}
