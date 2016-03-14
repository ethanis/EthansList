using System;
using Android.Content;

namespace EthansList.Droid
{
    public static class PixelConverter
    {
        static Context activity = EthansList.Droid.MainActivity.Instance;

        public static int DpToPixels(float dip)
        {
            return (int)(dip * activity.Resources.DisplayMetrics.Density);
        }

        public static int PixelsToDp(float pixelValue)
        {
            return (int)((pixelValue) / activity.Resources.DisplayMetrics.Density);
        }
    }
}

