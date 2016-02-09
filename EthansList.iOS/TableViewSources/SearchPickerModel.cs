using System;
using UIKit;
using System.Collections.Generic;

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

//        public override UIView GetView(UIPickerView pickerView, nint row, nint component, UIView view)
//        {
//            throw new System.NotImplementedException ();
//        }

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

}

