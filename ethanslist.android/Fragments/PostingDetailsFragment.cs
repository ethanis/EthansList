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
using EthansList.Shared;

namespace ethanslist.android
{
    public class PostingDetailsFragment : Fragment
    {
        TextView postingTitle;
        TextView postingDetails;
        ImageView postingImageView;
        TextView postingDate;
        Button saveButton;

        public Posting posting { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.PostingDetails, container, false);

            Console.WriteLine(posting.Title);
            postingTitle = view.FindViewById<TextView>(Resource.Id.postingTitleText);
            postingDetails = view.FindViewById<TextView>(Resource.Id.postingDetailsText);
            postingImageView = view.FindViewById<ImageView>(Resource.Id.postingImageView);
            postingDate = view.FindViewById<TextView>(Resource.Id.postingDateText);
            saveButton = view.FindViewById<Button>(Resource.Id.saveListingButton);

            postingTitle.Text = posting.Title;
            postingDetails.Text = posting.Description;
            postingDate.Text = "Listed: " + posting.Date.ToShortDateString() + " at " + posting.Date.ToShortTimeString();
            string imageLink = posting.ImageLink;  

            if (imageLink != "-1")
            {
                Koush.UrlImageViewHelper.SetUrlDrawable(postingImageView, imageLink, Resource.Drawable.placeholder);
            }

            saveButton.Click += SaveButton_Click;;

            return view;
        }

        void SaveButton_Click (object sender, EventArgs e)
        {
            MainActivity.databaseConnection.AddNewListingAsync(posting.Title, posting.Description, posting.Link, posting.ImageLink, posting.Date);
            Console.WriteLine(MainActivity.databaseConnection.StatusMessage);
        }

    }
}

