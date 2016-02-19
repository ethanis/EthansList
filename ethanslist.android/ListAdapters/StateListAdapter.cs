using System;
using Android.App;
using System.Collections.Generic;
using Android.Widget;
using System.Linq;

namespace ethanslist.android
{
    public class StateListAdapter : BaseAdapter<String>
    {
        Activity context;
        SortedSet<String> states;

        public StateListAdapter(Activity context, SortedSet<String> states)
        {
            this.context = context;
            this.states = states;
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
            var view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.StateRow, parent, false);
                var _state = view.FindViewById<TextView>(Resource.Id.stateListViewItem);

                view.Tag = new StateListViewHolder { State = _state };
            }

            var holder = (StateListViewHolder)view.Tag;
            holder.State.Text = states.ElementAt(position);
            holder.State.SetTextColor(ColorScheme.MidnightBlue);

            return view;
        }
    }
}

