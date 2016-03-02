using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace EthansList.MaterialDroid
{
    public class NumberPickerDialogFragment : DialogFragment
    {
        private readonly Context _context;
        private readonly int _min, _max, _current, _step;
        private readonly NumberPicker.IOnValueChangeListener _listener;
        private readonly string _title, _suffix;

        public NumberPickerDialogFragment(Context context, int min, int max, int current, int step, string title, string suffix, NumberPicker.IOnValueChangeListener listener)
        {
            _context = context;
            _min = min;
            _max = max;
            _current = current;
            _step = step;
            _listener = listener;
            _title = title;
            _suffix = suffix;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
            var view = inflater.Inflate(Resource.Layout.NumberPickerDialog, null);
            var numberPicker = view.FindViewById<NumberPicker>(Resource.Id.numberPicker);
            numberPicker.MaxValue = _max;
            numberPicker.MinValue = _min;
            numberPicker.Value = _current;

            List<string> values = new List<string>();
            for (var i = _min; i <= _max; i += 1)
            {
                values.Add((i*_step) + _suffix);
            }
            numberPicker.SetDisplayedValues(values.ToArray());
            numberPicker.SetOnValueChangedListener(_listener);

            var dialog = new Android.Support.V7.App.AlertDialog.Builder(_context);
            dialog.SetTitle(_title);
            dialog.SetView(view);
            dialog.SetNegativeButton("Cancel", (s, a) => { });
            dialog.SetPositiveButton("Ok", (s, a) => { });
            return dialog.Create();
        }
    }
}

