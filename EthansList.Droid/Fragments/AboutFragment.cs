
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

namespace EthansList.Droid
{
    public class AboutFragment : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = new AboutView(this.Activity);

            return view;
        }
    }

    public class AboutView : LinearLayout
    {
        readonly Context _context;

        public AboutView(Context context)
            : base(context)
        {
            _context = context;
            Initialize();
        }

        void Initialize()
        {
            Orientation = Orientation.Vertical;

            var name = new TextView(_context) { Text = "Ethan Dennis" };
            name.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            AddRowItem(name);
        }

        void AddRowItem(View item)
        {
            var row = new LinearLayout(_context);
            row.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);

            row.AddView(item);

            AddView(row);
        }
    }
}

