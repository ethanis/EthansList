using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using EthansList.Shared;

namespace EthansList.MaterialDroid
{
    public class CategoryListAdapter : BaseAdapter<CatTableGroup>
    {
        readonly Context context;
        readonly List<CatTableGroup> categories;
        readonly Location SelectedLocation;

        public CategoryListAdapter(Context context, List<CatTableGroup> categories, Location searchLocation)
        {
            this.context = context;
            this.categories = categories;
            this.SelectedLocation = searchLocation;
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
                view = new CategoryGroupRow(context);
                ((CategoryGroupRow)view).Items = categories[position].Items;
            }

            ((CategoryGroupRow)view).headerLabel.Text = categories[position].Name;

            ((CategoryGroupRow)view).CategorySelected += (object sender, CategorySelectedEventArgs e) => 
            { 
                FragmentTransaction transaction = ((Activity)context).FragmentManager.BeginTransaction();
                SearchOptionsFragment searchFragment = new SearchOptionsFragment();
                searchFragment.Category = e.Selected;
                searchFragment.SearchLocation = this.SelectedLocation;

                transaction.Replace(Resource.Id.frameLayout, searchFragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            }; 

            return view;
        }
    }
}

