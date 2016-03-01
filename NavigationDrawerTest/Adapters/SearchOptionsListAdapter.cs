using System;
using Android.Widget;
using Android.Content;
using System.Collections.Generic;
using Android.Views;

namespace EthansList.MaterialDroid
{
    public class SearchOptionsListAdapter : BaseAdapter<SearchRow>
    {
        readonly Context context;
        List<SearchRow> searchOptions;
        SearchOptionsView owner;

        public SearchOptionsListAdapter(SearchOptionsView owner, Context context, List<SearchRow> searchOptions)
        {
            this.context = context;
            this.searchOptions = searchOptions;
            this.owner = owner;
        }

        public override SearchRow this[int position]
        {
            get
            {
                return searchOptions[position];
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
            var entryHolder = new LinearLayout(context);
            entryHolder.Orientation = Orientation.Horizontal;
            entryHolder.SetHorizontalGravity(GravityFlags.Right);
            entryHolder.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            entryHolder.ShowDividers = ShowDividers.Middle;
            entryHolder.DividerPadding = 20;
            LinearLayout.LayoutParams p = new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.MatchParent, 0.5f);
            view.WeightSum = 1;

            switch (item.RowType)
            {
                case SearchRowTypes.Heading:
                    view.AddView(Heading(item));

                    break;
                case SearchRowTypes.SearchTerms:
                    EditText searchfield = new EditText(context);
                    searchfield.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                    searchfield.Hint = string.Format("Search {0}:", owner.category.Value);
                    searchfield.TextSize = 14f;
                    searchfield.SetSingleLine(true);
                    view.AddView(searchfield);

                    break;
                case SearchRowTypes.Price:
                    view.AddView(Title(item));

                    EditText minPricefield = new EditText(context);
                    minPricefield.LayoutParameters = p;
                    minPricefield.Hint = "min";
                    minPricefield.TextSize = 14f;
                    minPricefield.SetSingleLine(true);
                    minPricefield.InputType = Android.Text.InputTypes.ClassNumber;
                    entryHolder.AddView(minPricefield);

                    EditText maxPricefield = new EditText(context);
                    maxPricefield.LayoutParameters = p;
                    maxPricefield.Hint = "min";
                    maxPricefield.TextSize = 14f;
                    maxPricefield.SetSingleLine(true);
                    maxPricefield.InputType = Android.Text.InputTypes.ClassNumber;
                    entryHolder.AddView(maxPricefield);

                    view.AddView(entryHolder);
                    break;

                case SearchRowTypes.SqFeet:
                    view.AddView(Title(item));

                    EditText minfield = new EditText(context);
                    minfield.LayoutParameters = p;

                    minfield.Hint = "min";
                    minfield.TextSize = 14f;
                    minfield.SetSingleLine(true);
                    minfield.InputType = Android.Text.InputTypes.ClassNumber;
                    entryHolder.AddView(minfield);

                    EditText maxfield = new EditText(context);
                    maxfield.LayoutParameters = p;
                    maxfield.Hint = "min";
                    maxfield.TextSize = 14f;
                    maxfield.SetSingleLine(true);
                    maxfield.InputType = Android.Text.InputTypes.ClassNumber;
                    entryHolder.AddView(maxfield);

                    view.AddView(entryHolder);
                    break;
                default:
                    view.AddView(Title(item));

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

        TextView Heading(SearchRow item)
        {
            TextView heading = new TextView(context);
            heading.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.MatchParent);
            heading.Text = item.Title;
            heading.TextSize = 20f;
            heading.SetTypeface(null, Android.Graphics.TypefaceStyle.Bold);

            return heading;
        }

        TextView Title(SearchRow item)
        {
            TextView title = new TextView(context);
            title.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.MatchParent);
            title.Text = item.Title;
            title.SetPadding(10,0,10,0);

            return title;
        }
    }
}

