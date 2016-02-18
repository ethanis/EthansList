
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

namespace ethanslist.android
{
    public class SearchView : LinearLayout
    {
        Button proceedButton;
        public SearchView(Context context)
            : base(context)
        {
            Initialize(context);
        }

        public SearchView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Initialize(context);
        }

        public SearchView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            Initialize(context);
        }

        void Initialize(Context context)
        {
            this.SetBackgroundColor(ColorScheme.Clouds);

            proceedButton = new Button(context);
            proceedButton.LayoutParameters = new ViewGroup.LayoutParams(WindowManagerLayoutParams.MatchParent, 100);
            proceedButton.SetBackgroundColor(ColorScheme.MidnightBlue);
            proceedButton.Text = "Search";
            proceedButton.SetTextColor(ColorScheme.Clouds);
            this.AddView(proceedButton);
        }
    }
}

