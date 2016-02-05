using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EthansList.Models
{
    public partial class DatabaseConnection
    {
        public async Task AddNewSearchAsync(String linkUrl, String cityName, string minPrice, string maxPrice, string minBedrooms, string minBathrooms, string searchQuery, int? postedDate, int maxListings, string categoryKey, string CategoryValue)
        {
            try
            {
                //basic validation to ensure a name was entered
                if (linkUrl == null)
                    throw new Exception("Valid search required");

                //insert a new person into the Person table
                var result = await conn.InsertAsync(new Search { LinkUrl = linkUrl, 
                    CityName = cityName, MinPrice = minPrice, MaxPrice = maxPrice, 
                    MinBedrooms = minBedrooms, MinBathrooms = minBathrooms, SearchQuery = searchQuery,
                    MaxListings = maxListings, PostedDate = postedDate, CategoryKey = categoryKey, CategoryValue = CategoryValue})
                    .ConfigureAwait(continueOnCapturedContext: false);
                StatusMessage = string.Format("{0} search(es) added [Link: {1})", result, linkUrl);
                StatusCode = codes.ok;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", linkUrl, ex.Message);
                StatusCode = codes.bad;
            }
        }

        public async Task DeleteSearchAsync(Search search)
        {
            try
            {
                var result = await conn.DeleteAsync(search).ConfigureAwait(continueOnCapturedContext: false);
                StatusMessage = string.Format("{0} dropped from database [Link: {1}]", result, search.LinkUrl);
                StatusCode = codes.ok;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to delete record: {0}, Error: {1}", search.LinkUrl, ex.Message); 
                StatusCode = codes.bad;
            }
        }

        public Task<List<Search>> GetAllSearchesAsync()
        {
            return conn.Table<Search>().ToListAsync();
        }

        public string FormatSearchQuery(Search search)
        {
            return String.Format("Min Price: {1}{0}Max Price: {2}{0}Min Bedrooms: {3}{0}Min Bathrooms: {4}{0}Search Items: {5}",
                Environment.NewLine, search.MinPrice, search.MaxPrice, search.MinBedrooms, search.MinBathrooms, search.SearchQuery);
        }

        public string SecondFormatSearch(Search search)
        {
            string result = string.Empty;

            foreach (KeyValuePair<string, object> prop in new Dictionary<string, object>() 
            {
                {"Min Price: ", search.MinPrice}, 
                {"Max Price: ", search.MaxPrice},
                {"Min Bedrooms: ", search.MinBedrooms},
                {"Min Bathrooms: ", search.MinBathrooms},
                {"Search Items: ", search.SearchQuery},
                {"Max Listings: ", search.MaxListings},
                {"Posted Date: ", search.PostedDate}
            })
            {
                if (prop.Value != null)
                    result += String.Format("{0}{1}, ", prop.Key, prop.Value);
//                else 
//                    result += String.Format("{0}{1}, ", prop.Key, "Any");
            }
            result = result.Trim();

            if (result[result.Length - 1].Equals(','))
                result = result.TrimEnd(',');

            return result;
        }
    }
}

