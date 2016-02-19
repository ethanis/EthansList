using System;
using Android.Widget;
using Android.Content;
using System.Collections.Generic;
using Android.Views;

namespace ethanslist.android
{
    public class SearchOptionsListAdapter : BaseAdapter<SearchRow>
    {
        Context context;
        List<SearchRow> searchOptions;

        public SearchOptionsListAdapter(Context context, List<SearchRow> searchOptions)
        {
            this.context = context;
            this.searchOptions = searchOptions;
        }

        public override SearchRow this[int index]
        {
            get
            {
                return searchOptions[index];
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            var item = searchOptions[position];
            var view = new LinearLayout(context);
            view.Orientation = Orientation.Horizontal;
            LinearLayout.LayoutParams p = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.MatchParent);
            view.WeightSum = 1;

            TextView title = new TextView(context);
            title.LayoutParameters = p;
            title.Text = item.Title;
            title.SetTextColor(ColorScheme.MidnightBlue);
            view.AddView(title);

            switch (item.RowType)
            {
                case SearchRowTypes.Heading:

                    break;
                case SearchRowTypes.SearchTerms:
                    EditText searchfield = new EditText(context);
                    searchfield.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                    searchfield.SetBackgroundColor(ColorScheme.Silver);
                    searchfield.SetTextColor(ColorScheme.MidnightBlue);
                    view.AddView(searchfield);

                    break;
                default:
                    break;
            }
            return view;
        }

        public override int Count
        {
            get
            {
                return searchOptions.Count;
            }
        }
    }
}

