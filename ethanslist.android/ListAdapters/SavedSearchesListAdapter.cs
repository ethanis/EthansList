using System;
using Android.Widget;
using EthansList.Models;
using Android.App;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ethanslist.android
{
    public class SavedSearchesListAdapter : BaseAdapter<Search>
    {
        List<Search> savedSearches;
//        ObservableCollection<Search> observedSearches;
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
                var _searchCity = view.FindViewById<TextView>(Resource.Id.searchCityText);
                var _searchInformation = view.FindViewById<TextView>(Resource.Id.searchInformationText);

                view.Tag = new SavedSearchesViewHolder { SearchCity = _searchCity, SearchInformation = _searchInformation };
            }
            Button delete = view.FindViewById<Button>(Resource.Id.deleteSearchButton);

            var holder = (SavedSearchesViewHolder)view.Tag;
            holder.SearchCity.Text = savedSearches[position].CityName;
            holder.SearchInformation.Text = MainActivity.databaseConnection.FormatSearchQuery(savedSearches[position]);

            delete.Click += async (object sender, EventArgs e) => {
                await MainActivity.databaseConnection.DeleteSearchAsync(savedSearches[position]);
                savedSearches.RemoveAt(position);

                this.NotifyDataSetChanged();
            };

            return view;
        }
    }
}

