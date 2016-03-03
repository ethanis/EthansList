using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using EthansList.Models;
using Java.Lang;

namespace EthansList.MaterialDroid
{
    class FeedResultsAdapter : BaseAdapter<Posting>
    {
        Activity activity;
        List<Posting> postings;

        public FeedResultsAdapter(Activity activity, List<Posting> postings)
        {
            this.activity = activity;
            this.postings = postings;
        }

        public override Posting this[int position]
        {
            get
            {
                return postings[position];
            }
        }

        public override int Count
        {
            get
            {
                return postings.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            return new TextView(activity) { Text = postings[position].PostTitle };
        }
    }
}