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
        private const string cellID = "searchCellID";
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
            SearchViewCell cell = (SearchViewCell)tableView.DequeueReusableCell (SearchViewCell.Key);
            if (cell == null)
                cell = SearchViewCell.Create();

            cell.Model = savedSearches[indexPath.Row];

            return cell;
        }

//        public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, Foundation.NSIndexPath indexPath)
//        {
//            UITableViewRowAction deleteButton = UITableViewRowAction.Create (
//                UITableViewRowActionStyle.Destructive,
//                "Delete",
//                async delegate {
//                await AppDelegate.databaseConnection.DeleteSearchAsync(savedSearches[indexPath.Row]);
//                Console.WriteLine(AppDelegate.databaseConnection.StatusMessage);
//                if (this.ItemDeleted != null)
//                    this.ItemDeleted(this, new EventArgs());
//            });
//            return new UITableViewRowAction[] { deleteButton };
//        }

        public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath)
        {
            switch (editingStyle) {
                case UITableViewCellEditingStyle.Delete:
                    AppDelegate.databaseConnection.DeleteSearchAsync(savedSearches[indexPath.Row]);
                    savedSearches.RemoveAt(indexPath.Row);
                    tableView.DeleteRows(new [] { indexPath }, UITableViewRowAnimation.Fade);
                    break;
                case UITableViewCellEditingStyle.None:
                    Console.WriteLine ("CommitEditingStyle:None called");
                    break;
            }
        }
        public override bool CanEditRow (UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            return true;
        }
        public override string TitleForDeleteConfirmation (UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            return "Delete";
        }

        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var storyboard = UIStoryboard.FromName("Main", null);
            var feedResultsVC = (FeedResultsTableViewController)storyboard.InstantiateViewController("FeedResultsTableViewController");
            feedResultsVC.Query = GenerateQuery(savedSearches[indexPath.Row]);

            owner.ShowViewController(feedResultsVC, owner);
        }

        public string GenerateQuery(Search search)
        {
            return String.Format("{0}/search/apa?format=rss&min_price={1}&max_price={2}&bedrooms={3}&bathrooms{4}&query={5}", 
                search.LinkUrl, search.MinPrice, search.MaxPrice, search.MinBedrooms, search.MinBathrooms, search.SearchQuery);
        }
    }
}

