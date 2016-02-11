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
	[Register ("DoubleInputCell")]
	partial class DoubleInputCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField MaxLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField MinLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel TitleLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (MaxLabel != null) {
				MaxLabel.Dispose ();
				MaxLabel = null;
			}
			if (MinLabel != null) {
				MinLabel.Dispose ();
				MinLabel = null;
			}
			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}
		}
	}
}
