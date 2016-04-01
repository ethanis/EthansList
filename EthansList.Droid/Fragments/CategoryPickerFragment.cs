using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
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

