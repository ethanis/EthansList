
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

            return view;
        }
    }

    public class SearchOptionsView : LinearLayout
    {
        readonly Context context;
        readonly Location location;
        public readonly KeyValuePair<string, string> category;

        #region GeneratedFromSearchOptions
        public int MaxListings 
        { 
            get { return maxListings; } 
            set { maxListings = value; }
        }
        private int maxListings = 25;
        public int? WeeksOld { get; set; }
        public KeyValuePair<object,object> SubCategory { get; set; }
        public Dictionary<string, string> SearchItems { get; set;}
        public Dictionary<object, KeyValuePair<object, object>> Conditions { get; set; }
        #endregion


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

            SearchItems = new Dictionary<string, string>();
            Conditions = new Dictionary<object,  KeyValuePair<object, object>>();

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

            proceedButton.Click += ProceedButton_Click;
        }

        void ProceedButton_Click(object sender, EventArgs e)
        {
            QueryGeneration queryHelper = new QueryGeneration();

            SearchObject searchObject = new SearchObject();
            searchObject.SearchLocation = location;
            searchObject.Category = SubCategory.Value != null ? new KeyValuePair<object,object>(SubCategory.Value, SubCategory.Key) : new KeyValuePair<object,object>(category.Key, category.Value);
            searchObject.SearchItems = this.SearchItems;
            searchObject.Conditions = this.Conditions;

            var query = queryHelper.Generate(searchObject);
            Toast.MakeText(context, query, ToastLength.Short).Show();
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

                new SearchRow {Title = "Search Terms", RowType = SearchRowTypes.SearchTerms, QueryPrefix = "query"},
                new SearchRow {Title = "Price Cell", RowType = SearchRowTypes.PriceDoubleEntry},
                new SearchRow {Title = "Sq Feet", RowType = SearchRowTypes.DoubleEntry},

                new SearchRow {Title = "Make/Model", RowType = SearchRowTypes.SingleEntryLabel, QueryPrefix = "auto_make_model"},
                new SearchRow {Title = "Year", RowType = SearchRowTypes.DoubleEntry},
                new SearchRow {Title = "Odometer", RowType = SearchRowTypes.DoubleEntry},

                new SearchRow {Title = "Space", RowType = SearchRowTypes.Space},

                new SearchRow {Title = "Options", RowType = SearchRowTypes.Heading},

                new SearchRow 
                {
                    Title = "Bedrooms", 
                    RowType = SearchRowTypes.SinglePicker,
                    NumberPickerOptions = new NumberPickerOptions {Initial = 1, Minimum = 0, Maximum = 10, Step = 1, DisplaySuffix = "+"},
                    QueryPrefix = "bedrooms"
                },
                new SearchRow 
                {
                    Title = "Bathrooms", 
                    RowType = SearchRowTypes.SinglePicker,
                    NumberPickerOptions = new NumberPickerOptions {Initial = 1, Minimum = 0, Maximum = 10, Step = 1, DisplaySuffix = "+"},
                    QueryPrefix = "bathrooms"
                },

                #region ComboPickers
                new SearchRow
                {
                    Title = "Condition",
                    RowType = SearchRowTypes.ComboPicker,
                    ComboPickerOptions = Categories.ComboOptions["condition"],
                    QueryPrefix = "condition"
                },
                new SearchRow
                {
                    Title = "Job Type",
                    RowType = SearchRowTypes.ComboPicker,
                    ComboPickerOptions = Categories.ComboOptions["employment_type"],
                    QueryPrefix = "employment_type"
                },
                new SearchRow
                {
                    Title = "Paid",
                    RowType = SearchRowTypes.ComboPicker,
                    ComboPickerOptions = Categories.ComboOptions["is_paid"],
                    QueryPrefix = "is_paid"
                },
                new SearchRow
                {
                    Title = "Cylinders",
                    RowType = SearchRowTypes.ComboPicker,
                    ComboPickerOptions = Categories.ComboOptions["auto_cylinders"],
                    QueryPrefix = "auto_cylinders"
                },
                new SearchRow
                {
                    Title = "Drivetrain",
                    RowType = SearchRowTypes.ComboPicker,
                    ComboPickerOptions = Categories.ComboOptions["auto_drivetrain"],
                    QueryPrefix = "auto_drivetrain"
                },
                new SearchRow
                {
                    Title = "Fuel Type",
                    RowType = SearchRowTypes.ComboPicker,
                    ComboPickerOptions = Categories.ComboOptions["auto_fuel_type"],
                    QueryPrefix = "auto_fuel_type"
                },
                new SearchRow
                {
                    Title = "Paint Color",
                    RowType = SearchRowTypes.ComboPicker,
                    ComboPickerOptions = Categories.ComboOptions["auto_paint"],
                    QueryPrefix = "auto_paint"
                },
                new SearchRow
                {
                    Title = "Title Status",
                    RowType = SearchRowTypes.ComboPicker,
                    ComboPickerOptions = Categories.ComboOptions["auto_title_status"],
                    QueryPrefix = "auto_title_status"
                },
                new SearchRow
                {
                    Title = "Transmission",
                    RowType = SearchRowTypes.ComboPicker,
                    ComboPickerOptions = Categories.ComboOptions["auto_transmission"],
                    QueryPrefix = "auto_transmission"
                },

                #endregion

                new SearchRow 
                {
                    Title = "Posted Date", 
                    RowType = SearchRowTypes.SinglePicker,
                    NumberPickerOptions = new NumberPickerOptions {Initial = 1, Minimum = 0, Maximum = 5, Step = 1},
                    QueryPrefix = "posted_date"
                },
                new SearchRow 
                {
                    Title = "Max Listings", 
                    RowType = SearchRowTypes.SinglePicker,
                    NumberPickerOptions = new NumberPickerOptions {Initial = 1, Minimum = 1, Maximum = 4, Step = 25},
                    QueryPrefix = "max listings"
                },

            };

            return searchOptions;
        }
    }
}

