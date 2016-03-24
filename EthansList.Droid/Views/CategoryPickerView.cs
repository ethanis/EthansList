
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
using EthansList.Shared;

namespace EthansList.Droid
{
    public class CategoryPickerView : RelativeLayout
    {
        ListView categoryList;
        readonly Context context;
        public readonly Location SelectedLocation;

        public CategoryPickerView(Context context, Location location) :
            base(context)
        {
            this.context = context;
            this.SelectedLocation = location;
            Initialize();
        }

        public CategoryPickerView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            this.context = context;
            Initialize();
        }

        public CategoryPickerView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            this.context = context;
            Initialize();
        }

        void Initialize()
        {
            //TODO: categories groups disappear for bottom group
            categoryList = new ListView(context);
            categoryList.Adapter = new CategoryListAdapter(context, Categories.Groups, SelectedLocation);
            categoryList.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
            categoryList.Clickable = false;
            AddView(categoryList);
        }
    }
}

