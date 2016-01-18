using System;
using UIKit;
using System.Collections.Generic;

namespace ethanslist.ios
{
    public class PickerModel : UIPickerViewModel
    {
        public List<PickerOptions> values;

        public event EventHandler<PickerChangedEventArgs> PickerChanged;

        public PickerModel(List<PickerOptions> values)
        {
            this.values = values;
        }

        public override nint GetComponentCount (UIPickerView picker)
        {
            return values.Count;
        }

        public override nint GetRowsInComponent (UIPickerView picker, nint component)
        {
            return values[(int)component].Options.Count;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            return values[(int)component].Options[(int)row].ToString ();
        }

        public override void Selected (UIPickerView picker, nint row, nint component)
        {
            if (this.PickerChanged != null)
            {
                this.PickerChanged(this, new PickerChangedEventArgs{SelectedValue = values[(int)component].Options[(int)row], FromComponent = (int)component});
            }
        }
    }

    public class PickerChangedEventArgs : EventArgs{
        public object SelectedValue {get;set;}
        public int FromComponent {get;set;}
    }

}

