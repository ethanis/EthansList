using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace EthansList.MaterialDroid
{
    public class ComboPickerDialogFragment : DialogFragment
    {
        readonly Context _context;
        readonly string _title;
        readonly List<KeyValuePair<object,object>> _options;
        public event EventHandler<ComboPickerItemChangedEventArgs> ItemChanged;
        public List<string> _selectedKeys { get; set; }

        public ComboPickerDialogFragment(Context context, string title, List<KeyValuePair<object,object>> options)
        {
            _context = context;
            _title = title;
            _options = options;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var p = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            var view = new LinearLayout(_context);
            view.LayoutParameters = p;

            var checkBoxHolder = new LinearLayout(_context);
            checkBoxHolder.Orientation = Orientation.Vertical;
            checkBoxHolder.LayoutParameters = p;

            foreach (var option in _options)
            {
                checkBoxHolder.AddView(AddCheckBoxRow(option));
            }
            var scrollView = new ScrollView(_context);
            scrollView.LayoutParameters = p;
            scrollView.AddView(checkBoxHolder);
            view.AddView(scrollView);

            var dialog = new Android.Support.V7.App.AlertDialog.Builder(_context);
            dialog.SetTitle(_title);
            dialog.SetView(view);
            //dialog.SetNegativeButton("Cancel", (s, a) => { });
            dialog.SetPositiveButton("Ok", (s, a) => { });
            return dialog.Create();
        }

        View AddCheckBoxRow(KeyValuePair<object, object> option)
        {
            var already_selected = _selectedKeys.Contains((string)option.Key);
            var row = new ComboRowView(_context, option, already_selected);

            row.ItemSelected += (object sender, CompoundButton.CheckedChangeEventArgs e) => 
            {
                if (ItemChanged != null)
                    ItemChanged(this, new ComboPickerItemChangedEventArgs {InitialArgs = e, Item = option});
            };

            return row;
        }
    }

    public class ComboRowView : LinearLayout
    {
        readonly Context _context;
        public event EventHandler<CompoundButton.CheckedChangeEventArgs> ItemSelected;

        public ComboRowView(Context context, KeyValuePair<object,object> option, bool selectedAlready)
            : base(context)
        {
            _context = context;
            Initialize(option, selectedAlready);
        }

        void Initialize(KeyValuePair<object,object> title, bool selectedAlready)
        {
            this.Orientation = Orientation.Horizontal;
            this.WeightSum = 1;
            this.LayoutParameters = new ListView.LayoutParams(LayoutParams.MatchParent, ConvertDpToPx(40));

            var text = new TextView(_context) { Text = (string)title.Key };
            text.SetTextSize(Android.Util.ComplexUnitType.Dip, 20);
            text.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.WrapContent, LayoutParams.MatchParent);

            AddView(text);

            var checkBoxPlacer = new LinearLayout(_context);
            checkBoxPlacer.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
            checkBoxPlacer.SetHorizontalGravity(GravityFlags.Right);

            var checkbox = new CheckBox(_context);

            if (selectedAlready)
                checkbox.Checked = true;

            checkbox.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.WrapContent, LayoutParams.MatchParent);
            checkbox.SetPadding(0,0, ConvertDpToPx(15), 0);
            checkBoxPlacer.AddView(checkbox);

            checkbox.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) => 
            { 
                Console.WriteLine(e.IsChecked);
                if (ItemSelected != null)
                    ItemSelected(this, e);
            };

            AddView(checkBoxPlacer);
        }

        private int ConvertDpToPx(float dip)
        {
            return (int)(dip * _context.Resources.DisplayMetrics.Density);
        }
    }

    public class ComboPickerItemChangedEventArgs : EventArgs
    { 
        public CompoundButton.CheckedChangeEventArgs InitialArgs { get; set; }
        public KeyValuePair<object, object> Item { get; set; }
    }
}

