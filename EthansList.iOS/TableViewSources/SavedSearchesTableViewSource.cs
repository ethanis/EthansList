using System;
using UIKit;
using EthansList.Models;
using System.Collections.Generic;

namespace ethanslist.ios
{
    public class SavedSearchesTableViewSource : UITableViewSource
    {
        UIViewController owner;
        List<Search> savedSearches;
        private const string cellID = "searchCell";

        public SavedSearchesTableViewSource(UIViewController owner, List<Search> savedSearches)
        {
            this.owner = owner;
            this.savedSearches = savedSearches;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return savedSearches.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(cellID);
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, cellID);
            
            //TODO need to write spaceing for these cells to display search query
            cell.DetailTextLabel.Lines = 5;
            cell.TextLabel.Text = savedSearches[indexPath.Row].CityName;
            cell.DetailTextLabel.Text = AppDelegate.databaseConnection.FormatSearchQuery(savedSearches[indexPath.Row]);

            return cell;
        }
    }
}

