
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace EthansList.Droid
{
    public class FavoriteCategoriesFragment : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = new ListView(this.Activity);
            var favorites = MainActivity.databaseConnection.GetAllFavoriteCategoriesAsync().Result;

            view.Adapter = new ArrayAdapter<string>(this.Activity,
                                                    Android.Resource.Layout.SimpleListItem1,
                                                    favorites.Select(x => x.CategoryKey).ToList());

            return view;
        }
    }
}

