
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

namespace ethanslist.android
{
    public class SavedSearchesFragment : Fragment
    {
        List<String> savedSearches = new List<String>();
        ListView savedSearchesListView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.SavedSearches, container, false);

            savedSearches.Add("search one");
            savedSearches.Add("search two");

            ArrayAdapter adapter = new ArrayAdapter(this.Activity, Android.Resource.Layout.SimpleListItem1, savedSearches);

            savedSearchesListView = view.FindViewById<ListView>(Resource.Id.savedSearchListView);
            savedSearchesListView.Adapter = adapter;
    
            return view;
        }
    }
}

