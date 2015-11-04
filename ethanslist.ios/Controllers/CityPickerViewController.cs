using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Linq;
using System.Collections.Generic;

namespace ethanslist.ios
{
	partial class CityPickerViewController : UIViewController
	{
        AvailableLocations locations;
        String state;
        StatePickerModel stateModel;
        LocationPickerModel cityModel;
        Location currentSelected;

		public CityPickerViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            locations = new AvailableLocations();
            state = locations.States.ElementAt((int)StatePickerView.SelectedRowInComponent(0));
            currentSelected = locations.PotentialLocations.Where(loc => loc.State == state).ElementAt(0);

            cityModel = new LocationPickerModel(locations, state);
            CityPickerView.Model = cityModel;

            stateModel = new StatePickerModel(locations);
            StatePickerView.Model = stateModel;

            cityModel.ValueChange += cityPickerChanged;

            //TODO: Have this initialized to the correct first selection
            ProceedButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                var storyboard = UIStoryboard.FromName("Main", null);
                var searchViewController = (SearchViewController)storyboard.InstantiateViewController("SearchViewController");

                Console.WriteLine(currentSelected.SiteName);
                searchViewController.Url = currentSelected.Url;

                this.ShowViewController(searchViewController, this);
            };

            stateModel.ValueChanged += (object sender, EventArgs e) =>
            {
                state = stateModel.SelectedItem;
                CityPickerView.Select(0,0,false);

                currentSelected = locations.PotentialLocations.Where(loc => loc.State == state).ElementAt(0);
                cityModel = new LocationPickerModel(locations, stateModel.SelectedItem);
                CityPickerView.Model = cityModel;
                
                cityModel.ValueChange += cityPickerChanged;
            };
        }

        void cityPickerChanged (object sender, EventArgs e)
        {
            currentSelected = cityModel.SelectedCity;
        }

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
}