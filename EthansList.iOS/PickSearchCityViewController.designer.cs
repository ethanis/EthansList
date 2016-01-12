// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ethanslist.ios
{
	[Register ("PickSearchCityViewController")]
	partial class PickSearchCityViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIPickerView CityPickerView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField CurrentCityLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField CurrentStateLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ProceedButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton RecentCitiesButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIPickerView StatePickerView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (CityPickerView != null) {
				CityPickerView.Dispose ();
				CityPickerView = null;
			}
			if (CurrentCityLabel != null) {
				CurrentCityLabel.Dispose ();
				CurrentCityLabel = null;
			}
			if (CurrentStateLabel != null) {
				CurrentStateLabel.Dispose ();
				CurrentStateLabel = null;
			}
			if (ProceedButton != null) {
				ProceedButton.Dispose ();
				ProceedButton = null;
			}
			if (RecentCitiesButton != null) {
				RecentCitiesButton.Dispose ();
				RecentCitiesButton = null;
			}
			if (StatePickerView != null) {
				StatePickerView.Dispose ();
				StatePickerView = null;
			}
		}
	}
}
