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
		UIButton ProceedButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton RecentCitiesButton { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (ProceedButton != null) {
				ProceedButton.Dispose ();
				ProceedButton = null;
			}
			if (RecentCitiesButton != null) {
				RecentCitiesButton.Dispose ();
				RecentCitiesButton = null;
			}
		}
	}
}
