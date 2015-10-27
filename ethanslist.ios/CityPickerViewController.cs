using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ethanslist.ios
{
	partial class CityPickerViewController : UIViewController
	{
        AvailableLocations locations;

		public CityPickerViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            locations = new AvailableLocations();

            CityPickerView.Model = new LocationPickerModel(locations);



            for (int i = 0; i < locations.PotentialLocations.Count; i++)
            {
                Console.WriteLine(locations.PotentialLocations[i].Url);
            }

            ProceedButton.TouchUpInside += (object sender, EventArgs e) => {
                var storyboard = UIStoryboard.FromName("Main", null);
                var searchViewController = (SearchViewController)storyboard.InstantiateViewController("SearchViewController");

                searchViewController.Url = locations.PotentialLocations[(int)CityPickerView.SelectedRowInComponent(0)].Url;

                this.ShowViewController(searchViewController, this);
            };

        }



        public class LocationPickerModel : UIPickerViewModel
        {
            AvailableLocations locations;

            public LocationPickerModel(AvailableLocations locations)
            {
                this.locations = locations;
            }

            public override nint GetComponentCount(UIPickerView pickerView)
            {
                return 1;
            }

            public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
            {
                return locations.PotentialLocations.Count;
            }

            public override string GetTitle(UIPickerView pickerView, nint row, nint component)
            {
                
                return locations.PotentialLocations[(int)row].SiteName;
            }
        }
	}
}
