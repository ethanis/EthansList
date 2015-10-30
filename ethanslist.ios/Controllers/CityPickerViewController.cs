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

		public CityPickerViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            locations = new AvailableLocations();
            state = locations.States.ElementAt((int)StatePickerView.SelectedRowInComponent(0));

            CityPickerView.Model = new LocationPickerModel(locations, state);

            stateModel = new StatePickerModel(locations);
            StatePickerView.Model = stateModel;

            ProceedButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                var storyboard = UIStoryboard.FromName("Main", null);
                var searchViewController = (SearchViewController)storyboard.InstantiateViewController("SearchViewController");

                searchViewController.Url = locations.PotentialLocations[(int)CityPickerView.SelectedRowInComponent(0)].Url;

                this.ShowViewController(searchViewController, this);
            };

            stateModel.ValueChanged += (object sender, EventArgs e) =>
            {
                CityPickerView.Model = new LocationPickerModel(locations, 
                    stateModel.SelectedItem);
            };
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
                Console.WriteLine(locations.States.ElementAt((int)row));

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
            String state;

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
        }
	}
}