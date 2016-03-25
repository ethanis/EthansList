using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace ethanslist.UITests
{
    public class Tests : AbstractSetup
    {
        public Tests(Platform platform)
            : base(platform)
        {
        }

        [Test]
        public void SavePosting()
        {
            string state = "California";
            string area = "San Francisco Bay Area";
            string title = String.Empty;
            string cat = "real estate for sale";

            new CityPickerPage()
                .SelectState(state)
                .SelectCity(area);

            new CategoryPickerPage()
                .SelectCategory(cat);

            new SearchOptionsPage()
                .VerifyOnLocation(area)
                .ProceedToSearch();

            new FeedResultsPage()
                .SelectFirstListing();

            title = new PostingDetailsPage().GetListingTitle();

            new PostingDetailsPage()
                .SaveListing()
                .LookAtPosting()
                .ExitListing();

            new FeedResultsPage().GoBack();
            new SearchOptionsPage().GoBack();
            new CategoryPickerPage().GoBack();
            new CityPickerPage().GoBack();

            new MenuPage()
                .SavedPostings();

            new SavedPostingsPage()
                .ConfirmPostingVisible(title);
        }

        [Test]
        public void SearchPage()
        {
            string state = "California";
            string area = "San Francisco Bay Area";
            string searchTerms = "query: parking laundry view, min_price: $1000, max_price: $5000, minSqft: , maxSqft: , bedrooms: 3, bathrooms: 1";
            string cat = "apartments / housing rentals";

            new CityPickerPage()
                .SelectState(state)
                .SelectCity(area);

            new CategoryPickerPage()
                .SelectCategory(cat);

            new SearchOptionsPage()
                .VerifyOnLocation(area)
                .EnterSearchTerms(new string[] { "parking", "laundry", "view" })
                .SelectMinMaxPrice(1000, 5000)
                .SelectMinBedrooms(3)
                .SelectMinBathrooms(1)
                .SelectPostedDate("2 Weeks Old")
                .SelectMaxListings(50)
                .SaveSearch()
                .ProceedToSearch();

            new FeedResultsPage().GoBack();
            new CategoryPickerPage().GoBack();
            new SearchOptionsPage().GoBack();
            new CityPickerPage().GoBack();
            new MenuPage().SavedSearches();

            new SavedSearchPage().VerifyPreset(searchTerms);
        }
    }
}

