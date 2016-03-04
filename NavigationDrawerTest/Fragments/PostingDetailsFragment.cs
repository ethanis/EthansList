using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using EthansList.Models;

namespace EthansList.MaterialDroid
{
    public class PostingDetailsFragment : Fragment
    {
        public Posting Posting { get; set; }

        public override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            var view = new PostingDetailsView(this.Activity);

            view.PostingTitle.Text = Posting.PostTitle;

            if (Posting.ImageLink != "-1")
            {
                view.CurrentImage = Posting.ImageLink;
            }

            view.PostingDescription.Text = Posting.Description;

            return view;
        }
    }

    public class PostingDetailsView : LinearLayout
    {
        readonly Context _context;
        readonly LayoutParams rowParams;
        readonly LayoutParams textRowParams;

        public TextView PostingTitle { get; set; }
        public ImageView PostingImage { get; set; }
        public GridView ImageCollection { get; set; }
        public TextView PostingDescription { get; set; }
        public TextView PostingDate { get; set; }

        //todo: posting map and weblink

        public string CurrentImage
        {
            get { return currentImage; }
            set { 
                Koush.UrlImageViewHelper.SetUrlDrawable(PostingImage, value, Resource.Drawable.placeholder);
                currentImage = value;
            }
        }
        string currentImage;

        public PostingDetailsView(Context context)
            :base (context)
        {
            _context = context;
            rowParams = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            textRowParams = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            Initialize();
        }

        void Initialize()
        {
            Orientation = Orientation.Vertical;
            WeightSum = 1;

            PostingTitle = new TextView(_context) { LayoutParameters = textRowParams};
            PostingTitle.SetTextSize(Android.Util.ComplexUnitType.Dip, 18);
            PostingTitle.SetPadding(10, 10, 10, 10);
            PostingTitle.SetTypeface(Typeface.DefaultBold, TypefaceStyle.Bold);
            AddRowItem(PostingTitle);

            PostingImage = new ImageView(_context) { LayoutParameters = textRowParams};
            AddRowItem(PostingImage);

            PostingDescription = new TextView(_context) { LayoutParameters = textRowParams };
            PostingDescription.SetTextSize(Android.Util.ComplexUnitType.Dip, 14);
            PostingDescription.SetPadding(10, 10, 10, 10);
            PostingDescription.SetTypeface(Typeface.Default, TypefaceStyle.Normal);
            AddRowItem(PostingDescription);
        }

        private void AddRowItem(View item)
        {
            var view = new LinearLayout(_context) { Orientation = Orientation.Horizontal };
            view.LayoutParameters = rowParams;
            view.AddView(item);

            AddView(view);
        }
    }
}

