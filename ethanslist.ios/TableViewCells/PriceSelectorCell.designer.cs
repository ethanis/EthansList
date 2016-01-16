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
	[Register ("PriceSelectorCell")]
	partial class PriceSelectorCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel Heading { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField MaxPriceField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField MinPriceField { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (Heading != null) {
				Heading.Dispose ();
				Heading = null;
			}
			if (MaxPriceField != null) {
				MaxPriceField.Dispose ();
				MaxPriceField = null;
			}
			if (MinPriceField != null) {
				MinPriceField.Dispose ();
				MinPriceField = null;
			}
		}
	}
}
