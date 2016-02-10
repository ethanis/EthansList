using System;
using UIKit;
using System.Collections.Generic;
using System.Drawing;
using Foundation;

namespace ethanslist.ios
{
    public class SearchPickerModel : UIPickerViewModel
    {
        public List<PickerOptions> values;
        private readonly bool price;

        public event EventHandler<PickerChangedEventArgs> PickerChanged;

        public SearchPickerModel(List<PickerOptions> values, bool price)
        {
            this.values = values;
            this.price = price;
        }

        public override nint GetComponentCount (UIPickerView picker)
        {
            return values.Count;
        }

        public override nint GetRowsInComponent (UIPickerView picker, nint component)
        {
            if (price)
                return values[(int)component].Options.Count;
            else
                return values[(int)component].PickerWheelOptions.Count;
        }
            
        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            if (price)
                return values[(int)component].Options[(int)row].ToString ();
            else
                return values[(int)component].PickerWheelOptions[(int)row].Key.ToString ();
        }
        public override void Selected (UIPickerView picker, nint row, nint component)
        {
            if (this.PickerChanged != null)
            {
                if (price)
                    this.PickerChanged(this, new PickerChangedEventArgs{SelectedValue = values[(int)component].Options[(int)row], FromComponent = (int)component});
                else
                    this.PickerChanged(this, new PickerChangedEventArgs
                        {SelectedValue = values[(int)component].PickerWheelOptions[(int)row].Value, 
                            SelectedKey = values[(int)component].PickerWheelOptions[(int)row].Key, 
                            FromComponent = (int)component
                        });
            }
        }
    }

    public class PickerChangedEventArgs : EventArgs{
        public object SelectedValue {get;set;}
        public object SelectedKey { get; set; }
        public int FromComponent {get;set;}
    }

    public class ComboPickerTableSource : UITableViewSource
    {
        private List<PickerOptions> values;
        private const string cellID = "cellID";
        public event EventHandler<PickerChangedEventArgs> ValueChanged;

        public ComboPickerTableSource(List<PickerOptions> values)
        {
            this.values = values;
        }
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(cellID);
            var item = values[0].PickerWheelOptions[indexPath.Row];
            UITapGestureRecognizer tap;
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellID);
                tap = new UITapGestureRecognizer( delegate(UITapGestureRecognizer obj) {
                    if (cell.Accessory == UITableViewCellAccessory.None)
                        cell.Accessory = UITableViewCellAccessory.Checkmark;
                    else
                        cell.Accessory = UITableViewCellAccessory.None;
                    if (this.ValueChanged != null)
                        this.ValueChanged(this, new PickerChangedEventArgs(){SelectedKey = item.Key, SelectedValue = item.Value});
                });
                tap.ShouldRecognizeSimultaneously = (d1, d2) => true;
                cell.AddGestureRecognizer(tap);
            }
            cell.TextLabel.Text = item.Key.ToString();
            cell.BackgroundColor = ColorScheme.Clouds;

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return values[0].PickerWheelOptions.Count;
        }
        
    }
}

