using System;
using UIKit;
using System.Collections.Generic;
using EthansList.Models;
using EthansList.Shared;
using Foundation;

namespace ethanslist.ios
{
    public class RecentCityTableViewSource : UITableViewSource
    {
        UIViewController owner;
        private List<RecentCity> recentCities;
        const string cellID = "recentCityCell";

        public RecentCityTableViewSource(UIViewController owner, List<RecentCity> recentCities)
        {
            this.owner = owner;
            this.recentCities = recentCities;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return recentCities.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(cellID);
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellID);

            cell.TextLabel.AttributedText = new NSAttributedString(recentCities[indexPath.Row].City, Constants.LabelAttributes);

            return cell;
        }

        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            AvailableLocations allLocations = new AvailableLocations();
            var categoryVC = new CategoryPickerViewController();
            categoryVC.SelectedCity = allLocations.PotentialLocations.Find(x => x.SiteName == recentCities[indexPath.Row].City);

            this.owner.ShowViewController(categoryVC, this);
        }
    }
}

