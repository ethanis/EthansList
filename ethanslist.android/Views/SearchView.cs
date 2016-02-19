
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
        public TextView SearchCityText;
        public Button proceedButton;
        public ListView SearchTermsTable;
        public ListView OptionsTable;
        SearchOptionsListAdapter OptionsTableSource;

        private Context context;

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

            SearchCityText = new TextView(context);
            SearchCityText.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
            SearchCityText.Text = "Search Craigslist For:";
            SearchCityText.SetTextColor(ColorScheme.MidnightBlue);
            LinearLayout searchCityHolder = RowHolder();
            searchCityHolder.AddView(SearchCityText);
            AddView(searchCityHolder);

            proceedButton = new Button(context);
            proceedButton.LayoutParameters = new LayoutParams(0, LayoutParams.WrapContent, 0.5f);
            proceedButton.SetBackgroundColor(ColorScheme.MidnightBlue);
            proceedButton.Text = "Search";
            proceedButton.SetTextColor(ColorScheme.Clouds);
            LinearLayout buttonLayout = RowHolder();
            buttonLayout.AddView(proceedButton);
            AddView(buttonLayout);

            SearchTermsTable = new ListView(context);
            SearchTermsTable.Adapter = OptionsTableSource = new SearchOptionsListAdapter(context, GetTableSetup());
            AddView(SearchTermsTable);
        }

        private LinearLayout RowHolder(GravityFlags gravity = GravityFlags.CenterHorizontal)
        {
            LinearLayout layout = new LinearLayout(context);
            layout.Orientation = Orientation.Horizontal;
            layout.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            layout.WeightSum = 1;
            layout.SetGravity(gravity);
            return layout;
        }

        private List<SearchRow> GetTableSetup()
        {
            List<SearchRow> searchOptions = new List<SearchRow>()
            {
                    new SearchRow(){Title = "Heading", RowType = SearchRowTypes.Heading},
                    new SearchRow(){Title = "Search Terms", RowType = SearchRowTypes.SearchTerms},
                    new SearchRow(){Title = "Price Cell", RowType = SearchRowTypes.Price},
            };

            return searchOptions;
        }
    }
}

