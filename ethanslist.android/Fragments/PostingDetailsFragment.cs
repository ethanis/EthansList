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
        GridView imageGV;
        ListingImageDownloader imageHelper;

        public Posting posting { get; set; }
        string currentImage;
        public string CurrentImage
        {
            get { return currentImage; }
            set { 
                Koush.UrlImageViewHelper.SetUrlDrawable(postingImageView, value, Resource.Drawable.placeholder);
                currentImage = value;
            }
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.PostingDetails, container, false);

            postingTitle = view.FindViewById<TextView>(Resource.Id.postingTitleText);
            postingDetails = view.FindViewById<TextView>(Resource.Id.postingDetailsText);
            postingImageView = view.FindViewById<ImageView>(Resource.Id.postingImageView);
            postingDate = view.FindViewById<TextView>(Resource.Id.postingDateText);
            saveButton = view.FindViewById<Button>(Resource.Id.saveListingButton);
            imageGV = view.FindViewById<GridView>(Resource.Id.imageGridView);

            imageHelper = new ListingImageDownloader(posting.Link);

            imageHelper.loadingComplete += (sender, e) =>
            {
                imageGV.Adapter = new ImageAdapter(this.Activity, imageHelper.images);
            };
            
            saveButton.Enabled = true;
            postingTitle.Text = posting.Title;
            postingDetails.Text = posting.Description;
            postingDate.Text = "Listed: " + posting.Date.ToShortDateString() + " at " + posting.Date.ToShortTimeString();

            string imageLink = posting.ImageLink;  

            if (imageLink != "-1")
            {
                CurrentImage = imageLink;
            }
            postingImageView.Click += PostingImageView_Click;

            saveButton.Click += SaveButton_Click;;

            imageGV.ItemClick += (Object sender, AdapterView.ItemClickEventArgs args) => {
                CurrentImage = imageHelper.images.ElementAt(args.Position);
            };

            return view;
        }

        void PostingImageView_Click (object sender, EventArgs e)
        {
            var intent = new Intent(this.Activity, typeof(ImageZoomActivity));
            intent.PutExtra("imageUrl", currentImage);
            StartActivity(intent);
        }

        async void SaveButton_Click (object sender, EventArgs e)
        {
            await MainActivity.databaseConnection.AddNewListingAsync(posting.Title, posting.Description, posting.Link, posting.ImageLink, posting.Date);
            Console.WriteLine(MainActivity.databaseConnection.StatusMessage);
            if (MainActivity.databaseConnection.StatusCode == EthansList.Models.codes.ok)
            {
                Toast.MakeText(this.Activity, "Listing saved successfully!", ToastLength.Short).Show();
                saveButton.Enabled = false;
            }
            else
            {
                Toast.MakeText(this.Activity, "Unable to save listing. Please try again.", ToastLength.Short).Show();
                saveButton.Enabled = true;
            }
        }

    }
}

