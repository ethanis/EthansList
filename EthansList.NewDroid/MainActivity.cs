using Android.App;
using Android.Widget;
using Android.OS;
using System;

using AlertDialog = Android.Support.V7.App.AlertDialog;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;

namespace EthansList.NewDroid
{
    [Activity(Label = "EthansList.Android", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : AppCompatActivity
    {
        int count = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Button button = FindViewById<Button>(Resource.Id.myButton);
            button.Click += delegate 
            { 
                var builder = new AlertDialog.Builder (this);

                builder.SetTitle ("Hello Dialog")
                .SetMessage ("Is this material design?")
                .SetPositiveButton ("Yes", delegate { Console.WriteLine("Yes"); })
                .SetNegativeButton ("No", delegate { Console.WriteLine("No"); }); 

                builder.Create().Show ();
            };

            SupportActionBar.Title = "Hello from Appcompat Toolbar";
        }
    }
}


