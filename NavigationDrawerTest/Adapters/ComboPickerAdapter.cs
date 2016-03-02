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
            return new ComboRowView(_context, _options[position]);
        }
    }

    public class ComboRowView : LinearLayout
    {
        readonly Context _context;

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

            AddView(checkBoxPlacer);
        }

        private int ConvertDpToPx(float dip)
        {
            return (int)(dip * _context.Resources.DisplayMetrics.Density);
        }
    }
}
