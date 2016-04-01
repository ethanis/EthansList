
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using EthansList.Shared;

namespace EthansList.Droid
{
    public class FavoriteCategoriesFragment : Android.Support.V4.App.Fragment
    {
        public Location SelectedLocation { get; set; }
        List<FavoriteCategory> favorites;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = new ListView(this.Activity);
            favorites = (MainActivity.databaseConnection.GetAllFavoriteCategoriesAsync().
                             Result).
                                    OrderByDescending(x => x.Updated).ToList();


            view.Adapter = new ArrayAdapter<string>(this.Activity,
                                                    Android.Resource.Layout.SimpleListItem1,
                                                    favorites.Select(x => x.CategoryValue).ToList());

            view.ItemLongClick += (sender, e) =>
            {
                PopupMenu menu = new PopupMenu(this.Activity, view.GetChildAt(e.Position));
                menu.Inflate(Resource.Menu.UnfavoriteMenu);
                menu.Show();

                menu.MenuItemClick += (se, args) =>
                {
                    var result = MainActivity.databaseConnection.DeleteFavoriteCategoryAsync(favorites.ElementAt(e.Position)).Result;
                    if (MainActivity.databaseConnection.StatusCode == Models.codes.ok && result)
                    {
                        lock (favorites)
                        {
                            Activity.RunOnUiThread(() =>
                            {
                                favorites.RemoveAt(e.Position);
                                view.Adapter = new ArrayAdapter<string>(this.Activity,
                                        Android.Resource.Layout.SimpleListItem1,
                                                                        favorites.Select(x => x.CategoryValue).ToList());
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

            view.ItemClick += (sender, e) =>
            {
                var transaction = this.Activity.SupportFragmentManager.BeginTransaction();
                SearchOptionsFragment searchFragment = new SearchOptionsFragment();
                var cat = favorites.ElementAt(e.Position);
                searchFragment.Category = new KeyValuePair<string, string>(cat.CategoryKey, cat.CategoryValue);
                searchFragment.SearchLocation = this.SelectedLocation;

                transaction.Replace(Resource.Id.frameLayout, searchFragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };

            return view;
        }
    }
}

