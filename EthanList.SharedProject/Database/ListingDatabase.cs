using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace EthansList.Models
{
    public partial class DatabaseConnection
    {

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

        public async Task DeletePostingAsync(string link)
        {
            var query = conn.Table<Posting>().Where(x => x.Link.Equals(link)).ToListAsync().Result.First();
            if (query != null)
                await DeletePostingAsync(query).ConfigureAwait(false);
        }

        public Task<List<Posting>> GetAllPostingsAsync()
        {
            return conn.Table<Posting>().ToListAsync();
        }

        public bool PostingAlreadySaved(Posting posting)
        {
            var query = conn.Table<Posting>().Where(v => v.Link.Equals(posting.Link)).ToListAsync().Result;
            if (query.Count > 0)
                return true;

            return false;
        }
    }
}

