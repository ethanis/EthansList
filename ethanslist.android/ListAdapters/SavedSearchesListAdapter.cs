using System;
using Android.Widget;
using EthansList.Models;
using Android.App;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using EthansList.Shared;

namespace ethanslist.android
{
    public class SavedSearchesListAdapter : BaseAdapter<SearchObject>
    {
        List<SearchObject> savedSearches;
        Activity context;

        public SavedSearchesListAdapter(Activity context, List<SearchObject> savedSearches)
        {
            this.context = context;
            this.savedSearches = savedSearches;
        }

        public override SearchObject this[int index]
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
            var item = savedSearches[position];
            var view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.SavedSearchRow, parent, false);
                var _searchCity = view.FindViewById<TextView>(Resource.Id.searchCityText);
                var _searchInformation = view.FindViewById<TextView>(Resource.Id.searchInformationText);
                var _deleteBtn = view.FindViewById<Button>(Resource.Id.deleteSearchButton);

                _deleteBtn.Click += async (sender, e) => {
                    await MainActivity.databaseConnection.DeleteSearchAsync(item.SearchLocation.Url, item);
                    savedSearches.RemoveAt(position);
                    if (MainActivity.databaseConnection.StatusCode == codes.ok)
                        Toast.MakeText(this.context, "Search removed successfully",ToastLength.Short).Show();
                    else
                        Toast.MakeText(this.context, "Unable to remove search, please try again", ToastLength.Short).Show();
                    this.NotifyDataSetChanged();
                };

                view.Tag = new SavedSearchesViewHolder { SearchCity = _searchCity, SearchInformation = _searchInformation, DeleteBtn = _deleteBtn };
            }

            var holder = (SavedSearchesViewHolder)view.Tag;
            holder.SearchCity.Text = item.SearchLocation != null ? item.SearchLocation.SiteName : "null name";
            holder.SearchInformation.Text = MainActivity.databaseConnection.SecondFormatSearch(item);
           
            return view;
        }
    }
}

