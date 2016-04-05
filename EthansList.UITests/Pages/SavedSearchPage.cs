using System;
using Xamarin.UITest;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace ethanslist.UITests
{
    public class SavedSearchPage : BasePage
    {
        public SavedSearchPage()
            : base("Saved Searches", "Saved Searches")
        {
        }

        public SavedSearchPage VerifyPreset(string search)
        {
            app.WaitForElement(search, timeoutMessage: "Timed out wait for search to appear");
            app.Screenshot("Verified search was saved");
            return this;
        }
    }
}

