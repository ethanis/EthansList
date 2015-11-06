using System;
using Android.Widget;
using Android.App;
using System.Collections.Generic;
using System.Linq;
using EthansList.Shared;

namespace ethanslist.android
{
    public class PostingListAdapter : BaseAdapter<Posting>
    {
        Activity context;
        List<Posting> postings;

        public PostingListAdapter(Activity context, List<Posting> postings)
        {
            this.context = context;
            this.postings = postings;
        }

        public override Posting this[int index]
        {
            get
            {
                return postings[index];
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

        public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            var view = convertView;
            if (convertView == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.FeedResultsRow, parent, false);
                var _posting = view.FindViewById<TextView>(Resource.Id.feedListViewItem);

                view.Tag = new PostingListViewHolder { Posting = _posting };
            }

            var holder = (PostingListViewHolder)view.Tag;
            holder.Posting.Text = postings[position].Title;

            return view;
        }
    }
}

