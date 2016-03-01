
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
    public class CategoryGroupRow : LinearLayout
    {
        readonly Context context;
        public TextView headerLabel { get; set; }
        public event EventHandler<CategorySelectedEventArgs> CategorySelected;

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
            headerLabel.SetTextSize(ComplexUnitType.Dip, 20);

            row.AddView(headerLabel);

            AddView(row);
        }

        void AddSubCategories()
        {
            ViewGroup.LayoutParams p = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);

            foreach (var item in items)
            {
                LinearLayout row = new LinearLayout(context);
                row.Orientation = Orientation.Horizontal;
                row.LayoutParameters = p;

                TextView subCategory = new TextView(context);
                subCategory.LayoutParameters = p;
                subCategory.SetTextSize(ComplexUnitType.Dip, 16);
                subCategory.Text = item.Value;
                row.AddView(subCategory);

                //TODO: Add selected view to row to show that it was clicked
                row.Click += (object sender, EventArgs e) => 
                { 
                    if (this.CategorySelected != null)
                        this.CategorySelected(this, new CategorySelectedEventArgs { Selected = item });
                };

                AddView(row);
            }
        }
    }

    public class CategorySelectedEventArgs : EventArgs
    {
        public KeyValuePair<string, string> Selected { get; set; }
    }
}

