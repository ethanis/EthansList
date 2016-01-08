using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Linq;
using System.Collections.Generic;
using EthansList.Shared;
//using Cirrious.FluentLayouts.Touch;

namespace ethanslist.ios
{
	partial class CityPickerViewController : UIViewController
	{
        AvailableLocations locations;
        String state;
        StatePickerModel stateModel;
        LocationPickerModel cityModel;
        Location currentSelected;

		public CityPickerViewController (IntPtr handle) : base (handle)
		{
		}

        public override void LoadView()
        {
            base.LoadView();

            FormatButtons(new UIButton[]{ProceedButton, RecentCitiesButton});

            AddLayoutConstraints();
            this.View.Layer.BackgroundColor = ColorScheme.Clouds.CGColor;
        }

        void FormatButtons(UIButton[] buttons)
        {
            foreach (UIButton button in buttons)
            {
                button.Layer.BackgroundColor = ColorScheme.MidnightBlue.CGColor;
                button.SetTitleColor(ColorScheme.Clouds, UIControlState.Normal);
                button.Layer.CornerRadius = 10;
                button.ClipsToBounds = true;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            locations = new AvailableLocations();
            state = locations.States.ElementAt((int)StatePickerView.SelectedRowInComponent(0));
            currentSelected = locations.PotentialLocations.Where(loc => loc.State == state).ElementAt(0);

            cityModel = new LocationPickerModel(locations, state);
            CityPickerView.Model = cityModel;

            stateModel = new StatePickerModel(locations);
            StatePickerView.Model = stateModel;

            cityModel.ValueChange += cityPickerChanged;

            ProceedButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                var storyboard = UIStoryboard.FromName("Main", null);
                var searchViewController = (SearchViewController)storyboard.InstantiateViewController("SearchViewController");

                Console.WriteLine(currentSelected.SiteName);
                searchViewController.Url = currentSelected.Url;
                searchViewController.City = currentSelected.SiteName;

                    System.Threading.Tasks.Task.Run(async () => {
                        await AppDelegate.databaseConnection.AddNewRecentCityAsync(currentSelected.SiteName, currentSelected.Url);

                        if (AppDelegate.databaseConnection.GetAllRecentCitiesAsync().Result.Count > 5)
                        {
                            await AppDelegate.databaseConnection.DeleteOldestCityAsync();
                        }
                    });

                this.ShowViewController(searchViewController, this);
            };

            RecentCitiesButton.TouchUpInside += (object sender, EventArgs e) => {
                var storyboard = UIStoryboard.FromName("Main", null);
                var recentCitiesViewController = (RecentCitiesTableViewController)storyboard.InstantiateViewController("RecentCitiesTableViewController");
                this.ShowViewController(recentCitiesViewController, this);
            };

            stateModel.ValueChanged += (object sender, EventArgs e) =>
            {
                state = stateModel.SelectedItem;
                CityPickerView.Select(0,0,false);

                currentSelected = locations.PotentialLocations.Where(loc => loc.State == state).ElementAt(0);
                cityModel = new LocationPickerModel(locations, stateModel.SelectedItem);
                CityPickerView.Model = cityModel;
                
                cityModel.ValueChange += cityPickerChanged;
            };
        }

        void cityPickerChanged (object sender, EventArgs e)
        {
            currentSelected = cityModel.SelectedCity;
        }

        public class StatePickerModel : UIPickerViewModel
        {
            public AvailableLocations locations;
            public event EventHandler<EventArgs> ValueChanged;
            protected int SelectedIndex = 0;

            public StatePickerModel(AvailableLocations locations)
            {
                this.locations = locations;
            }

            public String SelectedItem {
                get { return locations.States.ElementAt(SelectedIndex);}
            }

            public override nint GetComponentCount(UIPickerView pickerView)
            {
                return 1;
            }

            public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
            {
                return locations.States.Count;
            }

            public override string GetTitle(UIPickerView pickerView, nint row, nint component)
            {
                return locations.States.ElementAt((int)row);
            }

            public override void Selected(UIPickerView pickerView, nint row, nint component)
            {
                SelectedIndex = (int)row;
                if (this.ValueChanged != null)
                {
                    this.ValueChanged(this, new EventArgs());
                }
            }
        }

        public class LocationPickerModel : UIPickerViewModel
        {
            AvailableLocations locations;
            public event EventHandler<EventArgs> ValueChange;
            protected int SelectedIndex = 0;
            String state;

            public Location SelectedCity 
            {   get { return locations.PotentialLocations.Where(loc => loc.State == state).ElementAt(SelectedIndex); } 
            }

            public LocationPickerModel(AvailableLocations locations, string state)
            {
                this.locations = locations;
                this.state = state;
            }

            public override nint GetComponentCount(UIPickerView pickerView)
            {
                return 1;
            }

            public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
            {
                return locations.PotentialLocations.Where(loc => loc.State == state).Count();
            }

            public override string GetTitle(UIPickerView pickerView, nint row, nint component)
            {
                return locations.PotentialLocations.Where(loc => loc.State == state).ElementAt((int)row).SiteName;
            }

            public override void Selected(UIPickerView pickerView, nint row, nint component)
            {
                SelectedIndex = (int)row;
                if (this.ValueChange != null)
                {
                    this.ValueChange(this, new EventArgs());
                } 
            }
        }

        void AddLayoutConstraints()
        {
            this.View.RemoveConstraints(constraints: this.View.Constraints);
            StatePickerView.TranslatesAutoresizingMaskIntoConstraints = false;
            CityPickerView.TranslatesAutoresizingMaskIntoConstraints = false;
            ProceedButton.TranslatesAutoresizingMaskIntoConstraints = false;

            List<NSLayoutConstraint> stateConstraints = new List<NSLayoutConstraint>();
            //State picker view constraints
            stateConstraints.Add(NSLayoutConstraint.Create(StatePickerView, NSLayoutAttribute.Left, 
                NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Left, 1, 0));
            stateConstraints.Add(NSLayoutConstraint.Create(StatePickerView, NSLayoutAttribute.Top, 
                NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Top, 1, 20));
            stateConstraints.Add(NSLayoutConstraint.Create(this.StatePickerView, NSLayoutAttribute.Width, 
                NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, 0.5f, 0));
            stateConstraints.Add(NSLayoutConstraint.Create(StatePickerView, NSLayoutAttribute.Height,
                NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Height, 0.75f, 0));
            this.View.AddConstraints(stateConstraints.ToArray());

            List<NSLayoutConstraint> cityConstraints = new List<NSLayoutConstraint>();
            //City picker view constraints
            cityConstraints.Add(NSLayoutConstraint.Create(CityPickerView, NSLayoutAttribute.Left, 
                NSLayoutRelation.Equal, StatePickerView, NSLayoutAttribute.Right, 1, 0));
            cityConstraints.Add(NSLayoutConstraint.Create(CityPickerView, NSLayoutAttribute.Right, 
                NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Right, 1, 0));
            cityConstraints.Add(NSLayoutConstraint.Create(CityPickerView, NSLayoutAttribute.Top, 
                NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Top, 1, 20));
            cityConstraints.Add(NSLayoutConstraint.Create(CityPickerView, NSLayoutAttribute.Height,
                NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Height, 0.75f, 0));

            this.View.AddConstraints(cityConstraints.ToArray());

            //Proceed Button View Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(ProceedButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .90f, 0),
                NSLayoutConstraint.Create(ProceedButton, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(ProceedButton, NSLayoutAttribute.Top, NSLayoutRelation.Equal, CityPickerView, NSLayoutAttribute.Bottom, 1, 20),
            });
            //Recent Cities Button View Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(RecentCitiesButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .90f, 0),
                NSLayoutConstraint.Create(RecentCitiesButton, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(RecentCitiesButton, NSLayoutAttribute.Top, NSLayoutRelation.Equal, ProceedButton, NSLayoutAttribute.Bottom, 1, 5),
            });
            this.View.LayoutIfNeeded();
        }
	}
}