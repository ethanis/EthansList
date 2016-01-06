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
	[Register ("postingImageViewController")]
	partial class postingImageViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView myImageView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIScrollView myScrollView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (myImageView != null) {
				myImageView.Dispose ();
				myImageView = null;
			}
			if (myScrollView != null) {
				myScrollView.Dispose ();
				myScrollView = null;
			}
		}
	}
}
