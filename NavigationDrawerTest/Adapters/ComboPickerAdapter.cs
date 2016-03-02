using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace EthansList.MaterialDroid
{

    public class ComboPickerAdapter : BaseAdapter<KeyValuePair<object, object>>
    {
        readonly List<KeyValuePair<object, object>> _options;
        readonly Context _context;
        public event EventHandler<ComboPickerItemChangedEventArgs> CheckedChanged;

        public ComboPickerAdapter(Context context, List<KeyValuePair<object, object>> options)
        {
            _context = context;
            _options = options;
        }

        public override KeyValuePair<object, object> this[int position]
        {
            get
            {
                return _options[position];
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get
            {
                return _options.Count;
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = new ComboRowView(_context, _options[position]);

            view.ItemSelected += (object sender, CompoundButton.CheckedChangeEventArgs e) => 
            {
                if (CheckedChanged != null)
                    CheckedChanged(this, new ComboPickerItemChangedEventArgs {InitialArgs = e, Item = _options[position]});
            };

            return view;
        }
    }

    public class ComboPickerItemChangedEventArgs : EventArgs
    { 
        public CompoundButton.CheckedChangeEventArgs InitialArgs { get; set; }
        public KeyValuePair<object, object> Item { get; set; }
    }

    public class ComboRowView : LinearLayout
    {
        readonly Context _context;
        public event EventHandler<CompoundButton.CheckedChangeEventArgs> ItemSelected;

        public ComboRowView(Context context, KeyValuePair<object,object> option)
            : base(context)
        {
            _context = context;
            Initialize(option);
        }

        void Initialize(KeyValuePair<object,object> title)
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
}
