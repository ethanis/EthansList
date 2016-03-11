
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class SelectCityView : LinearLayout
    {
        ListView city_picker, state_picker;
        Context context;
        CityListAdapter cityAdapter;
        protected string state;


        public SelectCityView(Context context) :
            base(context)
        {
            this.context = context;
            Initialize();
        }

        public SelectCityView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            this.context = context;
            Initialize();
        }

        public SelectCityView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            this.context = context;
            Initialize();
        }

        void Initialize()
        {
            this.WeightSum = 1;
            this.Orientation = Orientation.Horizontal;
            LayoutParams p = new LayoutParams(0, ViewGroup.LayoutParams.MatchParent, 0.5f);
            AvailableLocations locations = new AvailableLocations();
            state = locations.States.ElementAt(0);

            state_picker = new ListView(context);
            state_picker.LayoutParameters = p;
            state_picker.Adapter = new StateListAdapter(context, locations.States);
            AddView(state_picker);

            city_picker = new ListView(context);
            city_picker.LayoutParameters = p;
            cityAdapter = new CityListAdapter(context, locations.PotentialLocations.Where(loc => loc.State == state));
            city_picker.Adapter = cityAdapter;
            AddView(city_picker);

            state_picker.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
                state = locations.States.ElementAt(e.Position);
                cityAdapter.Cities = locations.PotentialLocations.Where(l => l.State.Equals(state));
                cityAdapter.NotifyDataSetChanged();
            };

            city_picker.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
            {
                Location selected = locations.PotentialLocations.Where(loc => loc.State == state).ElementAt(e.Position);

                var transaction = ((AppCompatActivity)context).SupportFragmentManager.BeginTransaction();
                CategoryPickerFragment categoryFragment = new CategoryPickerFragment();
                categoryFragment.SelectedLocation = selected;
                transaction.Replace(Resource.Id.frameLayout, categoryFragment)
                           .AddToBackStack(null)
                           .Commit();

                Task.Run(async () =>
                {
                    await MainActivity.databaseConnection.AddNewRecentCityAsync(selected.SiteName, selected.Url).ConfigureAwait(true);

                    if (MainActivity.databaseConnection.GetAllRecentCitiesAsync().Result.Count > 5)
                        await MainActivity.databaseConnection.DeleteOldestCityAsync().ConfigureAwait(true);
                });
            };
        }
    }
}

