using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Linq;
using System.Collections.Generic;
using EthansList.Shared;
using CoreGraphics;

namespace ethanslist.ios
{
	partial class CityPickerViewController : UIViewController
	{
        AvailableLocations locations;
        public String state {get;set;}
        StatePickerModel stateModel;
        LocationPickerModel cityModel;
        Location currentSelected;
        UIPickerView StatePickerView, CityPickerView;
        UITableView StateTableView, CityTableView;
        StateTableSource stateTableSource;
        CityTableSource cityTableSource;
        private static double MinOSForPicker = 10.0;

		public CityPickerViewController (IntPtr handle) : base (handle)
		{
		}

        public override void LoadView()
        {
            base.LoadView();

            FormatButton(ProceedButton, ColorScheme.MidnightBlue);
            FormatButton(RecentCitiesButton, ColorScheme.Concrete);

            if (Convert.ToDouble(UIDevice.CurrentDevice.SystemVersion.Substring(0,1)) >= MinOSForPicker)
            {
                StatePickerView = new UIPickerView();
                CityPickerView = new UIPickerView();
                this.View.AddSubviews(new UIView[] { StatePickerView, CityPickerView });

                AddiOS9LayoutConstraints();
            }
            else
            {
                StateTableView = new UITableView();
                CityTableView = new UITableView();
                StateTableView.ShowsVerticalScrollIndicator = false;
                CityTableView.ShowsVerticalScrollIndicator = false;
                StateTableView.BackgroundColor = ColorScheme.Clouds;
                CityTableView.BackgroundColor = ColorScheme.Clouds;
                StateTableView.AccessibilityIdentifier = "StatePickTableView";
                CityTableView.AccessibilityIdentifier = "CityPickTableView";

                this.View.AddSubviews(new UIView[] {StateTableView, CityTableView });

                AddiOS8LayoutConstraints();
            }

            this.View.Layer.BackgroundColor = ColorScheme.Clouds.CGColor;
        }

        void FormatButton(UIButton button, UIColor background)
        {
            button.Layer.BackgroundColor = background.CGColor;
            button.SetTitleColor(ColorScheme.Clouds, UIControlState.Normal);
            button.Layer.CornerRadius = 10;
            button.ClipsToBounds = true;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.SetLeftBarButtonItem(
                new UIBarButtonItem(UIImage.FromBundle("menu.png"), UIBarButtonItemStyle.Plain, (s, e) => NavigationController.PopViewController(true)), 
                true);
            
            locations = new AvailableLocations();

            #region iOS 9.0 logic
            Console.WriteLine (UIDevice.CurrentDevice.SystemVersion.Substring(0,1));
            if (Convert.ToDouble(UIDevice.CurrentDevice.SystemVersion.Substring(0,1)) >= MinOSForPicker)
            {
                state = locations.States.ElementAt((int)StatePickerView.SelectedRowInComponent(0));
                currentSelected = locations.PotentialLocations.Where(loc => loc.State == state).ElementAt(0);

                cityModel = new LocationPickerModel(locations, state);
                CityPickerView.Model = cityModel;

                stateModel = new StatePickerModel(locations);
                StatePickerView.Model = stateModel;

                cityModel.ValueChange += cityPickerChanged;

                stateModel.ValueChanged += (object sender, EventArgs e) =>
                {
                    state = stateModel.SelectedItem;
                    CityPickerView.Select(0, 0, false);

                    currentSelected = locations.PotentialLocations.Where(loc => loc.State == state).ElementAt(0);
                    cityModel = new LocationPickerModel(locations, stateModel.SelectedItem);
                    CityPickerView.Model = cityModel;

                    cityModel.ValueChange += cityPickerChanged;
                };
            }
            #endregion
            #region iOS8_And_Lower
            else
            {
                state = locations.States.ElementAt(0);
                currentSelected = locations.PotentialLocations.Where(loc => loc.State == state).ElementAt(0);

                stateTableSource = new StateTableSource(locations);
                cityTableSource = new CityTableSource(locations, locations.States.ElementAt(0));
                StateTableView.Source = stateTableSource;
                CityTableView.Source = cityTableSource;

                stateTableSource.ValueChanged += StateTable_Changed;
                cityTableSource.ValueChange += CityTable_Changed;
            }
            #endregion

            ProceedButton.TouchUpInside += ProceedToSearch;

            RecentCitiesButton.TouchUpInside += (object sender, EventArgs e) =>
                {
                    var storyboard = UIStoryboard.FromName("Main", null);
                    var recentCitiesViewController = (RecentCitiesTableViewController)storyboard.InstantiateViewController("RecentCitiesTableViewController");
                    recentCitiesViewController.FromMenu = false;
                    this.ShowViewController(recentCitiesViewController, this);
                };
        }

        void ProceedToSearch (object sender, EventArgs e)
        {
            var storyboard = UIStoryboard.FromName("Main", null);
            var searchViewController = (SearchOptionsViewController)storyboard.InstantiateViewController("SearchOptionsViewController");

            Console.WriteLine(currentSelected.SiteName);
            searchViewController.Location = currentSelected;

            System.Threading.Tasks.Task.Run(async () =>
                {
                    await AppDelegate.databaseConnection.AddNewRecentCityAsync(currentSelected.SiteName, currentSelected.Url);

                    if (AppDelegate.databaseConnection.GetAllRecentCitiesAsync().Result.Count > 5)
                    {
                        await AppDelegate.databaseConnection.DeleteOldestCityAsync();
                    }
                });

            this.ShowViewController(searchViewController, this);
        }

        void StateTable_Changed (object sender, EventArgs e)
        {
            state = stateTableSource.SelectedItem;
            cityTableSource = new CityTableSource(locations, state);
            CityTableView.Source = cityTableSource;
            CityTableView.ReloadData();
            Console.WriteLine (state);
            cityTableSource.ValueChange += CityTable_Changed;
        }

        void CityTable_Changed (object sender, EventArgs e)
        {
            currentSelected = cityTableSource.SelectedCity;
            Console.WriteLine(currentSelected.SiteName);
            cityTableSource.ValueChange += CityTable_Changed;
            ProceedToSearch(sender, e);
        }

        void cityPickerChanged (object sender, EventArgs e)
        {
            currentSelected = cityModel.SelectedCity;
        }

        void AddiOS9LayoutConstraints()
        {
            this.View.RemoveConstraints(constraints: this.View.Constraints);

            StatePickerView.TranslatesAutoresizingMaskIntoConstraints = false;
            CityPickerView.TranslatesAutoresizingMaskIntoConstraints = false;
            ProceedButton.TranslatesAutoresizingMaskIntoConstraints = false;

            Console.WriteLine(UIDevice.CurrentDevice.SystemVersion);

            List<NSLayoutConstraint> stateConstraints = new List<NSLayoutConstraint>();
            //State picker view constraints
            stateConstraints.Add(NSLayoutConstraint.Create(StatePickerView, NSLayoutAttribute.Left, 
                NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Left, 1, 0));
            stateConstraints.Add(NSLayoutConstraint.Create(StatePickerView, NSLayoutAttribute.Top, 
                NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Top, 1, 44));
            stateConstraints.Add(NSLayoutConstraint.Create(StatePickerView, NSLayoutAttribute.Width, 
                NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, 0.5f, 0));
            stateConstraints.Add(NSLayoutConstraint.Create(StatePickerView, NSLayoutAttribute.Bottom,
                NSLayoutRelation.Equal, ProceedButton, NSLayoutAttribute.Top, 1, -40));
            this.View.AddConstraints(stateConstraints.ToArray());

            List<NSLayoutConstraint> cityConstraints = new List<NSLayoutConstraint>();
            //City picker view constraints
            cityConstraints.Add(NSLayoutConstraint.Create(CityPickerView, NSLayoutAttribute.Left, 
                NSLayoutRelation.Equal, StatePickerView, NSLayoutAttribute.Right, 1, 0));
            cityConstraints.Add(NSLayoutConstraint.Create(CityPickerView, NSLayoutAttribute.Right, 
                NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Right, 1, 0));
            cityConstraints.Add(NSLayoutConstraint.Create(CityPickerView, NSLayoutAttribute.Top, 
                NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Top, 1, 44));
            cityConstraints.Add(NSLayoutConstraint.Create(CityPickerView, NSLayoutAttribute.Bottom,
                NSLayoutRelation.Equal, ProceedButton, NSLayoutAttribute.Top, 1, -40));
            
            this.View.AddConstraints(cityConstraints.ToArray());

            //Proceed Button View Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(ProceedButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .90f, 0),
                NSLayoutConstraint.Create(ProceedButton, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(ProceedButton, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, RecentCitiesButton, NSLayoutAttribute.Top, 1, -5),
                NSLayoutConstraint.Create(ProceedButton, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, 30),
            });
            //Recent Cities Button View Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(RecentCitiesButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .90f, 0),
                NSLayoutConstraint.Create(RecentCitiesButton, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(RecentCitiesButton, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Bottom, 1, -25),
                NSLayoutConstraint.Create(RecentCitiesButton, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, 30),
            });
            this.View.LayoutIfNeeded();
        }

        void AddiOS8LayoutConstraints()
        {
            this.View.RemoveConstraints(constraints: this.View.Constraints);

            StateTableView.TranslatesAutoresizingMaskIntoConstraints = false;
            CityTableView.TranslatesAutoresizingMaskIntoConstraints = false;
            ProceedButton.TranslatesAutoresizingMaskIntoConstraints = false;

            UILabel stateHeader = new UILabel();
            stateHeader.AttributedText = new NSAttributedString("   State", Constants.HeaderAttributes);
            stateHeader.BackgroundColor = ColorScheme.Silver;

            UILabel cityHeader = new UILabel();
            cityHeader.AttributedText = new NSAttributedString("   City", Constants.HeaderAttributes);
            cityHeader.BackgroundColor = ColorScheme.Silver;

            this.View.AddSubview(cityHeader);
            this.View.AddSubview(stateHeader);

            stateHeader.TranslatesAutoresizingMaskIntoConstraints = false;
            //Recent Cities Button View Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(stateHeader, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .50f, 0),
                NSLayoutConstraint.Create(stateHeader, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Left, 1, 0),
                NSLayoutConstraint.Create(stateHeader, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Top, 1, 64),
                NSLayoutConstraint.Create(stateHeader, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, 32),
            });

            cityHeader.TranslatesAutoresizingMaskIntoConstraints = false;
            //Recent Cities Button View Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(cityHeader, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .50f, 0),
                NSLayoutConstraint.Create(cityHeader, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Right, 1, 0),
                NSLayoutConstraint.Create(cityHeader, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Top, 1, 64),
                NSLayoutConstraint.Create(cityHeader, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, 32),
            });

            List<NSLayoutConstraint> stateConstraints = new List<NSLayoutConstraint>();
            //State table view constraints
            stateConstraints.Add(NSLayoutConstraint.Create(StateTableView, NSLayoutAttribute.Left, 
                NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Left, 1, 0));
            stateConstraints.Add(NSLayoutConstraint.Create(StateTableView, NSLayoutAttribute.Top, 
                NSLayoutRelation.Equal, stateHeader, NSLayoutAttribute.Bottom, 1, 0));
            stateConstraints.Add(NSLayoutConstraint.Create(StateTableView, NSLayoutAttribute.Width, 
                NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, 0.5f, 0));
            stateConstraints.Add(NSLayoutConstraint.Create(StateTableView, NSLayoutAttribute.Bottom,
                NSLayoutRelation.Equal, ProceedButton, NSLayoutAttribute.Top, 1, -20));
            this.View.AddConstraints(stateConstraints.ToArray());

            List<NSLayoutConstraint> cityConstraints = new List<NSLayoutConstraint>();
            //City table view constraints
            cityConstraints.Add(NSLayoutConstraint.Create(CityTableView, NSLayoutAttribute.Left, 
                NSLayoutRelation.Equal, StateTableView, NSLayoutAttribute.Right, 1, 0));
            cityConstraints.Add(NSLayoutConstraint.Create(CityTableView, NSLayoutAttribute.Right, 
                NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Right, 1, 0));
            cityConstraints.Add(NSLayoutConstraint.Create(CityTableView, NSLayoutAttribute.Top, 
                NSLayoutRelation.Equal, cityHeader, NSLayoutAttribute.Bottom, 1, 0));
            cityConstraints.Add(NSLayoutConstraint.Create(CityTableView, NSLayoutAttribute.Bottom,
                NSLayoutRelation.Equal, ProceedButton, NSLayoutAttribute.Top, 1, -20));
            this.View.AddConstraints(cityConstraints.ToArray());

            ProceedButton.TitleLabel.AttributedText = new NSAttributedString(ProceedButton.TitleLabel.Text, Constants.ButtonAttributes);
            RecentCitiesButton.TitleLabel.AttributedText = new NSAttributedString(RecentCitiesButton.TitleLabel.Text, Constants.ButtonAttributes);

            //Proceed Button View Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(ProceedButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .90f, 0),
                NSLayoutConstraint.Create(ProceedButton, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(ProceedButton, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, RecentCitiesButton, NSLayoutAttribute.Top, 1, -8),
                NSLayoutConstraint.Create(ProceedButton, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, Constants.ButtonHeight),
            });
            //Recent Cities Button View Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(RecentCitiesButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .90f, 0),
                NSLayoutConstraint.Create(RecentCitiesButton, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(RecentCitiesButton, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Bottom, 1, -25),
                NSLayoutConstraint.Create(RecentCitiesButton, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, Constants.ButtonHeight),
            });

            this.View.LayoutIfNeeded();
        }
	}
}