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
	[Register ("FeedResultsCell")]
	partial class FeedResultsCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel DescriptionLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView PostingImageView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel TitleLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (DescriptionLabel != null) {
				DescriptionLabel.Dispose ();
				DescriptionLabel = null;
			}
			if (PostingImageView != null) {
				PostingImageView.Dispose ();
				PostingImageView = null;
			}
			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}
		}
	}
}
