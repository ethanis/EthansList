using System;
using Android.Content;
using Android.Net;
using EthansList.Droid;

namespace Reachability
{
    public static class Reachability
    {
        static Context activity = EthansList.Droid.MainActivity.Instance;

        public static bool IsNetworkAvailable()
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)activity.GetSystemService(Android.Content.Context.ConnectivityService);
            NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;

            return (activeConnection != null) && activeConnection.IsConnected;
        }
    }
}

