
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace EthansList.Droid
{
    [Activity(Label = "Posting Details")]			
    public class PostingDetailsActivity : Activity
    {
        TextView postingTitle;
        TextView postingDetails;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PostingDetails);

            postingTitle = FindViewById<TextView>(Resource.Id.postingTitleText);
            postingDetails = FindViewById<TextView>(Resource.Id.postingDetailsText);

            postingTitle.Text = Intent.GetStringExtra("title");
            postingDetails.Text = Intent.GetStringExtra("description");
        }
    }
}

