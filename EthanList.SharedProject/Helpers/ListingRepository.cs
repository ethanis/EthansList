using System;
using System.Collections.Generic;
using System.Linq;
using EthansList.Shared;
using SQLite;
using System.Threading.Tasks;
using Listings.Models;

namespace Listings
{
    public class ListingRepository
    {
        private readonly SQLiteAsyncConnection conn;

        public string StatusMessage { get; set; }

        public ListingRepository(string dbPath)
        {
            conn = new SQLiteAsyncConnection(dbPath);
            conn.CreateTableAsync<Listing>().Wait();
        }

        public async Task AddNewListingAsync(String title, string description, string link, string imageLink, DateTime date)
        {
            try
            {
                //basic validation to ensure a name was entered
                if (title == null)
                    throw new Exception("Valid listing required");

                //insert a new person into the Person table
                var result = await conn.InsertAsync(new Listing { Title = title, 
                    Description = description, Link = link, 
                    ImageLink = imageLink, Date = date })
                        .ConfigureAwait(continueOnCapturedContext: false);
                StatusMessage = string.Format("{0} record(s) added [Title: {1})", result, title);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", title, ex.Message);
            }
        }

        public Task<List<Listing>> GetAllListingsAsync()
        {
            //return a list of people saved to the Person table in the database
            return conn.Table<Listing>().ToListAsync();
        }
    }
}