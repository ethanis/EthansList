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
	[Register ("SearchViewController")]
	partial class SearchViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel MaxLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel MaxRentLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISlider MaxRentSlider { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIStepper MinBathCountStep { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel MinBathLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel MinBathTitleLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIStepper MinBedCountStep { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel MinBedLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel MinBedTitleLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel MinLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel MinRentLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISlider MinRentSlider { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton saveSearchButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton SearchButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel searchCLLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField SearchField { get; set; }

		[Action ("SearchCL:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void SearchCL (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (MaxLabel != null) {
				MaxLabel.Dispose ();
				MaxLabel = null;
			}
			if (MaxRentLabel != null) {
				MaxRentLabel.Dispose ();
				MaxRentLabel = null;
			}
			if (MaxRentSlider != null) {
				MaxRentSlider.Dispose ();
				MaxRentSlider = null;
			}
			if (MinBathCountStep != null) {
				MinBathCountStep.Dispose ();
				MinBathCountStep = null;
			}
			if (MinBathLabel != null) {
				MinBathLabel.Dispose ();
				MinBathLabel = null;
			}
			if (MinBathTitleLabel != null) {
				MinBathTitleLabel.Dispose ();
				MinBathTitleLabel = null;
			}
			if (MinBedCountStep != null) {
				MinBedCountStep.Dispose ();
				MinBedCountStep = null;
			}
			if (MinBedLabel != null) {
				MinBedLabel.Dispose ();
				MinBedLabel = null;
			}
			if (MinBedTitleLabel != null) {
				MinBedTitleLabel.Dispose ();
				MinBedTitleLabel = null;
			}
			if (MinLabel != null) {
				MinLabel.Dispose ();
				MinLabel = null;
			}
			if (MinRentLabel != null) {
				MinRentLabel.Dispose ();
				MinRentLabel = null;
			}
			if (MinRentSlider != null) {
				MinRentSlider.Dispose ();
				MinRentSlider = null;
			}
			if (saveSearchButton != null) {
				saveSearchButton.Dispose ();
				saveSearchButton = null;
			}
			if (SearchButton != null) {
				SearchButton.Dispose ();
				SearchButton = null;
			}
			if (searchCLLabel != null) {
				searchCLLabel.Dispose ();
				searchCLLabel = null;
			}
			if (SearchField != null) {
				SearchField.Dispose ();
				SearchField = null;
			}
		}
	}
}
