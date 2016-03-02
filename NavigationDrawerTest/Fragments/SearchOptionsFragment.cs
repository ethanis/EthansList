
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
using EthansList.Shared;

namespace EthansList.MaterialDroid
{
    public class SearchOptionsFragment : Fragment
    {
        public Location SearchLocation { get; set; }
        public KeyValuePair<string, string> Category { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = new SearchOptionsView(this.Activity, SearchLocation, Category);
            Console.WriteLine(SearchLocation.SiteName);
            Console.WriteLine(Category.Value);
            return view;
        }
    }

    public class SearchOptionsView : LinearLayout
    {
        readonly Context context;
        readonly Location location;
        public readonly KeyValuePair<string, string> category;

        public TextView SearchCityText;
        public Button proceedButton;
        public ListView SearchTermsTable;
        public ListView OptionsTable;

        public SearchOptionsView(Context context, Location location, KeyValuePair<string,string> category)
            : base(context)
        {
            this.context = context;
            this.location = location;
            this.category = category;

            Initialize();
        }

        void Initialize()
        {
            this.Orientation = Orientation.Vertical;
            this.WeightSum = 1;

            SearchCityText = new TextView(context);
            SearchCityText.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
            SearchCityText.Text = string.Format("Search {0} for: ", location.SiteName);

            LinearLayout searchCityHolder = RowHolder();
            searchCityHolder.AddView(SearchCityText);
            AddView(searchCityHolder);

            proceedButton = new Button(context);
            proceedButton.LayoutParameters = new LayoutParams(0, LayoutParams.WrapContent, 0.5f);
            proceedButton.Text = "Search";
            LinearLayout buttonLayout = RowHolder();
            buttonLayout.AddView(proceedButton);
            AddView(buttonLayout);

            SearchTermsTable = new ListView(context);
            SearchTermsTable.Adapter = new SearchOptionsListAdapter(this, context, GetTableSetup());
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
            List<SearchRow> searchOptions = new List<SearchRow>
            {
                new SearchRow {Title = "Search Terms", RowType = SearchRowTypes.Heading},

                new SearchRow {Title = "Search Terms", RowType = SearchRowTypes.SearchTerms},
                new SearchRow {Title = "Price Cell", RowType = SearchRowTypes.PriceDoubleEntry},
                new SearchRow {Title = "Sq Feet", RowType = SearchRowTypes.DoubleEntry},

                new SearchRow {Title = "Make/Model", RowType = SearchRowTypes.SingleEntryLabel},
                new SearchRow {Title = "Year", RowType = SearchRowTypes.DoubleEntry},
                new SearchRow {Title = "Odometer", RowType = SearchRowTypes.DoubleEntry},

                new SearchRow {Title = "Space", RowType = SearchRowTypes.Space},

                new SearchRow {Title = "Options", RowType = SearchRowTypes.Heading},

                new SearchRow 
                {
                    Title = "Bedrooms", 
                    RowType = SearchRowTypes.SinglePicker,
                    NumberPickerOptions = new NumberPickerOptions {Initial = 1, Minimum = 0, Maximum = 10, Step = 1, DisplaySuffix = "+"}
                },
                new SearchRow 
                {
                    Title = "Bathrooms", 
                    RowType = SearchRowTypes.SinglePicker,
                    NumberPickerOptions = new NumberPickerOptions {Initial = 1, Minimum = 0, Maximum = 10, Step = 1, DisplaySuffix = "+"}
                },


                new SearchRow
                {
                    Title = "Condition",
                    RowType = SearchRowTypes.ComboPicker,
                },

                new SearchRow 
                {
                    Title = "Posted Date", 
                    RowType = SearchRowTypes.SinglePicker,
                    NumberPickerOptions = new NumberPickerOptions {Initial = 1, Minimum = 0, Maximum = 5, Step = 1}
                },
                new SearchRow 
                {
                    Title = "Max Listings", 
                    RowType = SearchRowTypes.SinglePicker,
                    NumberPickerOptions = new NumberPickerOptions {Initial = 1, Minimum = 1, Maximum = 4, Step = 25}
                },

            };

            return searchOptions;
        }
    }
}

