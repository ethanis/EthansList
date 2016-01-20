using System;
using NUnit.Framework;
using Xamarin.UITest;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace ethanslist.UITests
{
    public class SavedPostingsPage : BasePage
    {
        public SavedPostingsPage()
            : base("androidTrait", "Saved Postings")
        {
        }

        public SavedPostingsPage ConfirmPostingVisible(string title)
        {
            var result = app.Query(title)[0];
            Assert.IsNotNull(result, "Unable to verify posting was saved");

            return this;
        }
    }
}

