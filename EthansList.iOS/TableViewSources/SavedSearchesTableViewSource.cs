using System;
using UIKit;
using EthansList.Models;
using System.Collections.Generic;
using EthansList.Shared;

namespace ethanslist.ios
{
    public class SavedSearchesTableViewSource : UITableViewSource
    {
        UIViewController owner;
        List<Search> savedSearches;
        private const string cellID = "searchCellID";
        public event EventHandler<EventArgs> ItemDeleted;
        Dictionary<string, string> searchTerms = new Dictionary<string, string>();

        public SavedSearchesTableViewSource(UIViewController owner, List<Search> savedSearches)
        {
            this.owner = owner;
            this.savedSearches = savedSearches;
            searchTerms.Add("min_price", null);
            searchTerms.Add("max_price", null);
            searchTerms.Add("bedrooms", null);
            searchTerms.Add("bathrooms", null);
            searchTerms.Add("query", null);
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

        public override async void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath)
        {
            switch (editingStyle) {
                case UITableViewCellEditingStyle.Delete:
                    await AppDelegate.databaseConnection.DeleteSearchAsync(savedSearches[indexPath.Row]);
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

            var search = savedSearches[indexPath.Row];
            searchTerms["min_price"] = search.MinPrice;
            searchTerms["max_price"] = search.MaxPrice;
            searchTerms["bedrooms"] = search.MinBedrooms;
            searchTerms["bathrooms"] = search.MinBathrooms;
            searchTerms["query"] = search.SearchQuery;
            QueryGeneration helper = new QueryGeneration();

            feedResultsVC.Query = helper.Generate(search.LinkUrl, searchTerms);

            owner.ShowViewController(feedResultsVC, owner);
        }
    }
}

