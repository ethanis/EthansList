﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.Net;

namespace ethanslist.android
{
    [Activity(Label = "Posting Details")]			
    public class PostingDetailsActivity : Activity
    {
        TextView postingTitle;
        TextView postingDetails;
        ImageView postingImageView;
        TextView postingDate;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PostingDetails);

            postingTitle = FindViewById<TextView>(Resource.Id.postingTitleText);
            postingDetails = FindViewById<TextView>(Resource.Id.postingDetailsText);
            postingImageView = FindViewById<ImageView>(Resource.Id.postingImageView);
            postingDate = FindViewById<TextView>(Resource.Id.postingDateText);

            postingTitle.Text = Intent.GetStringExtra("title");
            postingDetails.Text = Intent.GetStringExtra("description");
            postingDate.Text = "Listed: " + Intent.GetStringExtra("date") + " at " + Intent.GetStringExtra("time");
            string imageLink = Intent.GetStringExtra("imageLink");  

            if (imageLink != "-1")
            {
                Koush.UrlImageViewHelper.SetUrlDrawable(postingImageView, imageLink, Resource.Drawable.placeholder);
            }
        }
    }
}

