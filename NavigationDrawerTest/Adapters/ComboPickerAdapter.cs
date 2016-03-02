using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace EthansList.MaterialDroid
{

    public class ComboPickerAdapter : BaseAdapter<string>
    {
        readonly string[] _options;
        readonly Context _context;

        public ComboPickerAdapter(Context context, string[] options)
        {
            _context = context;
            _options = options;
        }

        public override string this[int position]
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
                return _options.Length;
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

        public ComboRowView(Context context, string title)
            : base(context)
        {
            _context = context;
            Initialize(title);
        }

        void Initialize(string title)
        {
            this.Orientation = Orientation.Horizontal;
            this.WeightSum = 1;
            this.LayoutParameters = new ListView.LayoutParams(LayoutParams.MatchParent, ConvertDpToPx(40));


            var text = new TextView(_context) { Text = title };
            text.SetTextSize(Android.Util.ComplexUnitType.Dip, 20);
            text.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.WrapContent, LayoutParams.MatchParent);

            AddView(text);

            var checkBoxPlacer = new LinearLayout(_context);
            checkBoxPlacer.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
            checkBoxPlacer.SetBackgroundColor(Android.Graphics.Color.AliceBlue);
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
