using System;
using Xamarin.UITest;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;
using System.Threading;

namespace ethanslist.UITests
{
    public class CityPickerPage : BasePage
    {
        readonly string ProceedButton;
        readonly string RecentCityButton;
        readonly string MenuButton;

        public CityPickerPage()
            : base(x => x.Class("CityPickerRow"), x => x.Marked("Select City"))
        {
            if (OniOS)
            {
                ProceedButton = "proceedButton";
                RecentCityButton = "Recent Cities";
                MenuButton = "menu.png";
            }
            if (OnAndroid)
            {
                MenuButton = "Open drawer";
            }
        }

        public CityPickerPage SelectState(string state)
        {
            if (OniOS)
            {
                app.ScrollDownTo(state, "StatePickTableView", timeout: TimeSpan.FromSeconds(20));
                app.Tap(state);
            }
            else
            {
                app.ScrollDownTo(x => x.Marked(state), x => x.Class("ListView").Index(0), timeout: TimeSpan.FromSeconds(20));
                app.Tap(state);

            }
            Thread.Sleep(500);
            app.Screenshot("Selected state: " + state);

            return this;
        }

        public CityPickerPage SelectCity(string city)
        {
            if (OniOS)
            {
                app.ScrollDownTo(city, "CityPickTableView", timeout: TimeSpan.FromSeconds(20));
                app.Tap(city);
            }
            else
            {
                app.ScrollDownTo(x => x.Marked(city), x => x.Class("ListView").Index(1), timeout: TimeSpan.FromSeconds(20));
                app.Tap(city);

            }

            Thread.Sleep(500);
            app.Screenshot("Selected city: " + city);

            return this;
        }

        public void ProceedToSearchOptions()
        {
            app.Tap(ProceedButton);
            app.Screenshot("Tapped Proceed Button");
        }

        public void ProceedToRecentCities()
        {
            app.Tap(RecentCityButton);
            app.Screenshot("Tapped Recent Cities Button");
        }

        public override void GoBack()
        {
            app.Tap(MenuButton);
        }
    }
}

