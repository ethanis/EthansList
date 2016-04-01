
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
        ArrayAdapter<string> adapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = new ListView(this.Activity);
            var favorites = (MainActivity.databaseConnection.GetAllFavoriteCategoriesAsync().
                             Result).
                                    OrderByDescending(x => x.Updated).
                                    Select(x => x.CategoryValue).
                                    ToList();

            view.Adapter = adapter = new ArrayAdapter<string>(this.Activity,
                                                    Android.Resource.Layout.SimpleListItem1,
                                                    favorites);

            view.ItemLongClick += (sender, e) =>
            {
                PopupMenu menu = new PopupMenu(this.Activity, view.GetChildAt(e.Position));
                menu.Inflate(Resource.Menu.UnfavoriteMenu);
                menu.Show();

                menu.MenuItemClick += (se, args) =>
                {
                    var result = MainActivity.databaseConnection.DeleteFavoriteCategoryAsync(favorites.ElementAt(e.Position), true).Result;
                    if (MainActivity.databaseConnection.StatusCode == Models.codes.ok && result)
                    {
                        lock (favorites)
                        {
                            Activity.RunOnUiThread(() =>
                            {
                                favorites.RemoveAt(e.Position);
                                view.Adapter = adapter = new ArrayAdapter<string>(this.Activity,
                                        Android.Resource.Layout.SimpleListItem1,
                                        favorites);
                            });
                        }
                    }
                    else
                    {
                        Toast.MakeText(this.Activity, "Something went wrong. We're sorry!", ToastLength.Short).Show();
                    }
                    Console.WriteLine(MainActivity.databaseConnection.StatusMessage);
                };
            };

            return view;
        }
    }
}

