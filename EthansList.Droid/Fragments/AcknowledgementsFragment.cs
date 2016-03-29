
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;

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

            return view;
        }
    }

    public class AcknowledgementAdapter : BaseAdapter<string>
    {
        readonly Context _context;

        public AcknowledgementAdapter(Context context)
        {
            _context = context;
        }

        public override string this[int position]
        {
            get
            {
                return "Hello Cardview!";
            }
        }

        public override int Count
        {
            get
            {
                return 2;
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
                view = new AcknowledgementRow(_context);
            }

            return view;
        }
    }

    public class AcknowledgementRow : LinearLayout
    {
        readonly Context _context;

        public AcknowledgementRow(Context context)
            : base(context)
        {
            _context = context;
            Initialize();
        }

        void Initialize()
        {
            CardView card = new CardView(_context);
            card.LayoutParameters = new CardView.LayoutParams(LayoutParams.MatchParent, PixelConverter.DpToPixels(245));
            card.SetForegroundGravity(GravityFlags.CenterHorizontal);

            LinearLayout layoutHolder = new LinearLayout(_context);
            layoutHolder.LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.MatchParent, PixelConverter.DpToPixels(240));
            var padding = PixelConverter.DpToPixels(8);
            layoutHolder.SetPadding(padding, padding, padding, padding);

            TextView textDisplay = new TextView(_context) { Text = "Hello CardView!" };
            textDisplay.Gravity = GravityFlags.Center;
            textDisplay.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);

            layoutHolder.AddView(textDisplay);

            card.AddView(layoutHolder);
            AddView(card);
        }
    }
}