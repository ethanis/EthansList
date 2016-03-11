using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using EthansList.Shared;

namespace EthansList.MaterialDroid
{
    public class CategoryPickerFragment : Android.Support.V4.App.Fragment
    {
        public Location SelectedLocation { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = new CategoryPickerView(this.Activity, SelectedLocation);

            //var save_button = ((MainActivity)Activity).Menu.FindItem(Resource.Id.save_action_button);
            //save_button.SetVisible(false);

            return view;
        }
    }
}

