using System;
using Android.Widget;
using Android.Content;
using System.Collections.Generic;

namespace EthansList.Droid
{
    public class ImageAdapter : BaseAdapter
    {
        Context context;
        List<string> urls;

        public ImageAdapter(Context context, List<string> urls)
        {
            this.context = context;
            this.urls = urls;
        }

        public override int Count
        {
            get
            {
                return urls.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            ImageView imageView;

            //TODO: Set layoutparams to use dip instead
            if (convertView == null)
            {
                imageView = new ImageView(context);
                imageView.LayoutParameters = new GridView.LayoutParams(PixelConverter.DpToPixels(150), PixelConverter.DpToPixels(150));
                imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
                imageView.SetPadding(1, 1, 1, 1);
            }
            else
            {
                imageView = (ImageView)convertView;
            }

            Koush.UrlImageViewHelper.SetUrlDrawable(imageView, urls[position], Resource.Drawable.placeholder);

            return imageView;
        }
    }
}

