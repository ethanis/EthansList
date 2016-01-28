using System;
using EthansList.Shared;
using UIKit;
using System.Linq;

namespace ethanslist.ios
{

    public class StatePickerModel : UIPickerViewModel
    {
        public AvailableLocations locations;
        public event EventHandler<EventArgs> ValueChanged;
        protected int SelectedIndex = 0;

        public StatePickerModel(AvailableLocations locations)
        {
            this.locations = locations;
        }

        public String SelectedItem {
            get { return locations.States.ElementAt(SelectedIndex);}
        }

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
        }

        public override nfloat GetRowHeight(UIPickerView pickerView, nint component)
        {
            return Constants.CityPickerRowHeight;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return locations.States.Count;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            return locations.States.ElementAt((int)row);
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            SelectedIndex = (int)row;
            if (this.ValueChanged != null)
            {
                this.ValueChanged(this, new EventArgs());
            }
        }
    }

    public class LocationPickerModel : UIPickerViewModel
    {
        AvailableLocations locations;
        public event EventHandler<EventArgs> ValueChange;
        protected int SelectedIndex = 0;
        String state;

        public Location SelectedCity 
        {   get { return locations.PotentialLocations.Where(loc => loc.State == state).ElementAt(SelectedIndex); } 
        }

        public LocationPickerModel(AvailableLocations locations, string state)
        {
            this.locations = locations;
            this.state = state;
        }

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
        }

        public override nfloat GetRowHeight(UIPickerView pickerView, nint component)
        {
            return Constants.CityPickerRowHeight;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return locations.PotentialLocations.Where(loc => loc.State == state).Count();
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            return locations.PotentialLocations.Where(loc => loc.State == state).ElementAt((int)row).SiteName;
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            SelectedIndex = (int)row;
            if (this.ValueChange != null)
            {
                this.ValueChange(this, new EventArgs());
            } 
        }
    }
}

