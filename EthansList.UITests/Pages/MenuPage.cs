using System;
using Xamarin.UITest;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace ethanslist.UITests
{
    public class MenuPage : BasePage
    {
        readonly string SearchCraigslistButton = "Search Craigslist";
        readonly string RecentCitiesButton = "Recent Cities";
        readonly string SavedSearchesButton = "Saved Searches";
        readonly string SavedPostingsButton = "Saved Postings";

        public MenuPage()
            : base("androidTrait", "Ethan's List")
        {
        }

        public void SearchCraigslist()
        {
            app.Tap(SearchCraigslistButton);
        }

        public void RecentCities()
        {
            app.Tap(RecentCitiesButton);
        }

        public void SavedSearches()
        {
            app.Tap(SavedSearchesButton);
        }

        public void SavedPostings()
        {
            app.Tap(SavedPostingsButton);
        }
    }
}

