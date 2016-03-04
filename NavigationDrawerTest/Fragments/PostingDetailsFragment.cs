using System;
using Android.App;
using Android.Content;
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
            PostingTitle.SetTypeface(Android.Graphics.Typeface.DefaultBold, Android.Graphics.TypefaceStyle.Bold);
            AddRowItem(PostingTitle);

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

