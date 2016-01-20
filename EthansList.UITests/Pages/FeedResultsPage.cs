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
            : base ("androidTrait", "Craigslist Results")
        {
            if (OniOS)
            {
                LoadingOverlay = x => x.Class("ethanslist_ios_LoadingOverlay");
                FirstListingCell = x => x.Class("UITableViewCell");
            }

            app.WaitForNoElement(LoadingOverlay, timeout:TimeSpan.FromSeconds(10), timeoutMessage:"Timed out waiting for loading to complete");
        }

        public void SelectFirstListing()
        {
            app.Tap(FirstListingCell);
        }
    }
}

