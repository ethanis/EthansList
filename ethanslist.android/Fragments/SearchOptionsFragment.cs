using System;
using Android.App;
using Android.Views;
using Android.Graphics;
using Android.Widget;

namespace ethanslist.android
{
    public class SearchOptionsFragment : Fragment
    {
        public override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            var view = new SearchView(this.Activity);
            view.Layout(container.Left, container.Top, container.Right, container.Bottom);

            return view;
        }
    }
}

