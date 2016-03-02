﻿using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace EthansList.MaterialDroid
{
    public class ComboPickerDialogFragment : DialogFragment
    {
        readonly Context _context;
        readonly string _title;
        readonly List<KeyValuePair<object,object>> _options;
        public event EventHandler<ComboPickerItemChangedEventArgs> ItemChanged;
        public List<string> _selectedKeys { get; set; }

        public ComboPickerDialogFragment(Context context, string title, List<KeyValuePair<object,object>> options)
        {
            _context = context;
            _title = title;
            _options = options;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var p = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            var view = new LinearLayout(_context);
            view.LayoutParameters = p;

            var list = new ListView(_context);
            list.LayoutParameters = p;
            var adapter = new ComboPickerAdapter(_context, _options, _selectedKeys);
            list.Adapter = adapter;
            view.AddView(list);

            adapter.CheckedChanged += (object sender, ComboPickerItemChangedEventArgs e) => {
                if (this.ItemChanged != null)
                    this.ItemChanged(this, e);
            };

            var dialog = new Android.Support.V7.App.AlertDialog.Builder(_context);
            dialog.SetTitle(_title);
            dialog.SetView(view);
            dialog.SetNegativeButton("Cancel", (s, a) => { });
            dialog.SetPositiveButton("Ok", (s, a) => { });
            return dialog.Create();
        }
    }

    
}

