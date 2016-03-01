using System;
using Android.Widget;
using Android.Content;
using System.Collections.Generic;
using Android.Views;
using Android.App;
using Android.OS;

namespace EthansList.MaterialDroid
{
    public class SearchOptionsListAdapter : BaseAdapter<SearchRow>
    {
        readonly Context context;
        List<SearchRow> searchOptions;
        SearchOptionsView owner;
        LayoutInflater layoutInflater;

        public SearchOptionsListAdapter(SearchOptionsView owner, Context context, List<SearchRow> searchOptions)
        {
            this.context = context;
            this.searchOptions = searchOptions;
            this.owner = owner;
            layoutInflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
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
            LinearLayout.LayoutParams f = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);

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
                case SearchRowTypes.PriceDoubleEntry:
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

                case SearchRowTypes.DoubleEntry:
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
                case SearchRowTypes.SingleEntryLabel:
                    view.AddView(Title(item));

                    EditText inputField = new EditText(context);
                    inputField.LayoutParameters = f;

                    inputField.Hint = "make / model";
                    inputField.TextSize = 14f;
                    inputField.SetSingleLine(true);
                    entryHolder.AddView(inputField);

                    view.AddView(entryHolder);
                    break;
                case SearchRowTypes.SinglePicker:
                    view.AddView(Title(item));

                    TextView display = new TextView(context);
                    display.LayoutParameters = f;
                    display.Gravity = GravityFlags.Right;
                    display.Text = "Any";
                    display.SetPadding(0,0,ConvertDpToPx(50),0);

                    display.Click += (object sender, EventArgs e) => 
                    { 
                        //var builder = new Android.Support.V7.App.AlertDialog.Builder (context);

                        //builder.SetTitle ("Hello")
                        //    .SetMessage ("Is this material design?")
                        //    .SetPositiveButton ("Ok", delegate { Console.WriteLine("Yes"); })
                        //    .SetNegativeButton ("Cancel", delegate { Console.WriteLine("No"); }); 

                        //builder.Create().Show ();

                        //var numberPicker = layoutInflater.Inflate(Resource.Layout.NumberPickerDialog, null);
                        //numberPicker.Click += 

                        var dialog = new NumberPickerDialogFragment(context, 10, 1337, 42, null);
                        dialog.Show(((Activity)context).FragmentManager, "number"); 

                    };

                    view.AddView(display);

                    break;
                case SearchRowTypes.Space:
                    View strut = new View(context);
                    strut.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, 
                                                                        ConvertDpToPx(25));
                    view.ShowDividers = ShowDividers.None;
                    view.AddView(strut);
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
            return (int)(dip * this.context.Resources.DisplayMetrics.Density);
        }
    }

    public class NumberPickerDialogFragment : DialogFragment
    {
        private readonly Context _context;
        private readonly int _min, _max, _current;
        private readonly NumberPicker.IOnValueChangeListener _listener;

        public NumberPickerDialogFragment(Context context, int min, int max, int current, NumberPicker.IOnValueChangeListener listener)
        {
            _context = context;
            _min = min;
            _max = max;
            _current = current;
            _listener = listener;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
            var view = inflater.Inflate(Resource.Layout.NumberPickerDialog, null);
            var numberPicker = view.FindViewById<NumberPicker>(Resource.Id.numberPicker);
            numberPicker.MaxValue = _max;
            numberPicker.MinValue = _min;
            numberPicker.Value = _current;
            numberPicker.SetOnValueChangedListener(_listener);

            var dialog = new Android.Support.V7.App.AlertDialog.Builder(_context);
            dialog.SetTitle("Hello");
            dialog.SetView(view);
            dialog.SetNegativeButton("Cancel", (s, a) => { });
            dialog.SetPositiveButton("Ok", (s, a) => { });
            return dialog.Create();
        }
    }
}

