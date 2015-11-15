using System;
using System.Collections.Generic;
using System.Linq;
using EthansList.Shared;
using SQLite;
using System.Threading.Tasks;

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

        public async Task AddNewListingAsync(Posting post)
        {
            try
            {
                //basic validation to ensure a name was entered
                if (post.Title != null)
                    throw new Exception("Valid listing required");

                //insert a new person into the Person table
                var result = await conn.InsertAsync(new Listing { information = post }).ConfigureAwait(continueOnCapturedContext: false);
                StatusMessage = string.Format("{0} record(s) added [Title: {1})", result, post.Title);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", post.Title, ex.Message);
            }
        }

        public Task<List<Listing>> GetAllListingsAsync()
        {
            //return a list of people saved to the Person table in the database
            return conn.Table<Listing>().ToListAsync();
        }
    }
}