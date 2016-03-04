using System;
using Android.Widget;
using Android.App;
using System.Collections.Generic;
using System.Linq;
using EthansList.Shared;
using Android.Content;
using Android.Views;

namespace EthansList.MaterialDroid
{
    public class CityListAdapter : BaseAdapter<Location>
    {
        readonly Context _context;
        IEnumerable<Location> cities;
        LayoutInflater layoutInflater;

        public CityListAdapter(Context context, IEnumerable<Location> cities)
        {
            this.cities = cities;
            this._context = context;
        }

        public override Location this[int index]
        {
            get
            {
                return cities.ElementAt(index);
            }
        }

        public override int Count
        {
            get
            {
                return cities.Count();
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            var view = (CityPickerRow)convertView;
            if (view == null)
            {
                view = new CityPickerRow(_context);
            }

            view.ItemLabel.Text = cities.ElementAt(position).SiteName;

            return view;
        }
    }

    public class CityPickerRow : LinearLayout
    {
        readonly Context _context;
        public TextView ItemLabel { get; set; }

        private int rowHeight;

        public CityPickerRow(Context context)
            :base (context)
        {
            _context = context;
            rowHeight = ConvertDpToPx(_context.Resources.GetInteger(Resource.Integer.textLabelRowHeight));
            Initialize();
        }

        void Initialize()
        {
            Orientation = Orientation.Horizontal;

            ItemLabel = new TextView(_context);
            ItemLabel.Gravity = GravityFlags.CenterVertical;
            ItemLabel.SetTextSize(Android.Util.ComplexUnitType.Px, rowHeight * 0.50f);
            ItemLabel.SetPadding((int)(rowHeight * 0.1), (int)(rowHeight * 0.15), (int)(rowHeight * 0.1), (int)(rowHeight * 0.15));

            ItemLabel.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            AddView(ItemLabel);
        }

        private int ConvertDpToPx(float dip)
        {
            return (int)(dip * _context.Resources.DisplayMetrics.Density);
        }
    }
}

