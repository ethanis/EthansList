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
   
    public class ComboPickerDelegate : NSObject, IUIPickerViewDelegate
    {
        private const string cellID = "cellID";
        public List<PickerOptions> values;
        public event EventHandler<PickerChangedEventArgs> PickerChanged;

        public ComboPickerDelegate(List<PickerOptions> values)
        {
            this.values = values;
        }

        [Foundation.Export("pickerView:didSelectRow:inComponent:")]
        public void Selected(UIKit.UIPickerView pickerView, System.nint row, System.nint component)
        {
            if (this.PickerChanged != null)
            {
                this.PickerChanged(this, new PickerChangedEventArgs
                    {SelectedValue = values[(int)component].PickerWheelOptions[(int)row].Value, 
                        SelectedKey = values[(int)component].PickerWheelOptions[(int)row].Key, 
                        FromComponent = (int)component
                    });
            }

            UITableViewCell cell = (UITableViewCell)pickerView.ViewFor(row, component);
            cell.Accessory = UITableViewCellAccessory.Checkmark;
            cell.SelectionStyle = UITableViewCellSelectionStyle.Gray;
        }

        [Foundation.Export("pickerView:viewForRow:forComponent:reusingView:")]
        public UIKit.UIView GetView(UIKit.UIPickerView pickerView, System.nint row, System.nint component, UIKit.UIView view)
        {
            UITableViewCell cell = (UITableViewCell)view;
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellID);
            }

            cell.TextLabel.Text = values[(int)component].PickerWheelOptions[(int)row].Key.ToString();

            cell.Tag = row;

            return cell;
        }

        #region IDisposable implementation
        
        new public void Dispose()
        {
            this.Dispose();
        }
        
        #endregion
        #region INativeObject implementation
        
        new public IntPtr Handle
        {
            get
            {
                return IntPtr.Zero;
            }
        }
        
        #endregion
    }

    public class ComboPickerDataSource : UIPickerViewDataSource
    {
        private const string cellID = "cellID";
        public List<PickerOptions> values;

        public ComboPickerDataSource(List<PickerOptions> values)
        {
            this.values = values;   
        }

        #region implemented abstract members of UIPickerViewDataSource
        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
        }
        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return values[0].PickerWheelOptions.Count;
        }
        #endregion
        
    }
}

