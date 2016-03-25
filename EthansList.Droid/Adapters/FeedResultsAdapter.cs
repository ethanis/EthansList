using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using EthansList.Models;
using Java.Lang;

namespace EthansList.Droid
{
    class FeedResultsAdapter : BaseAdapter<Posting>
    {
        Activity activity;
        public ObservableCollection<Posting> Postings { get; set; }
        bool _deleteable;

        public event EventHandler<ImageCollectionClickEventArgs> ItemDeleted;

        public FeedResultsAdapter(Activity activity, ObservableCollection<Posting> postings, bool deleteable = false)
        {
            this.activity = activity;
            Postings = postings;
            _deleteable = deleteable;
        }

        public override Posting this[int position]
        {
            get
            {
                return Postings[position];
            }
        }

        public override int Count
        {
            get
            {
                return Postings.Count;
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
                view = new FeedResultRow(activity, _deleteable);

                view.DeleteCalled += (sender, e) =>
                {
                    if (ItemDeleted != null)
                        ItemDeleted(sender, new ImageCollectionClickEventArgs { Index = position });
                };

            }

            string imageLink = Postings[position].ImageLink;
            if (imageLink != "-1")
            {
                Koush.UrlImageViewHelper.SetUrlDrawable(view._postingImage, imageLink, Resource.Drawable.placeholder);
            }
            else
            {
                view._postingImage.SetImageResource(Resource.Drawable.placeholder);
            }

            view._postingTitle.Text = Postings[position].PostTitle;
            view._postingDescription.Text = Postings[position].Description;

            return view;
        }
    }

    public class FeedResultRow : LinearLayout
    {
        readonly Context _context;
        bool _deleteable;

        public ImageView _postingImage { get; set; }
        public TextView _postingTitle { get; set; }
        public TextView _postingDescription { get; set; }

        public FeedResultRow(Context context, bool deleteable)
            : base(context)
        {
            _context = context;
            _deleteable = deleteable;

            Initialize();
        }

        void Initialize()
        {
            Orientation = Orientation.Horizontal;
            WeightSum = 1;

            var imageParams = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

            var imageHolder = new LinearLayout(_context);
            imageHolder.LayoutParameters = new LayoutParams(0, LayoutParams.MatchParent, 0.30f);

            _postingImage = new ImageView(_context);
            _postingImage.LayoutParameters = imageParams;
            imageHolder.AddView(_postingImage);
            AddView(imageHolder);

            var spacingHolder = new LinearLayout(_context);
            spacingHolder.LayoutParameters = new LayoutParams(0, LayoutParams.MatchParent, 0.01f);
            AddView(spacingHolder);

            var titleDescriptionHolder = new LinearLayout(_context) { Orientation = Orientation.Vertical };
            titleDescriptionHolder.LayoutParameters = new LayoutParams(0, LayoutParams.WrapContent, 0.69f);

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

            if (_deleteable)
            {
                this.LongClick += (sender, e) =>
                {
                    PopupMenu menu = new PopupMenu(_context, this);
                    menu.Inflate(Resource.Menu.DeleteMenu);
                    menu.Show();

                    menu.MenuItemClick += (se, args) =>
                    {
                        if (DeleteCalled != null)
                            DeleteCalled(this, new ImageCollectionClickEventArgs());
                    };
                };
            }
        }

        public event EventHandler<ImageCollectionClickEventArgs> DeleteCalled;
    }
}