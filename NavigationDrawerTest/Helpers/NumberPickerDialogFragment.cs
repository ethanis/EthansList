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
        private readonly NumberPicker.IOnValueChangeListener _listener;
        private readonly string _title, _callerKey;
        private readonly NumberPickerOptions _options;

        public event EventHandler<NumberPickerValueChanged> NumberChanged;

        public NumberPickerDialogFragment(Context context, string title, NumberPickerOptions options, string callerKey)
        {
            _context = context;
            _title = title;
            _options = options;
            _callerKey = callerKey;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
            var view = inflater.Inflate(Resource.Layout.NumberPickerDialog, null);
            var numberPicker = view.FindViewById<NumberPicker>(Resource.Id.numberPicker);
            numberPicker.MaxValue = _options.Maximum;
            numberPicker.MinValue = _options.Minimum;
            numberPicker.Value = _options.Initial;

            List<string> values = new List<string>();
            for (var i = _options.Minimum; i <= _options.Maximum; i += 1)
            {
                values.Add((i*_options.Step) + _options.DisplaySuffix);
            }
            numberPicker.SetDisplayedValues(values.ToArray());

            var dialog = new Android.Support.V7.App.AlertDialog.Builder(_context);
            dialog.SetTitle(_title);
            dialog.SetView(view);
            dialog.SetNegativeButton("Cancel", (s, a) => { });
            dialog.SetPositiveButton("Ok", (s, a) => { });

            numberPicker.ValueChanged += (object sender, NumberPicker.ValueChangeEventArgs e) => {
                if (NumberChanged != null)
                    NumberChanged(this, new NumberPickerValueChanged() { CallerKey = _callerKey, Value = e.NewVal });
            };

            return dialog.Create();
        }

        public class NumberPickerValueChanged : EventArgs
        { 
            public string CallerKey { get; set; }
            public int Value { get; set; }
        }
    }
}

