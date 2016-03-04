using System;
using Android.App;
using System.Collections.Generic;
using Android.Widget;
using System.Linq;
using Android.Content;
using Android.Views;

namespace EthansList.MaterialDroid
{
    public class StateListAdapter : BaseAdapter<String>
    {
        SortedSet<String> states;
        readonly Context _context;

        public StateListAdapter(Context context, SortedSet<String> states)
        {
            this.states = states;
            _context = context;
        }

        public override string this[int index]
        {
            get
            {
                return states.ElementAt(index);
            }
        }

        public override int Count
        {
            get
            {
                return states.Count;
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

            view.ItemLabel.Text = states.ElementAt(position);

            return view;
        }
    }
}

