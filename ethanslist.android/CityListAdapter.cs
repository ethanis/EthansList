using System;
using Android.Widget;
using Android.App;
using System.Collections.Generic;
using System.Linq;

namespace ethanslist.android
{
    public class CityListAdapter : BaseAdapter<Location>
    {
        Activity context;
        List<Location> cities;

        public CityListAdapter(Activity context, List<Location> cities)
        {
            this.context = context;
            this.cities = cities;
        }

        public override Location this[int index]
        {
            get
            {
                return cities[index];
            }
        }

        public override int Count
        {
            get
            {
                return cities.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            var view = context.LayoutInflater.Inflate(Resource.Layout.CityRow, parent, false);
            var city = view.FindViewById<TextView>(Resource.Id.cityListViewItem);

            city.Text = cities[position].SiteName;
            return view;
        }
    }
}

