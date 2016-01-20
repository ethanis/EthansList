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
    }
}

