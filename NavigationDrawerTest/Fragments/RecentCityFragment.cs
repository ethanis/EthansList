
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
using EthansList.Models;
using EthansList.Shared;

namespace EthansList.MaterialDroid
{
    public class RecentCityFragment : Android.Support.V4.App.Fragment
    {
        private List<RecentCity> recentCities;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = new ListView(this.Activity);

            recentCities = MainActivity.databaseConnection.GetAllRecentCitiesAsync().Result;
            recentCities.Sort((s1, s2)=>s2.Updated.CompareTo(s1.Updated));

            view.Adapter = new RecentCityAdapter(this.Activity, recentCities);

            view.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => { 
                var transaction = this.Activity.SupportFragmentManager.BeginTransaction();
                CategoryPickerFragment categoryFragment = new CategoryPickerFragment();

                AvailableLocations allLocations = new AvailableLocations();
                categoryFragment.SelectedLocation = allLocations.PotentialLocations.Find(x => x.SiteName == recentCities[e.Position].City);

                transaction.Replace(Resource.Id.frameLayout, categoryFragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };

            return view;
        }
    }

    public class RecentCityAdapter : BaseAdapter<RecentCity>
    {
        readonly List<RecentCity> _recentCities;
        readonly Context _context;
        private int rowHeight;

        public RecentCityAdapter(Context context, List<RecentCity> recentCities)
        {
            _context = context;
            _recentCities = recentCities;
            rowHeight = ConvertDpToPx(_context.Resources.GetInteger(Resource.Integer.textLabelRowHeight));
        }

        public override RecentCity this[int position]
        {
            get
            {
                return _recentCities[position];
            }
        }

        public override int Count
        {
            get
            {
                return _recentCities.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = (TextView)convertView;
            if (view == null)
            {
                view = new TextView(_context);

                view.Gravity = GravityFlags.CenterVertical;
                view.SetTextSize(Android.Util.ComplexUnitType.Px, rowHeight * 0.40f);
                view.SetPadding((int)(rowHeight * 0.1), (int)(rowHeight * 0.15), (int)(rowHeight * 0.1), (int)(rowHeight * 0.15));

            }
            view.Text = _recentCities[position].City;

            return view;
        }

        private int ConvertDpToPx(float dip)
        {
            return (int)(dip * _context.Resources.DisplayMetrics.Density);
        }
    }
}

