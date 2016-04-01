using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using EthansList.Shared;

namespace EthansList.Droid
{
    public class CategoryPickerFragment : Android.Support.V4.App.Fragment
    {
        public Location SelectedLocation { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = new CategoryPickerView(this.Activity, SelectedLocation);


            ((MainActivity)this.Activity).OptionItemSelected += (sender, e) =>
            {
                if (e.Item.TitleFormatted.ToString() == "Favorite")
                {
                    var transaction = Activity.SupportFragmentManager.BeginTransaction();
                    FavoriteCategoriesFragment favoriteFragment = new FavoriteCategoriesFragment();
                    favoriteFragment.SelectedLocation = SelectedLocation;

                    transaction.Replace(Resource.Id.frameLayout, favoriteFragment);
                    transaction.AddToBackStack(null);
                    transaction.Commit();
                }
            };


            view.CategoryLongClick += (sender, e) =>
            {
                var menu = new Android.Support.V7.Widget.PopupMenu(this.Activity, e.SelectedView);
                var presentAlready = MainActivity.databaseConnection.FavoriteCategoryAlreadyPresent(e.Selected.Key);

                if (presentAlready)
                    menu.Inflate(Resource.Menu.UnfavoriteMenu);
                else
                    menu.Inflate(Resource.Menu.FavoriteMenu);

                menu.Show();
                menu.MenuItemClick += async (s, ev) =>
                {
                    if (presentAlready)
                        await MainActivity.databaseConnection.DeleteFavoriteCategoryAsync(e.Selected.Key);
                    else
                        await MainActivity.databaseConnection.AddNewFavoriteCategoryAsync(e.Selected.Key, e.Selected.Value);

                    if (MainActivity.databaseConnection.StatusCode == Models.codes.ok)
                        Toast.MakeText(this.Activity, $"Successfully (un)favorited: {e.Selected.Value}", ToastLength.Short).Show();
                    else
                        Toast.MakeText(this.Activity, $"Something went wrong, we're sorry!", ToastLength.Short).Show();
                };
            };

            return view;
        }

        public override void OnPrepareOptionsMenu(IMenu menu)
        {
            IMenuItem fav_item = menu.FindItem(Resource.Id.favorite_action_button);
            fav_item.SetVisible(true);

            base.OnPrepareOptionsMenu(menu);
        }
    }
}

