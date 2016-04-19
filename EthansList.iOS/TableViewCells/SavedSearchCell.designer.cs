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
    [Register ("SavedSearchCell")]
    partial class SavedSearchCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LabelCity { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LabelSearchTerms { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (LabelCity != null) {
                LabelCity.Dispose ();
                LabelCity = null;
            }

            if (LabelSearchTerms != null) {
                LabelSearchTerms.Dispose ();
                LabelSearchTerms = null;
            }
        }
    }
}