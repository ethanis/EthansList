using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using EthansList.Shared;

namespace EthansList.MaterialDroid
{
    public class CategoryListAdapter : BaseAdapter<CatTableGroup>
    {
        List<CatTableGroup> categories;
        LayoutInflater layoutInflater;

        public CategoryListAdapter(Context context, List<CatTableGroup> categories)
        {
            this.categories = categories;
            layoutInflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
        }

        public override CatTableGroup this[int position]
        {
            get
            {
                return categories[position];
            }
        }

        public override int Count
        {
            get
            {
                return categories.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            if (view == null)
            {
                view = layoutInflater.Inflate(Resource.Layout.CityRow, parent, false);
                var _city = view.FindViewById<TextView>(Resource.Id.cityListViewItem);

                view.Tag = new CityListViewHolder { City = _city };
            }

            var holder = (CityListViewHolder)view.Tag;

            holder.City.Text = categories[position].Name;

            return view;
        }
    }
}

