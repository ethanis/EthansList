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
	[Register ("SearchOptionsViewController")]
	partial class SearchOptionsViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView SearchTableView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (SearchTableView != null) {
				SearchTableView.Dispose ();
				SearchTableView = null;
			}
		}
	}
}
