using System;
using Xamarin.UITest;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace ethanslist.UITests
{
    public class FeedResultsPage : BasePage
    {
        readonly Query LoadingOverlay;
        readonly Query FirstListingCell;

        public FeedResultsPage()
            : base (x => x.Class("FeedResultRow"), x => x.Marked("Search Results"))
        {
            if (OniOS)
            {
                LoadingOverlay = x => x.Class("ethanslist_ios_LoadingOverlay");
                FirstListingCell = x => x.ClassFull("ethanslist_ios_FeedResultsCell");
            }
            if (OnAndroid)
            {
                FirstListingCell = x => x.Class("FeedResultRow").Index(0);
                LoadingOverlay = x => x.Id("progress");
            }

            app.WaitForNoElement(LoadingOverlay, timeout:TimeSpan.FromSeconds(60), timeoutMessage:"Timed out waiting for loading to complete");
            app.Screenshot("Loading icon has been removed");
        }

        public void SelectFirstListing()
        {
            app.Tap(FirstListingCell);
            app.Screenshot("Selected first listing");
        }

        public FeedResultsPage VerifyCatValid()
        {
            app.WaitForElement(FirstListingCell, timeout:TimeSpan.FromSeconds(30));
            app.Screenshot("Verified category has listings");

            return this;
        }
    }
}

