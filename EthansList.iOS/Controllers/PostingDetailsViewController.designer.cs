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
	[Register ("PostingDetailsViewController")]
	partial class PostingDetailsViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel dateLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIBarButtonItem DoneButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UICollectionView imageCollectionView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIScrollView imageScrollView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView imageViewPlaceholder { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UINavigationBar myNavBar { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UINavigationItem myNavBarItem { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView PostingDescription { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView postingImageView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel PostingTitle { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIBarButtonItem SaveButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView statusBarColorPlaceholder { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (dateLabel != null) {
				dateLabel.Dispose ();
				dateLabel = null;
			}
			if (DoneButton != null) {
				DoneButton.Dispose ();
				DoneButton = null;
			}
			if (imageCollectionView != null) {
				imageCollectionView.Dispose ();
				imageCollectionView = null;
			}
			if (imageScrollView != null) {
				imageScrollView.Dispose ();
				imageScrollView = null;
			}
			if (imageViewPlaceholder != null) {
				imageViewPlaceholder.Dispose ();
				imageViewPlaceholder = null;
			}
			if (myNavBar != null) {
				myNavBar.Dispose ();
				myNavBar = null;
			}
			if (myNavBarItem != null) {
				myNavBarItem.Dispose ();
				myNavBarItem = null;
			}
			if (PostingDescription != null) {
				PostingDescription.Dispose ();
				PostingDescription = null;
			}
			if (postingImageView != null) {
				postingImageView.Dispose ();
				postingImageView = null;
			}
			if (PostingTitle != null) {
				PostingTitle.Dispose ();
				PostingTitle = null;
			}
			if (SaveButton != null) {
				SaveButton.Dispose ();
				SaveButton = null;
			}
			if (statusBarColorPlaceholder != null) {
				statusBarColorPlaceholder.Dispose ();
				statusBarColorPlaceholder = null;
			}
		}
	}
}