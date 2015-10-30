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
        String state = "Michigan";

		public CityPickerViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            locations = new AvailableLocations();

            CityPickerView.Model = new LocationPickerModel(locations, state);
            StatePickerView.Model = new StatePickerModel(locations);

            ProceedButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                var storyboard = UIStoryboard.FromName("Main", null);
                var searchViewController = (SearchViewController)storyboard.InstantiateViewController("SearchViewController");

                searchViewController.Url = locations.PotentialLocations[(int)CityPickerView.SelectedRowInComponent(0)].Url;

                this.ShowViewController(searchViewController, this);
            };
        }

        public class StatePickerModel : UIPickerViewModel
        {
            AvailableLocations locations;

            public StatePickerModel(AvailableLocations locations)
            {
                this.locations = locations;
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
        }

        public class LocationPickerModel : UIPickerViewModel
        {
            AvailableLocations locations;
            String state;
            List<Location> sites;

            public LocationPickerModel(AvailableLocations locations, string state)
            {
                this.locations = locations;
                this.state = state;
//                this.sites = locations.PotentialLocations.Where(loc => loc.State == state).ToList();
            }

            public override nint GetComponentCount(UIPickerView pickerView)
            {
                return 1;
            }

            public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
            {

                return locations.PotentialLocations.Where(loc => loc.State == state).Count();
//                return sites.Count;
//                return locations.PotentialLocations.Count;
            }

            public override string GetTitle(UIPickerView pickerView, nint row, nint component)
            {
                return locations.PotentialLocations.Where(loc => loc.State == state).ElementAt((int)row).SiteName;
            }
        }
	}
}
