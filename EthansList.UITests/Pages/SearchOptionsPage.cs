using System;
using Xamarin.UITest;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace ethanslist.UITests
{
    public class SearchOptionsPage : BasePage
    {
        public SearchOptionsPage()
            : base ("androidTrait", "save.png")
        {
        }

        public SearchOptionsPage VerifyOnLocation(string state)
        {
            app.WaitForElement(string.Format("Search {0} for:", state));

            return this;
        }
    }
}

