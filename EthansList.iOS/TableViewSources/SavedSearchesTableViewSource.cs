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
        public event EventHandler<EventArgs> ItemDeleted;

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

        public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            UITableViewRowAction deleteButton = UITableViewRowAction.Create (
                UITableViewRowActionStyle.Destructive,
                "Delete",
                delegate {
                AppDelegate.databaseConnection.DeleteSearchAsync(savedSearches[indexPath.Row]);
                Console.WriteLine(AppDelegate.databaseConnection.StatusMessage);
                if (this.ItemDeleted != null)
                    this.ItemDeleted(this, new EventArgs());
            });
            return new UITableViewRowAction[] { deleteButton };
        }

        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var storyboard = UIStoryboard.FromName("Main", null);
            var feedResultsVC = (FeedResultsTableViewController)storyboard.InstantiateViewController("FeedResultsTableViewController");
            feedResultsVC.Query = GenerateQuery(savedSearches[indexPath.Row]);
            Console.WriteLine("Hello");
//            owner.PresentModalViewController(feedResultsVC, true);
            owner.ShowViewController(feedResultsVC, owner);
        }

        public string GenerateQuery(Search search)
        {
            return String.Format("{0}/search/apa?format=rss&min_price={1}&max_price={2}&bedrooms={3}&bathrooms{4}&query={5}", 
                search.LinkUrl, search.MinPrice, search.MaxPrice, search.MinBedrooms, search.MinBathrooms, search.SearchQuery);
        }
    }
}

