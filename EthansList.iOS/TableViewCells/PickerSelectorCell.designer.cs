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
	[Register ("PickerSelectorCell")]
	partial class PickerSelectorCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel NumberLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField TextField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel TitleLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (NumberLabel != null) {
				NumberLabel.Dispose ();
				NumberLabel = null;
			}
			if (TextField != null) {
				TextField.Dispose ();
				TextField = null;
			}
			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}
		}
	}
}
