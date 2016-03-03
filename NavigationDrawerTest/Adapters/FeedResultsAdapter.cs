using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using EthansList.Models;
using Java.Lang;

namespace EthansList.MaterialDroid
{
    class FeedResultsAdapter : BaseAdapter<Posting>
    {
        Activity activity;
        List<Posting> postings;

        public FeedResultsAdapter(Activity activity, List<Posting> postings)
        {
            this.activity = activity;
            this.postings = postings;
        }

        public override Posting this[int position]
        {
            get
            {
                return postings[position];
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

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = new FeedResultRow(activity, postings[position]);
            //return new TextView(activity) { Text = postings[position].PostTitle };
            string imageLink = postings[position].ImageLink;
            if (imageLink != "-1")
            {
                Koush.UrlImageViewHelper.SetUrlDrawable(view._postingImage, imageLink, Resource.Drawable.placeholder);
            }
            else
            {
                view._postingImage.SetImageResource(Resource.Drawable.placeholder);
            }

            return view;
        }
    }

    public class FeedResultRow : LinearLayout
    {
        readonly Context _context;
        readonly Posting _posting;

        public ImageView _postingImage { get; set; }
        public TextView _postingTitle { get; set; }
        public TextView _postingDescription { get; set; }

        public FeedResultRow(Context context, Posting posting)
            :base(context)
        {
            _context = context;
            _posting = posting;
            Initialize();
        }

        void Initialize()
        {
            Orientation = Orientation.Horizontal;
            WeightSum = 1;

            var imageParams = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

            var imageHolder = new LinearLayout(_context);
            //imageHolder.LayoutParameters = new LayoutParams(0, ConvertDpToPx(100), 0.30f);
            imageHolder.LayoutParameters = new LayoutParams(0, LayoutParams.WrapContent, 0.30f);

            _postingImage = new ImageView(_context);
            _postingImage.LayoutParameters = imageParams;
            imageHolder.AddView(_postingImage);
            imageHolder.SetBackgroundColor(Android.Graphics.Color.Blue);

            AddView(imageHolder);
        }

        private int ConvertDpToPx(float dip)
        {
            return (int)(dip * _context.Resources.DisplayMetrics.Density);
        }
    }
}