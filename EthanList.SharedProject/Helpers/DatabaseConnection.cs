using System;
using System.Collections.Generic;
using System.Linq;
using EthansList.Shared;
using SQLite;
using System.Threading.Tasks;
using EthansList.Models;

namespace EthansList.Models
{
    public class DatabaseConnection
    {
        private readonly SQLiteAsyncConnection conn;

        public string StatusMessage { get; set; }
        public codes StatusCode { get; set; }

        public DatabaseConnection(string dbPath)
        {
            conn = new SQLiteAsyncConnection(dbPath);
            conn.CreateTableAsync<Posting>().Wait();
            conn.CreateTableAsync<Search>().Wait();
            conn.CreateTableAsync<RecentCity>().Wait();
        }

        public async Task AddNewListingAsync(String title, string description, string link, string imageLink, DateTime date)
        {
            try
            {
                //basic validation to ensure a title was entered
                if (title == null)
                    throw new Exception("Valid listing required");

                //insert a new posting into the Posting table
                var result = await conn.InsertAsync(new Posting { PostTitle = title, 
                    Description = description, Link = link, 
                    ImageLink = imageLink, Date = date })
                        .ConfigureAwait(continueOnCapturedContext: false);
                StatusMessage = string.Format("{0} record(s) added [Title: {1})", result, title);
                StatusCode = codes.ok;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", title, ex.Message);
                StatusCode = codes.bad;
            }
        }

        public async Task DeletePostingAsync(Posting posting)
        {
            try
            {
                var result = await conn.DeleteAsync(posting).ConfigureAwait(continueOnCapturedContext: false);
                StatusMessage = string.Format("{0} dropped from database [Title: {1}]", result, posting.PostTitle);
                StatusCode = codes.ok;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to delete record: {0}, Error: {1}", posting.PostTitle, ex.Message); 
                StatusCode = codes.bad;
            }
        }

        public Task<List<Posting>> GetAllPostingsAsync()
        {
            //return a list of people saved to the Person table in the database
            return conn.Table<Posting>().ToListAsync();
        }

        public bool PostingAlreadySaved(Posting posting)
        {
            var query = conn.Table<Posting>().Where(v => v.Link.Equals(posting.Link)).ToListAsync().Result;
            if (query.Count > 0)
                return true;

            return false;
        }

        public async Task AddNewSearchAsync(String linkUrl, String cityName, string minPrice, string maxPrice, string minBedrooms, string minBathrooms, string searchQuery, int? postedDate, int maxListings)
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
                    MaxListings = maxListings, PostedDate = postedDate})
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
            //return a list of people saved to the Person table in the database
            return conn.Table<Search>().ToListAsync();
        }

        public string FormatSearchQuery(Search search)
        {
            return String.Format("Min Price: {1}{0}Max Price: {2}{0}Min Bedrooms: {3}{0}Min Bathrooms: {4}{0}Search Items: {5}",
                Environment.NewLine, search.MinPrice, search.MaxPrice, search.MinBedrooms, search.MinBathrooms, search.SearchQuery);
        }

        public async Task AddNewRecentCityAsync(String city, string url)
        {
            var current = conn.Table<RecentCity>().Where(x => x.City.Equals(city)).ToListAsync().Result;
            try
            {
                if (current.Count > 0)
                {
                    foreach (var past in current)
                    {
                        past.Updated = System.DateTime.UtcNow;
                        var result = conn.UpdateAsync(past).Result;
                        StatusMessage = string.Format("{0} recent city updated [City: {1})", result, past);
                        StatusCode = codes.ok;
                    }
                }
                else 
                {
                        //basic validation to ensure a name was entered
                        if (city == null)
                            throw new Exception("Valid city required");

                        var result = await conn.InsertAsync(new RecentCity { City = city, Url = url, Updated = System.DateTime.UtcNow })
                            .ConfigureAwait(continueOnCapturedContext: false);
                        StatusMessage = string.Format("{0} recent city added [City: {1})", result, city);
                        StatusCode = codes.ok;
                }
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", city, ex.Message);
                StatusCode = codes.bad;
            }
        }

        public async Task DeleteOldestCityAsync()
        {
            var list = GetAllRecentCitiesAsync().Result;
            RecentCity oldest = list.OrderBy(x => x.Updated).FirstOrDefault();

            try
            {
                await DeleteCityAsync(oldest);
                StatusMessage = string.Format("Oldest Search dropped from database [City: {1}]", oldest);
                StatusCode = codes.ok;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to delete last city: {0}, Error: {1}", oldest, ex.Message);
                StatusCode = codes.bad;
            }
        }

        public async Task DeleteCityAsync(RecentCity city)
        {
            try
            {
                var result = await conn.DeleteAsync(city).ConfigureAwait(continueOnCapturedContext: false);
                StatusMessage = string.Format("{0} dropped from database [City: {1}]", result, city);
                StatusCode = codes.ok;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to delete record: {0}, Error: {1}", city, ex.Message); 
                StatusCode = codes.bad;
            }
        }

        public Task<List<RecentCity>> GetAllRecentCitiesAsync()
        {
            return conn.Table<RecentCity>().ToListAsync();
        }
    }

    public enum codes
    {
        ok,
        bad
    }
}