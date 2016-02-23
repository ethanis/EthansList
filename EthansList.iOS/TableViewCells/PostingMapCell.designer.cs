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
    [Register ("PostingMapCell")]
    partial class PostingMapCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStepper MapStepper { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MapKit.MKMapView PostingMapView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (MapStepper != null) {
                MapStepper.Dispose ();
                MapStepper = null;
            }

            if (PostingMapView != null) {
                PostingMapView.Dispose ();
                PostingMapView = null;
            }
        }
    }
}