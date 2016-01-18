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
	[Register ("SearchTermsCell")]
	partial class SearchTermsCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField SearchTermField { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (SearchTermField != null) {
				SearchTermField.Dispose ();
				SearchTermField = null;
			}
		}
	}
}
