
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
        Button secondButton;
        Context context;

        public SearchView(Context context)
            : base(context)
        {
            this.context = context;
            Initialize();
        }

        public SearchView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            this.context = context;
            Initialize();
        }

        public SearchView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            this.context = context;
            Initialize();
        }

        void Initialize()
        {
            this.SetBackgroundColor(ColorScheme.Clouds);
            this.Orientation = Orientation.Vertical;
            this.WeightSum = 1;

            proceedButton = new Button(context);
            proceedButton.LayoutParameters = new LayoutParams(0, LayoutParams.WrapContent, 0.5f);
            proceedButton.SetBackgroundColor(ColorScheme.MidnightBlue);
            proceedButton.Text = "Search";
            proceedButton.SetTextColor(ColorScheme.Clouds);

            LinearLayout buttonLayout = RowHolder();
            buttonLayout.AddView(proceedButton);
            AddView(buttonLayout);

            secondButton = new Button(context);
            secondButton.LayoutParameters = new LayoutParams(0, LayoutParams.WrapContent, 0.75f);
            secondButton.SetBackgroundColor(ColorScheme.Alizarin);
            secondButton.Text = "Second";
            secondButton.SetTextColor(ColorScheme.Clouds);

            LinearLayout secondButtonLayout = RowHolder();
            secondButtonLayout.AddView(secondButton);
            AddView(secondButtonLayout);
        }

        private LinearLayout RowHolder()
        {
            LinearLayout layout = new LinearLayout(context);
            layout.Orientation = Orientation.Horizontal;
            layout.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            layout.WeightSum = 1;
            layout.SetGravity(GravityFlags.CenterHorizontal);
            return layout;
        }
    }
}

