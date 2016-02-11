using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using EthansList.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using EthansList.Shared;
using Newtonsoft.Json;

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

        public async override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.Title = "Saved Searches";

            NavigationItem.SetLeftBarButtonItem(
                new UIBarButtonItem(UIImage.FromBundle("menu.png"), UIBarButtonItemStyle.Plain, (s, e) => NavigationController.PopViewController(true)), 
                true);

            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.EstimatedRowHeight = 70;

            savedSearches = AppDelegate.databaseConnection.GetAllSearchesAsync().Result;
            searchTableViewSource = new SavedSearchesTableViewSource(this, await DeserializeSearches(savedSearches));

            TableView.Source = searchTableViewSource;
            TableView.ReloadData();
        }

        private async Task<List<SearchObject>> DeserializeSearches(List<Search> savedSearches)
        {
            List<SearchObject> searchObjects = new List<SearchObject>();
            await Task.Run(() =>
                {
                    foreach (Search search in savedSearches)
                    {
                        searchObjects.Add(JsonConvert.DeserializeObject<SearchObject>(search.SerializedSearch));
                    }
                });

            return searchObjects;
        }
	}
}
