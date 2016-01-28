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
            :base(platform)
        {
        }
            
        [Test]
        public void SavePosting()
        {
            string state = "California";
            string area = "San Francisco Bay Area";
            string title = String.Empty;

            new CityPickerPage()
                .SelectStateAtRow(3, state)
                .SelectCityAtRow(19, area)
                .ProceedToSearchOptions();

            new SearchOptionsPage()
                .VerifyOnLocation(area)
                .ProceedToSearch();

            new FeedResultsPage()
                .SelectFirstListing();

            title = new PostingDetailsPage().GetListingTitle();

            new PostingDetailsPage()
                .SaveListing()
                .ExitListing();

            new FeedResultsPage().GoBack();
            new SearchOptionsPage().GoBack();
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
            string searchTerms = "Min Bedrooms: 3, Min Bathrooms: 1, Search Items: parking laundry view, Max Listings: 50, Posted Date: 2";

            new CityPickerPage()
                .SelectStateAtRow(3, state)
                .SelectCityAtRow(19, area)
                .ProceedToSearchOptions();

            new SearchOptionsPage()
                .VerifyOnLocation(area)
                .EnterSearchTerms(new string[] {"parking", "laundry", "view"})
                .SelectMinBedrooms(3)
                .SelectMinBathrooms(1)
                .SelectPostedDate("2 Weeks Old")
                .SelectMaxListings(50)
                .SaveSearch()
                .ProceedToSearch();

            new FeedResultsPage().GoBack();
            new SearchOptionsPage().GoBack();
            new CityPickerPage().GoBack();
            new MenuPage().SavedSearches();

            new SavedSearchPage().VerifyPreset(searchTerms);
        }
    }
}

