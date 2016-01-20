using System;
using Xamarin.UITest;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace ethanslist.UITests
{
    public class SearchOptionsPage : BasePage
    {
        readonly string SearchButton = "Search";
        readonly string SaveButton;

        public SearchOptionsPage()
            : base ("androidTrait", "save.png")
        {
            if (OniOS)
            {
                SaveButton = "save.png";
            }
        }

        public SearchOptionsPage VerifyOnLocation(string state)
        {
            app.WaitForElement(string.Format("Search {0} for:", state));
            app.Screenshot("Verified searching: " + state);
            return this;
        }

        public void ProceedToSearch()
        {
            app.Tap(SearchButton);
            app.Screenshot("Tapped Search Button");
        }
    }
}

