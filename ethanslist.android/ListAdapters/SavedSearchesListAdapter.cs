using System;
using Android.Widget;
using EthansList.Models;
using Android.App;
using System.Collections.Generic;

namespace ethanslist.android
{
    public class SavedSearchesListAdapter : BaseAdapter<Search>
    {
        List<Search> savedSearches;
        Activity context;

        public SavedSearchesListAdapter(Activity context, List<Search> savedSearches)
        {
            this.context = context;
            this.savedSearches = savedSearches;
        }

        public override Search this[int index]
        {
            get
            {
                return savedSearches[index];
            }
        }

        public override int Count
        {
            get
            {
                return savedSearches.Count;
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
                view = context.LayoutInflater.Inflate(Resource.Layout.SavedSearchRow, parent, false);
                var _searchCity = view.FindViewById<TextView>(Resource.Id.searchURLText);
                var _minPrice = view.FindViewById<TextView>(Resource.Id.searchMinPriceText);

                view.Tag = new SavedSearchesViewHolder { SearchCity = _searchCity, MinPriceText = _minPrice };
            }

            var holder = (SavedSearchesViewHolder)view.Tag;
            holder.SearchCity.Text = savedSearches[position].CityName;
            holder.MinPriceText.Text = savedSearches[position].MinPrice;

            return view;
        }
    }
}

