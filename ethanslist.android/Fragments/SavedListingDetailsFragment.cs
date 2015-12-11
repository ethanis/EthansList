
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
using EthansList.Models;

namespace ethanslist.android
{
    public class SavedListingDetailsFragment : Fragment
    {
        TextView postingTitle;
        TextView postingDetails;
        ImageView postingImageView;
        TextView postingDate;
        Button deleteButton;

        public Posting posting { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.PostingDetails, container, false);

            Console.WriteLine(posting.PostTitle);
            postingTitle = view.FindViewById<TextView>(Resource.Id.postingTitleText);
            postingDetails = view.FindViewById<TextView>(Resource.Id.postingDetailsText);
            postingImageView = view.FindViewById<ImageView>(Resource.Id.postingImageView);
            postingDate = view.FindViewById<TextView>(Resource.Id.postingDateText);
            deleteButton = view.FindViewById<Button>(Resource.Id.saveListingButton);
            deleteButton.Text = "Delete Saved Listing";

            postingTitle.Text = posting.PostTitle;
            postingDetails.Text = posting.Description;
            postingDate.Text = "Listed: " + posting.Date.ToShortDateString() + " at " + posting.Date.ToShortTimeString();
            string imageLink = posting.ImageLink;  

            if (imageLink != "-1")
            {
                Koush.UrlImageViewHelper.SetUrlDrawable(postingImageView, imageLink, Resource.Drawable.placeholder);
            }

            deleteButton.Click += DeleteButton_Click;

            return view;
        }

        async void DeleteButton_Click (object sender, EventArgs e)
        {
            await MainActivity.databaseConnection.DeleteListingAsync(this.posting);
            Console.WriteLine(MainActivity.databaseConnection.StatusMessage);
            if (MainActivity.databaseConnection.StatusCode == codes.ok)
            {
                Toast.MakeText(this.Activity, "Listing removed successfully", ToastLength.Short).Show();
                this.FragmentManager.PopBackStack();
            }
            else
            {
                Toast.MakeText(this.Activity, "Unable to remove listing, please try again", ToastLength.Short).Show();
            }
        }
    }
}

