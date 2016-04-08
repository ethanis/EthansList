using System;
using UIKit;
using EthansList.Shared;
using System.Linq;
using Foundation;
using System.Collections.Generic;

namespace ethanslist.ios
{
    public class StateTableSource : UITableViewSource
    {
        AvailableLocations locations;
        public event EventHandler<EventArgs> ValueChanged;
        protected int SelectedIndex = 0;
        const string stateCell = "stateCell";

        public StateTableSource(AvailableLocations locations)
        {
            this.locations = locations;
        }

        public String SelectedItem
        {
            get { return locations.States.ElementAt(SelectedIndex); }
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return locations.States.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(stateCell);
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Default, stateCell);

            cell.TextLabel.AttributedText = new NSAttributedString(locations.States.ElementAt(indexPath.Row), Constants.CityPickerCellAttributes);
            cell.BackgroundColor = ColorScheme.Clouds;

            return cell;
        }

        public override nfloat GetHeightForRow(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            return Constants.CityPickerRowHeight;
        }

        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            SelectedIndex = indexPath.Row;
            if (this.ValueChanged != null)
                this.ValueChanged(this, new EventArgs());
        }
    }

    public class CityTableSource : UITableViewSource
    {
        public event EventHandler<EventArgs> ValueChange;
        protected int SelectedIndex = 0;
        const string cityCell = "cityCell";
        List<Location> citiesInState;

        public CityTableSource(AvailableLocations locations, string state)
        {
            citiesInState = locations.PotentialLocations.Where(l => l.State.Equals(state)).OrderBy(l => l.SiteName).ToList();
        }

        public Location SelectedCity
        {
            get { return citiesInState.ElementAt(SelectedIndex); }
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return citiesInState.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(cityCell);
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Default, cityCell);

            cell.TextLabel.AttributedText = new NSAttributedString(citiesInState.ElementAt(indexPath.Row).SiteName, Constants.CityPickerCellAttributes);
            cell.BackgroundColor = ColorScheme.Clouds;

            return cell;
        }

        public override nfloat GetHeightForRow(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            return Constants.CityPickerRowHeight;
        }

        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            SelectedIndex = indexPath.Row;
            if (this.ValueChange != null)
                this.ValueChange(this, new EventArgs());
        }
    }
}

