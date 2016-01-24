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
	[Register ("PostingWebViewController")]
	partial class PostingWebViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIBarButtonItem DoneButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UINavigationBar navBar { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UINavigationItem navBarItem { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView statusBarPlaceholder { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIWebView webView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (DoneButton != null) {
				DoneButton.Dispose ();
				DoneButton = null;
			}
			if (navBar != null) {
				navBar.Dispose ();
				navBar = null;
			}
			if (navBarItem != null) {
				navBarItem.Dispose ();
				navBarItem = null;
			}
			if (statusBarPlaceholder != null) {
				statusBarPlaceholder.Dispose ();
				statusBarPlaceholder = null;
			}
			if (webView != null) {
				webView.Dispose ();
				webView = null;
			}
		}
	}
}
