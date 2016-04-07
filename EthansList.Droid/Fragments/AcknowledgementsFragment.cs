
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using EthansList.Models;

namespace EthansList.Droid
{
    public class AcknowledgementsFragment : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = new ListView(this.Activity);
            var adapter = new AcknowledgementAdapter(this.Activity);
            view.Adapter = adapter;
            view.Divider = null;
            view.DividerHeight = 0;

            return view;
        }
    }

    public class AcknowledgementAdapter : BaseAdapter<Acknowledgement>
    {
        readonly Context _context;
        readonly List<Acknowledgement> _acks;

        public AcknowledgementAdapter(Context context)
        {
            _context = context;
            _acks = Acknowledgements.All;
        }

        public override Acknowledgement this[int position]
        {
            get
            {
                return _acks[position];
            }
        }

        public override int Count
        {
            get
            {
                return _acks.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = (AcknowledgementRow)convertView;
            if (view == null)
            {
                view = new AcknowledgementRow(_context, _acks[position]);
                view.Click += (sender, e) =>
                {
                    var transaction = ((MainActivity)_context).SupportFragmentManager.BeginTransaction();
                    WebviewFragment webviewFragment = new WebviewFragment();
                    webviewFragment.Link = _acks[position].Link;

                    transaction.Replace(Resource.Id.frameLayout, webviewFragment);
                    transaction.AddToBackStack(null);
                    transaction.Commit();
                };
            }

            return view;
        }
    }

    public class AcknowledgementRow : LinearLayout
    {
        readonly Context _context;
        readonly Acknowledgement _ack;

        public AcknowledgementRow(Context context, Acknowledgement ack)
            : base(context)
        {
            _context = context;
            _ack = ack;
            Initialize();
        }

        void Initialize()
        {
            CardView card = new CardView(_context);
            card.LayoutParameters = new CardView.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            card.SetForegroundGravity(GravityFlags.CenterHorizontal);
            card.Radius = PixelConverter.DpToPixels(6);
            card.CardElevation = PixelConverter.DpToPixels(4);
            card.SetCardBackgroundColor(Resource.Color.accent);

            LinearLayout layoutHolder = new LinearLayout(_context);
            layoutHolder.Orientation = Orientation.Vertical;
            layoutHolder.LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            var padding = PixelConverter.DpToPixels(8);
            layoutHolder.SetPadding(padding, padding, padding, padding);
            layoutHolder.SetBackgroundResource(Resource.Color.accent);

            TextView titleDisplay = new TextView(_context) { Text = _ack.Title };
            titleDisplay.Gravity = GravityFlags.CenterVertical;
            titleDisplay.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            titleDisplay.SetTextSize(ComplexUnitType.Dip, 20);
            titleDisplay.SetTypeface(Android.Graphics.Typeface.DefaultBold, Android.Graphics.TypefaceStyle.Bold);
            titleDisplay.SetTextColor(Android.Graphics.Color.AntiqueWhite);
            layoutHolder.AddView(titleDisplay);

            TextView descriptionDisplay = new TextView(_context) { Text = _ack.Description };
            descriptionDisplay.Gravity = GravityFlags.CenterVertical;
            descriptionDisplay.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            descriptionDisplay.SetTextSize(ComplexUnitType.Dip, 14);
            descriptionDisplay.SetTypeface(Android.Graphics.Typeface.Default, Android.Graphics.TypefaceStyle.Normal);
            descriptionDisplay.SetTextColor(Android.Graphics.Color.AntiqueWhite);
            layoutHolder.AddView(descriptionDisplay);

            card.AddView(layoutHolder);
            AddView(card);
        }
    }
}