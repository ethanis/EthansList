using System;
using UIKit;
using EthansList.Models;
using System.Collections.Generic;
using EthansList.Shared;
using System.Drawing;
using Foundation;
using CoreGraphics;

namespace ethanslist.ios
{
    public class SavedSearchesTableViewSource : UITableViewSource
    {
        UIViewController owner;
        List<SearchObject> savedSearches;
        Dictionary<string, string> searchTerms = new Dictionary<string, string>();

        public SavedSearchesTableViewSource(UIViewController owner, List<SearchObject> savedSearches)
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
            SavedSearchCell cell = (SavedSearchCell)tableView.DequeueReusableCell("searchCell");
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Default, "searchCell") as SavedSearchCell;
            cell.SetCity(savedSearches[indexPath.Row].SearchLocation.SiteName, (string)savedSearches[indexPath.Row].Category.Value);
            cell.SetTerms(AppDelegate.databaseConnection.SecondFormatSearch(savedSearches[indexPath.Row]));

            return cell;
        }

        public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath)
        {
            var deletion = UITableViewRowAction.Create(UITableViewRowActionStyle.Destructive, "Delete", async delegate {
                await AppDelegate.databaseConnection.DeleteSearchAsync(savedSearches[indexPath.Row].SearchLocation.Url, savedSearches[indexPath.Row]);
                if (AppDelegate.databaseConnection.StatusCode == codes.ok)
                {
                    savedSearches.RemoveAt(indexPath.Row);
                    tableView.DeleteRows(new [] { indexPath }, UITableViewRowAnimation.Fade);
                }
                else
                {
                    DidEndEditing(tableView, indexPath);
                }
                Console.WriteLine (AppDelegate.databaseConnection.StatusMessage);
            });
//            var bg = UIImage.FromBundle("DeletePatternImage.png");
//            bg.Scale(new CGSize(80f, tableView.RectForRowAtIndexPath(indexPath).Height));
//            deletion.BackgroundColor = UIColor.FromPatternImage(bg);
            deletion.BackgroundColor = ColorScheme.Alizarin;

            return new UITableViewRowAction[]{ deletion };
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
            QueryGeneration helper = new QueryGeneration();
            var query = helper.Generate(savedSearches[indexPath.Row]);
            Console.WriteLine(query);
            feedResultsVC.Query = query;

            feedResultsVC.MaxListings = search.MaxListings;
            feedResultsVC.WeeksOld = search.PostedDate;

            owner.ShowViewController(feedResultsVC, owner);
        }
    }
}

