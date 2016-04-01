
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
    public class CategoryGroupRow : LinearLayout
    {
        readonly Context context;
        public TextView headerLabel { get; set; }
        public event EventHandler<CategorySelectedEventArgs> CategorySelected;
        public event EventHandler<CategorySelectedEventArgs> CategoryLongClick;
        private int rowHeight;

        public List<KeyValuePair<string, string>> Items
        {
            set
            {
                items = value;
                AddSubCategories();
            }
        }
        protected List<KeyValuePair<string, string>> items;

        public CategoryGroupRow(Context context) :
            base(context)
        {
            this.context = context;
            rowHeight = PixelConverter.DpToPixels(context.Resources.GetInteger(Resource.Integer.textLabelRowHeight));
            Initialize();
        }

        public CategoryGroupRow(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public CategoryGroupRow(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        void Initialize()
        {
            ViewGroup.LayoutParams p = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            this.Orientation = Orientation.Vertical;

            LinearLayout row = new LinearLayout(context);
            row.Orientation = Orientation.Horizontal;
            row.LayoutParameters = p;
            row.SetBackgroundColor(Android.Graphics.Color.LightGray);

            headerLabel = new TextView(context);
            headerLabel.LayoutParameters = p;
            //headerLabel.SetTextSize(ComplexUnitType.Dip, 20);

            headerLabel.Gravity = GravityFlags.CenterVertical;
            headerLabel.SetTextSize(Android.Util.ComplexUnitType.Px, rowHeight * 0.50f);
            headerLabel.SetPadding((int)(rowHeight * 0.1), (int)(rowHeight * 0.15), (int)(rowHeight * 0.1), (int)(rowHeight * 0.15));

            row.AddView(headerLabel);

            AddView(row);
        }

        void AddSubCategories()
        {
            ViewGroup.LayoutParams p = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);

            var index = 0;
            foreach (var item in items)
            {
                LinearLayout row = new LinearLayout(context);
                row.Orientation = Orientation.Horizontal;
                row.LayoutParameters = p;

                TextView subCategory = new TextView(context);
                subCategory.LayoutParameters = p;

                subCategory.Gravity = GravityFlags.CenterVertical;
                subCategory.SetTextSize(Android.Util.ComplexUnitType.Px, rowHeight * 0.40f);
                subCategory.SetPadding((int)(rowHeight * 0.1), (int)(rowHeight * 0.15), (int)(rowHeight * 0.1), (int)(rowHeight * 0.15));

                subCategory.Text = item.Value;
                row.AddView(subCategory);

                row.Click += (object sender, EventArgs e) =>
                {
                    row.SetBackgroundResource(Android.Resource.Color.HoloBlueLight);
                    if (this.CategorySelected != null)
                        this.CategorySelected(this, new CategorySelectedEventArgs { Selected = item });
                };

                row.LongClick += (sender, e) =>
                {
                    if (this.CategoryLongClick != null)
                        CategoryLongClick(this, new CategorySelectedEventArgs { Selected = item });
                };

                AddView(row);
                index++;
            }
        }
    }

    public class CategorySelectedEventArgs : EventArgs
    {
        public KeyValuePair<string, string> Selected { get; set; }
    }
}

