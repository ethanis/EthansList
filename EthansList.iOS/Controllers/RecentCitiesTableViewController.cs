using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using EthansList.Models;
using System.Collections.Generic;

namespace ethanslist.ios
{
	partial class RecentCitiesTableViewController : UITableViewController
	{
        RecentCityTableViewSource recentCitySource;
        private List<RecentCity> recentCities;
        public bool FromMenu { 
            get { return fromMenu; } 
            set { fromMenu = value; } 
        }
        private bool fromMenu = true;

		public RecentCitiesTableViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (FromMenu)
            {
                NavigationItem.SetLeftBarButtonItem(
                    new UIBarButtonItem(UIImage.FromBundle("menu.png"), UIBarButtonItemStyle.Plain, (s, e) => NavigationController.PopViewController(true)), 
                    true);
            }

            recentCities = AppDelegate.databaseConnection.GetAllRecentCitiesAsync().Result;
            recentCities.Sort((s1, s2)=>s2.Updated.CompareTo(s1.Updated));

            recentCitySource = new RecentCityTableViewSource(this, recentCities);
            TableView.Source = recentCitySource;
        }
	}
}
