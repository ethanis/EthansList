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
            : base ("androidTrait", "cityPicker")
        {
            if (OniOS)
            {
                ProceedButton = "proceedButton";
                RecentCityButton = "Recent Cities";
                MenuButton = "menu.png";
            }
        }

        public CityPickerPage SelectStateAtRow(int row, string state)
        {
            app.Query(x => x.Id("statePicker").Invoke("selectRow", row, "inComponent", 0, "animated", false));
            app.Tap(state);
            Thread.Sleep(500);

            return this;
        }

        public CityPickerPage SelectCityAtRow(int row, string city)
        {
            app.Query(x => x.Id("cityPicker").Invoke("selectRow", row, "inComponent", 0, "animated", false));
            app.Tap(city);
            Thread.Sleep(500);

            return this;
        }

        public void ProceedToSearchOptions()
        {
            app.Tap(ProceedButton);
        }

        public void ProceedToRecentCities()
        {
            app.Tap(RecentCityButton);
        }

        public override void GoBack()
        {
            app.Tap(MenuButton);
        }
    }
}

