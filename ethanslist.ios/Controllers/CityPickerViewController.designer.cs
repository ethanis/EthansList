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
	[Register ("CityPickerViewController")]
	partial class CityPickerViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIPickerView CityPickerView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ProceedButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton StatePickerButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIPickerView StatePickerView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (CityPickerView != null) {
				CityPickerView.Dispose ();
				CityPickerView = null;
			}
			if (ProceedButton != null) {
				ProceedButton.Dispose ();
				ProceedButton = null;
			}
			if (StatePickerButton != null) {
				StatePickerButton.Dispose ();
				StatePickerButton = null;
			}
			if (StatePickerView != null) {
				StatePickerView.Dispose ();
				StatePickerView = null;
			}
		}
	}
}
