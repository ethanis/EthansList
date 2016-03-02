using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace EthansList.MaterialDroid
{

    public class ComboPickerAdapter : BaseAdapter<string>
    {
        readonly string[] _options;
        readonly Context _context;

        public ComboPickerAdapter(Context context, string[] options)
        {
            _context = context;
            _options = options;
        }

        public override string this[int position]
        {
            get
            {
                return _options[position];
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get
            {
                return _options.Length;
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            return new TextView(_context) { Text = _options[position] };
        }
    }
}
