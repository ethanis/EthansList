using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace EthansList.MaterialDroid
{
    public class SingleStringDialogFragment : DialogFragment
    {
        readonly Context _context;
        readonly string _title;
        readonly List<KeyValuePair<object,object>> _options;
        ViewGroup.LayoutParams rowParams, textParams;

        public SingleStringDialogFragment(Context context, string title, List<KeyValuePair<object,object>> options)
        {
            _context = context;
            _title = title;
            _options = options;
            rowParams = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ConvertDpToPx(40));
            textParams = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var p = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            var view = new LinearLayout(_context);
            view.LayoutParameters = p;

            var optionHolder = new LinearLayout(_context);
            optionHolder.Orientation = Orientation.Vertical;
            optionHolder.LayoutParameters = p;

            foreach (var option in _options)
            {
                optionHolder.AddView(AddCategoryRow(option));
            }
            var scrollView = new ScrollView(_context);
            scrollView.LayoutParameters = p;
            scrollView.AddView(optionHolder);
            view.AddView(scrollView);

            var dialog = new Android.Support.V7.App.AlertDialog.Builder(_context);
            dialog.SetTitle(_title);
            dialog.SetView(view);
            //dialog.SetNegativeButton("Cancel", (s, a) => { });
            dialog.SetPositiveButton("Ok", (s, a) => { });
            return dialog.Create();
        }

        View AddCategoryRow(KeyValuePair<object, object> option)
        {
            var row = new LinearLayout(_context);
            row.Orientation = Orientation.Horizontal;
            row.LayoutParameters = rowParams;

            var text = new TextView(_context);
            text.Text = (string)option.Key;
            text.SetPadding(ConvertDpToPx(20),0,0,0);
            text.LayoutParameters = textParams;
            text.Gravity = GravityFlags.CenterVertical;

            row.AddView(text);
            return row;
        }

        private int ConvertDpToPx(float dip)
        {
            return (int)(dip * _context.Resources.DisplayMetrics.Density);
        }
    }
}

