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
            var view = (FeedResultRow)convertView;
            if (view == null)
            {
                view = new FeedResultRow(activity);
            }

            string imageLink = postings[position].ImageLink;
            if (imageLink != "-1")
            {
                Koush.UrlImageViewHelper.SetUrlDrawable(view._postingImage, imageLink, Resource.Drawable.placeholder);
            }
            else
            {
                view._postingImage.SetImageResource(Resource.Drawable.placeholder);
            }

            view._postingTitle.Text = postings[position].PostTitle;
            view._postingDescription.Text = postings[position].Description;

            return view;
        }
    }

    public class FeedResultRow : LinearLayout
    {
        readonly Context _context;

        public ImageView _postingImage { get; set; }
        public TextView _postingTitle { get; set; }
        public TextView _postingDescription { get; set; }

        public FeedResultRow(Context context)
            :base(context)
        {
            _context = context;
            Initialize();
        }

        void Initialize()
        {
            Orientation = Orientation.Horizontal;
            WeightSum = 1;

            var imageParams = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

            var imageHolder = new LinearLayout(_context);
            //imageHolder.LayoutParameters = new LayoutParams(0, ConvertDpToPx(100), 0.30f);
            imageHolder.LayoutParameters = new LayoutParams(0, LayoutParams.MatchParent, 0.30f);

            _postingImage = new ImageView(_context);
            _postingImage.LayoutParameters = imageParams;
            imageHolder.AddView(_postingImage);
            AddView(imageHolder);

            var titleDescriptionHolder = new LinearLayout(_context) { Orientation = Orientation.Vertical };
            titleDescriptionHolder.LayoutParameters = new LayoutParams(0, LayoutParams.WrapContent, 0.70f);
            //titleDescriptionHolder.SetBackgroundColor(Android.Graphics.Color.AliceBlue);

            _postingTitle = new TextView(_context);
            _postingTitle.SetTextSize(Android.Util.ComplexUnitType.Dip, 18);
            _postingTitle.SetTypeface(Android.Graphics.Typeface.DefaultBold, Android.Graphics.TypefaceStyle.Bold);
            _postingTitle.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            _postingTitle.SetMaxLines(2);
            titleDescriptionHolder.AddView(_postingTitle);

            _postingDescription = new TextView(_context);
            _postingDescription.SetTextSize(Android.Util.ComplexUnitType.Dip, 16);
            _postingDescription.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            _postingDescription.SetMaxLines(3);
            titleDescriptionHolder.AddView(_postingDescription);

            AddView(titleDescriptionHolder);
        }

        private int ConvertDpToPx(float dip)
        {
            return (int)(dip * _context.Resources.DisplayMetrics.Density);
        }
    }
}