using System;
using Android.Widget;
using Android.App;
using System.Collections.Generic;
using System.Linq;
using EthansList.Shared;
using Android.Graphics;
using System.Net;

using com.refractored.monodroidtoolkit.imageloader;

namespace ethanslist.android
{
    public class PostingListAdapter : BaseAdapter<Posting>
    {
        Activity context;
        List<Posting> postings;

        public PostingListAdapter(Activity context, List<Posting> postings)
        {
            this.context = context;
            this.postings = postings;
        }

        public override Posting this[int index]
        {
            get
            {
                return postings[index];
            }
        }

        public override int Count
        {
            get
            {
                return postings.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            var view = convertView;
            if (convertView == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.FeedResultsRow, parent, false);
                var _title = view.FindViewById<TextView>(Resource.Id.postingTitleText);
                var _description = view.FindViewById<TextView>(Resource.Id.postingDetailsText);
                var _image = view.FindViewById<ImageView>(Resource.Id.photoImageView);

                view.Tag = new PostingListViewHolder { Title = _title, Description = _description, ImageView = _image };
            }

            var holder = (PostingListViewHolder)view.Tag;
            holder.Title.Text = postings[position].Title;
            holder.Description.Text = postings[position].Description;
            string imageLink = postings[position].ImageLink;
            if (imageLink != "-1")
            {
                holder.ImageView.SetImageBitmap(GetImageBitmapFromUrl(imageLink));
            }
            else
            {
                holder.ImageView.SetImageResource(Resource.Drawable.placeholder);
            }

            return view;
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }
    }
}

