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
        SavedPostingsTableViewSource tableSource;
        List<Posting> savedPostings;

		public SavedListingsTableViewController (IntPtr handle) : base (handle)
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

            NavigationItem.SetLeftBarButtonItem(
                new UIBarButtonItem(UIImage.FromBundle("menu.png"), UIBarButtonItemStyle.Plain, (s, e) => NavigationController.PopViewController(true)), 
                true);
            
            this.Title = "Saved Postings";
            savedPostings = AppDelegate.databaseConnection.GetAllPostingsAsync().Result;
            tableSource = new SavedPostingsTableViewSource(this, savedPostings);
            TableView.Source = tableSource;
            TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
        }
	}
}