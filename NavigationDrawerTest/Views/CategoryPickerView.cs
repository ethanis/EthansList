
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

namespace EthansList.MaterialDroid
{
    public class CategoryPickerView : RelativeLayout
    {
        ListView categoryList;
        Context context;

        public CategoryPickerView(Context context) :
            base(context)
        {
            this.context = context;
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
            categoryList = new ListView(context);
            categoryList.Adapter = new CategoryListAdapter(context, Categories.Groups);
            categoryList.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
            AddView(categoryList);
        }
    }
}

