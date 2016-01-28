using System;
using UIKit;
using EthansList.Shared;
using System.Linq;
using Foundation;

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

        public String SelectedItem {
            get { return locations.States.ElementAt(SelectedIndex); }
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return locations.States.Count;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var title = new UILabel(new CoreGraphics.CGRect(0, 0, tableView.Bounds.Width, 25f));
            title.Text = "State";
            title.TextAlignment = UITextAlignment.Center;
            title.BackgroundColor = ColorScheme.Concrete;

            return title;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return 25f;
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(stateCell);
            if (cell == null) 
                cell = new UITableViewCell(UITableViewCellStyle.Default, stateCell);
            
            cell.TextLabel.Text = locations.States.ElementAt(indexPath.Row);
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
        AvailableLocations locations;
        String state;
        public event EventHandler<EventArgs> ValueChange;
        protected int SelectedIndex = 0;
        const string cityCell = "cityCell";

        public CityTableSource(AvailableLocations locations, string state)
        {
            this.locations = locations;
            this.state = state;
        }

        public Location SelectedCity {
            get { return locations.PotentialLocations.Where(l => l.State == state).ElementAt(SelectedIndex); }
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return locations.PotentialLocations.Where(l => l.State == state).Count();
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(cityCell);
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Default, cityCell);
            
            cell.TextLabel.Text = locations.PotentialLocations.Where(l => l.State == state).ElementAt(indexPath.Row).SiteName;
            cell.BackgroundColor = ColorScheme.Clouds;

            return cell;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var title = new UILabel(new CoreGraphics.CGRect(0, 0, tableView.Bounds.Width, 25f));
            title.Text = "City";
            title.TextAlignment = UITextAlignment.Center;
            title.BackgroundColor = ColorScheme.Concrete;

            return title;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return 25f;
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

