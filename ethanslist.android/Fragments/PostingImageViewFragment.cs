
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Webkit;
using EthansList.Shared;

namespace ethanslist.android
{
    public class PostingImageViewFragment : Fragment
    {
        public Posting post { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.postingImageView, container, false);

            WebView webView = view.FindViewById<WebView>(Resource.Id.myWebView);
//            webView.Settings.SupportZoom(true;
            webView.Settings.BuiltInZoomControls = true;
            webView.Settings.DisplayZoomControls = true;

            if (post.ImageLink != "-1")
            {
                webView.LoadUrl(post.ImageLink);
            }

//            ImageView image = view.FindViewById<ImageView>(Resource.Id.imageViewMap);
//
//            if (post.ImageLink != "-1")
//            {
//                Koush.UrlImageViewHelper.SetUrlDrawable(image, post.ImageLink, Resource.Drawable.placeholder);
//            }

            return view;
        }
    }
}

