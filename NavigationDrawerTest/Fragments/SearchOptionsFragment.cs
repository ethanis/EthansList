
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
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

            var scrollView = new ScrollView(this.Activity);
            scrollView.AddView(view);

            return scrollView;
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
 
            var tableItems = GetTableSetup();
            foreach (var item in tableItems)
            {
                AddRowToLayout(item);
            }

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

            FragmentTransaction transaction = ((Activity)context).FragmentManager.BeginTransaction();
            SearchResultsFragment resultsFragment = new SearchResultsFragment();
            resultsFragment.Query = queryHelper.Generate(searchObject);
            resultsFragment.MaxListings = MaxListings;
            resultsFragment.WeeksOld = WeeksOld;

            transaction.Replace(Resource.Id.frameLayout, resultsFragment);
            transaction.AddToBackStack(null);
            transaction.Commit();
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
            };

            searchOptions.Add(new SearchRow { Title = "Search Terms", RowType = SearchRowTypes.SearchTerms, QueryPrefix = "query" });

            if (Categories.Groups.Find(x => x.Name == "Housing").Items.Contains(category) || Categories.Groups.Find(x => x.Name == "For Sale").Items.Contains(category))
            {
                searchOptions.Add(new SearchRow
                {
                    Title = "Price",
                    RowType = SearchRowTypes.PriceDoubleEntry,
                    QueryPrefix = "min_price",
                    SecondQueryPrefix = "max_price"
                });
            }

            if (Categories.Groups.Find(x => x.Name == "Housing").Items.Contains(category))
            {
                searchOptions.Add(new SearchRow
                {
                    Title = "Sq Feet",
                    RowType = SearchRowTypes.DoubleEntry,
                    QueryPrefix = "minSqft",
                    SecondQueryPrefix = "maxSqft"
                });
            }

            if (Categories.Autos.Contains(category.Key))
            {
                searchOptions.Add(new SearchRow
                {
                    Title = "Make/Model",
                    RowType = SearchRowTypes.SingleEntryLabel,
                    QueryPrefix = "auto_make_model"
                });

                searchOptions.Add(new SearchRow
                {
                    Title = "Year",
                    RowType = SearchRowTypes.DoubleEntry,
                    QueryPrefix = "min_auto_year",
                    SecondQueryPrefix = "max_auto_year"
                });
                searchOptions.Add(new SearchRow
                {
                    Title = "Odometer",
                    RowType = SearchRowTypes.DoubleEntry,
                    QueryPrefix = "min_auto_miles",
                SecondQueryPrefix = "max_auto_miles"
                });
            }

            searchOptions.Add(new SearchRow {Title = "Space", RowType = SearchRowTypes.Space});
            searchOptions.Add(new SearchRow {Title = "Options", RowType = SearchRowTypes.Heading});

            if (Categories.SubCategories.ContainsKey(category.Key))
            {
                searchOptions.Add(new SearchRow
                {
                    Title = "Sub Category",
                    RowType = SearchRowTypes.SubCatPicker,
                    ComboPickerOptions = Categories.SubCategories[category.Key],
                    QueryPrefix = category.Key
                });
            }

            if (Categories.Housing.Contains(category.Key))
            {
                searchOptions.Add(new SearchRow
                {
                    Title = "Bedrooms",
                    RowType = SearchRowTypes.SinglePicker,
                    NumberPickerOptions = new NumberPickerOptions { Initial = 1, Minimum = 0, Maximum = 10, Step = 1, DisplaySuffix = "+" },
                    QueryPrefix = "bedrooms"
                });
                searchOptions.Add(new SearchRow
                {
                    Title = "Bathrooms",
                    RowType = SearchRowTypes.SinglePicker,
                    NumberPickerOptions = new NumberPickerOptions { Initial = 1, Minimum = 0, Maximum = 10, Step = 1, DisplaySuffix = "+" },
                    QueryPrefix = "bathrooms"
                });
            }
            #region ComboPickers
            if (Categories.Groups.Find(x => x.Name == "For Sale").Items.Contains(category) || Categories.Autos.Contains(category.Key))
            {
                searchOptions.Add(new SearchRow
                {
                    Title = "Condition",
                    RowType = SearchRowTypes.ComboPicker,
                    ComboPickerOptions = Categories.ComboOptions["condition"],
                    QueryPrefix = "condition"
                });
            }
            if (Categories.Groups.Find(x => x.Name == "Jobs").Items.Contains(category))
            {
                searchOptions.Add(new SearchRow
                {
                    Title = "Job Type",
                    RowType = SearchRowTypes.ComboPicker,
                    ComboPickerOptions = Categories.ComboOptions["employment_type"],
                    QueryPrefix = "employment_type"
                });
            }
            if (Categories.Groups.Find(x => x.Name == "Gigs").Items.Contains(category))
            {
                searchOptions.Add(new SearchRow
                {
                    Title = "Paid",
                    RowType = SearchRowTypes.ComboPicker,
                    ComboPickerOptions = Categories.ComboOptions["is_paid"],
                    QueryPrefix = "is_paid"
                });
            }
            if (Categories.Autos.Contains(category.Key))
            {
                searchOptions.Add(new SearchRow
                {
                    Title = "Cylinders",
                    RowType = SearchRowTypes.ComboPicker,
                    ComboPickerOptions = Categories.ComboOptions["auto_cylinders"],
                    QueryPrefix = "auto_cylinders"
                });
                searchOptions.Add(new SearchRow
                {
                    Title = "Drivetrain",
                    RowType = SearchRowTypes.ComboPicker,
                    ComboPickerOptions = Categories.ComboOptions["auto_drivetrain"],
                    QueryPrefix = "auto_drivetrain"
                });
                searchOptions.Add(new SearchRow
                {
                    Title = "Fuel Type",
                    RowType = SearchRowTypes.ComboPicker,
                    ComboPickerOptions = Categories.ComboOptions["auto_fuel_type"],
                    QueryPrefix = "auto_fuel_type"
                });
                searchOptions.Add(new SearchRow
                {
                    Title = "Paint Color",
                    RowType = SearchRowTypes.ComboPicker,
                    ComboPickerOptions = Categories.ComboOptions["auto_paint"],
                    QueryPrefix = "auto_paint"
                });
                searchOptions.Add(new SearchRow
                {
                    Title = "Title Status",
                    RowType = SearchRowTypes.ComboPicker,
                    ComboPickerOptions = Categories.ComboOptions["auto_title_status"],
                    QueryPrefix = "auto_title_status"
                });
                searchOptions.Add(new SearchRow
                {
                    Title = "Transmission",
                    RowType = SearchRowTypes.ComboPicker,
                    ComboPickerOptions = Categories.ComboOptions["auto_transmission"],
                    QueryPrefix = "auto_transmission"
                });
            }
            #endregion

            searchOptions.Add(new SearchRow {
                Title = "Posted Date", 
                RowType = SearchRowTypes.SubCatPicker,
                ComboPickerOptions = new List<KeyValuePair<object, object>>
                {
                    new KeyValuePair<object, object>("Any", null),
                    new KeyValuePair<object, object>("Today", -1),
                    new KeyValuePair<object, object>("1 Week Old", 1),
                    new KeyValuePair<object, object>("2 Weeks Old", 2),
                    new KeyValuePair<object, object>("3 Weeks Old", 3),
                    new KeyValuePair<object, object>("4 Weeks Old", 4),
                },
                QueryPrefix = "posted_date"
            });
            searchOptions.Add(new SearchRow {
                Title = "Max Listings", 
                RowType = SearchRowTypes.SubCatPicker,
                ComboPickerOptions = new List<KeyValuePair<object, object>>
                {
                    new KeyValuePair<object, object>("25", 25),
                    new KeyValuePair<object, object>("50", 50),
                    new KeyValuePair<object, object>("75", 75),
                    new KeyValuePair<object, object>("100", 100),
                },
                QueryPrefix = "max_listings"
            });

            return searchOptions;
        }

        void AddRowToLayout(SearchRow item)
        {
            var row = new LinearLayout(context) { Orientation = Orientation.Horizontal };
            row.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, 100);
            row.WeightSum = 1;

            LinearLayout.LayoutParams p = new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.MatchParent, 0.5f);
            LinearLayout.LayoutParams f = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            var entryHolder = new LinearLayout(context);
            entryHolder.Orientation = Orientation.Horizontal;
            entryHolder.SetHorizontalGravity(GravityFlags.Right);
            entryHolder.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            entryHolder.ShowDividers = ShowDividers.Middle;
            entryHolder.DividerPadding = 20;

            switch (item.RowType)
            {
                case SearchRowTypes.Heading:
                    row.AddView(Heading(item));

                break;
                case SearchRowTypes.SearchTerms:
                    EditText searchfield = new EditText(context);
                    searchfield.LayoutParameters = f;
                    searchfield.Hint = string.Format("Search {0}:", this.category.Value);
                    searchfield.TextSize = 14f;
                    searchfield.SetSingleLine(true);
                    searchfield.InputType = InputTypes.ClassText;
                    row.AddView(searchfield);

                    searchfield.TextChanged += (object sender, TextChangedEventArgs e) => {
                        AddSearchItem(item.QueryPrefix, e.Text.ToString());
                    };

                break;
                case SearchRowTypes.PriceDoubleEntry:
                    row.AddView(Title(item));

                    EditText minPricefield = new EditText(context);
                    minPricefield.LayoutParameters = p;
                    minPricefield.Hint = "min";
                    minPricefield.TextSize = 14f;
                    minPricefield.SetSingleLine(true);
                    minPricefield.InputType = InputTypes.ClassNumber;
                    entryHolder.AddView(minPricefield);

                    EditText maxPricefield = new EditText(context);
                    maxPricefield.LayoutParameters = p;
                    maxPricefield.Hint = "min";
                    maxPricefield.TextSize = 14f;
                    maxPricefield.SetSingleLine(true);
                    maxPricefield.InputType = InputTypes.ClassNumber;
                    entryHolder.AddView(maxPricefield);

                    minPricefield.TextChanged += (object sender, TextChangedEventArgs e) => { 
                        AddSearchItem(item.QueryPrefix, e.Text.ToString());
                    };

                    maxPricefield.TextChanged += (object sender, TextChangedEventArgs e) => { 
                        AddSearchItem(item.SecondQueryPrefix, e.Text.ToString());
                    };

                    row.AddView(entryHolder);
                break;

                case SearchRowTypes.DoubleEntry:
                    row.AddView(Title(item));

                    EditText minfield = new EditText(context);
                    minfield.LayoutParameters = p;

                    minfield.Hint = "min";
                    minfield.TextSize = 14f;
                    minfield.SetSingleLine(true);
                    minfield.InputType = InputTypes.ClassNumber;
                    entryHolder.AddView(minfield);

                    EditText maxfield = new EditText(context);
                    maxfield.LayoutParameters = p;
                    maxfield.Hint = "min";
                    maxfield.TextSize = 14f;
                    maxfield.SetSingleLine(true);
                    maxfield.InputType = InputTypes.ClassNumber;
                    entryHolder.AddView(maxfield);

                    minfield.TextChanged += (object sender, TextChangedEventArgs e) => { 
                        AddSearchItem(item.QueryPrefix, e.Text.ToString());
                    };

                    maxfield.TextChanged += (object sender, TextChangedEventArgs e) => { 
                        AddSearchItem(item.SecondQueryPrefix, e.Text.ToString());
                    };

                    row.AddView(entryHolder);

                break;
                case SearchRowTypes.SingleEntryLabel:
                    row.AddView(Title(item));

                    EditText inputField = new EditText(context);
                    inputField.LayoutParameters = f;

                    inputField.Hint = "make / model";
                    inputField.TextSize = 14f;
                    inputField.SetSingleLine(true);
                    entryHolder.AddView(inputField);

                    inputField.TextChanged += (object sender, TextChangedEventArgs e) => { 
                        AddSearchItem(item.QueryPrefix, e.Text.ToString());
                    };

                    row.AddView(entryHolder);
                break;
                case SearchRowTypes.SinglePicker:
                    row.AddView(Title(item));

                    TextView display = new TextView(context);
                    display.LayoutParameters = f;
                    display.Gravity = GravityFlags.Right;
                    display.Text = "Any";
                    display.SetPadding(0,0,ConvertDpToPx(50),0);

                    var dialog = new NumberPickerDialogFragment(context, item.Title, item.NumberPickerOptions, item.QueryPrefix);
                    display.Click += (object sender, EventArgs e) => 
                    {
                        dialog.Show(((Activity)context).FragmentManager, "number");
                    };

                    dialog.NumberChanged += (object sender, NumberPickerDialogFragment.NumberPickerValueChanged e) =>
                    {
                        display.Text = e.Value.ToString() + item.NumberPickerOptions.DisplaySuffix;
                        AddSearchItem(e.CallerKey, e.Value.ToString());
                    };

                    row.AddView(display);

                break;
                case SearchRowTypes.ComboPicker:
                    row.AddView(Title(item));

                    TextView comboLabel = new TextView(context);
                    comboLabel.LayoutParameters = f;
                    comboLabel.Gravity = GravityFlags.Right;
                    comboLabel.Text = "Any";
                    comboLabel.SetPadding(0,0,ConvertDpToPx(50),0);

                    var comboDialog = new ComboPickerDialogFragment(context, item.Title, item.ComboPickerOptions);

                    comboLabel.Click += (object sender, EventArgs e) => 
                    {
                        var selectedKeys = (from kvp in Conditions where (string)kvp.Value.Key == item.QueryPrefix select (string)kvp.Key).ToList();
                        comboDialog._selectedKeys = selectedKeys;
                        comboDialog.Show(((Activity)context).FragmentManager, "combo");
                    };

                    comboDialog.ItemChanged += (object sender, ComboPickerItemChangedEventArgs e) => { 
                        if (e.InitialArgs.IsChecked)
                        {
                            Conditions.Add(e.Item.Key, new KeyValuePair<object, object>(item.QueryPrefix, e.Item.Value));
                            Console.WriteLine ("Added Key: " + e.Item.Key + ", Value: " + e.Item.Value + " with prefix: " + item.QueryPrefix);
                        }
                        else
                        {
                            Conditions.Remove(e.Item.Key);
                            Console.WriteLine ("Removed Key: " + e.Item.Key + ", Value: " + e.Item.Value);
                        }

                        var keys = (from kvp in Conditions where (string)kvp.Value.Key == item.QueryPrefix select (string)kvp.Key).ToList();
                        var text = keys.Count > 0 ? string.Join(", ", keys.ToArray()) : "Any";
                        comboLabel.Text = text;
                    };

                    row.AddView(comboLabel);

                break;
                case SearchRowTypes.SubCatPicker:
                    row.AddView(Title(item));

                    TextView catLabel = new TextView(context);
                    catLabel.LayoutParameters = f;
                    catLabel.Gravity = GravityFlags.Right;
                    catLabel.Text = (string)item.ComboPickerOptions.First().Key;
                    catLabel.SetPadding(0,0,ConvertDpToPx(50),0);

                    if (item.Title == "Sub Category")
                        SubCategory = item.ComboPickerOptions.First();

                    var categoryDialog = new SingleStringDialogFragment(context, item.Title, item.ComboPickerOptions);

                    catLabel.Click += (object sender, EventArgs e) => {
                        categoryDialog.Show(((Activity)context).FragmentManager, "stringPick");
                    };

                    categoryDialog.CatPicked += (object sender, SubCatSelectedEventArgs e) => { 
                        categoryDialog.Dismiss();

                        if (item.Title != "Posted Date" && item.Title != "Max Listings")
                        {
                            SubCategory = e.SubCatPicked;
                        }
                        else if (item.Title == "Posted Date")
                        {
                            WeeksOld = (int?)e.SubCatPicked.Value;
                        }
                        else if (item.Title == "Max Listings")
                        {
                            MaxListings = (int)e.SubCatPicked.Value;
                        }

                        catLabel.Text = (string)e.SubCatPicked.Key;
                    };

                    row.AddView(catLabel);
                break;
                case SearchRowTypes.Space:
                    View strut = new View(context);
                    strut.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, 
                                                                        ConvertDpToPx(25));
                    row.ShowDividers = ShowDividers.None;
                    row.AddView(strut);
                break;
                default:
                    row.AddView(Title(item));

                break;
            }

            AddView(row);
        }

        TextView Heading(SearchRow item)
        {
            TextView heading = new TextView(context);
            heading.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.MatchParent);
            heading.Text = item.Title;
            heading.TextSize = 18f;
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

        private int ConvertDpToPx(float dip)
        {
            return (int)(dip * context.Resources.DisplayMetrics.Density);
        }

        private void AddSearchItem (string itemKey, string itemValue)
        {
            if (SearchItems.ContainsKey(itemKey))
                SearchItems[itemKey] = itemValue;
            else
                SearchItems.Add(itemKey, itemValue);
        }
    }
}

