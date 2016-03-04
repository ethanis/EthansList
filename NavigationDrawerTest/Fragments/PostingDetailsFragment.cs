using System;
using Android.App;
using Android.Content;
using Android.Widget;
using EthansList.Models;

namespace EthansList.MaterialDroid
{
    public class PostingDetailsFragment : Fragment
    {
        public Posting Posting { get; set; }

        public override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            return new TextView(this.Activity) { Text = Posting.PostTitle };
        }
    }

    public class PostingDetailsView : LinearLayout
    {
        readonly Context _context;

        public TextView PostingTitle { get; set; }
        public ImageView PostingImage { get; set; }
        public GridView ImageCollection { get; set; }
        public TextView PostingDescription { get; set; }
        public TextView PostingDate { get; set; }

        //todo: posting map and weblink

        public PostingDetailsView(Context context)
            :base (context)
        {
            _context = context;
            Initialize();
        }

        void Initialize()
        {
            Orientation = Orientation.Vertical;
            WeightSum = 1;


        }
    }
}

