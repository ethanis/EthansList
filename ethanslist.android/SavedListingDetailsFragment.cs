
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

        public Listing listing { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.PostingDetails, container, false);

            Console.WriteLine(listing.Title);
            postingTitle = view.FindViewById<TextView>(Resource.Id.postingTitleText);
            postingDetails = view.FindViewById<TextView>(Resource.Id.postingDetailsText);
            postingImageView = view.FindViewById<ImageView>(Resource.Id.postingImageView);
            postingDate = view.FindViewById<TextView>(Resource.Id.postingDateText);
            deleteButton = view.FindViewById<Button>(Resource.Id.saveListingButton);
            deleteButton.Text = "Delete Saved Listing";

            postingTitle.Text = listing.Title;
            postingDetails.Text = listing.Description;
            postingDate.Text = "Listed: " + listing.Date.ToShortDateString() + " at " + listing.Date.ToShortTimeString();
            string imageLink = listing.ImageLink;  

            if (imageLink != "-1")
            {
                Koush.UrlImageViewHelper.SetUrlDrawable(postingImageView, imageLink, Resource.Drawable.placeholder);
            }

            deleteButton.Click += DeleteButton_Click;

            return view;
        }

        void DeleteButton_Click (object sender, EventArgs e)
        {
            MainActivity.listingRepository.DeleteListingAsync(this.listing);
            Console.WriteLine(MainActivity.listingRepository.StatusMessage);
            this.FragmentManager.PopBackStack();
        }
    }
}

