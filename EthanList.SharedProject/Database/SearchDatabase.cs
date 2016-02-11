using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using EthansList.Shared;
using System.Linq;

namespace EthansList.Models
{
    public partial class DatabaseConnection
    {
        public async Task AddNewSearchAsync(String linkUrl, String serialized)
        {
            try
            {
                //basic validation to ensure a name was entered
                if (serialized == null)
                    throw new Exception("Valid search required");

                //insert a new person into the Person table
                var result = await conn.InsertAsync(new Search {SerializedSearch = serialized}).ConfigureAwait(false);

                StatusMessage = string.Format("{0} search(es) added [Link: {1})", result, linkUrl);
                StatusCode = codes.ok;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", linkUrl, ex.Message);
                StatusCode = codes.bad;
            }
        }

        public async Task DeleteSearchAsync(String linkUrl, SearchObject searchObject)
        {
            var currentSerialized = JsonConvert.SerializeObject(searchObject);
            var search = conn.Table<Search>().Where(x => x.SerializedSearch.Equals(currentSerialized)).ToListAsync().Result.First();

            if (search == null)
                Console.WriteLine("----------------------------------------------------------------------------------------------");
            
            try
            {
                var result = await conn.DeleteAsync(search).ConfigureAwait(continueOnCapturedContext: false);
                StatusMessage = string.Format("{0} dropped from database [Link: {1}]", result, linkUrl);
                StatusCode = codes.ok;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to delete record: {0}, Error: {1}", linkUrl, ex.Message); 
                StatusCode = codes.bad;
            }
        }

        public Task<List<Search>> GetAllSearchesAsync()
        {
            return conn.Table<Search>().ToListAsync();
        }

        public string SecondFormatSearch(SearchObject searchObject)
        {
            string result = string.Empty;

            foreach (KeyValuePair<string, string> prop in searchObject.SearchItems)
            {
                if (prop.Value != null)
                    result += String.Format("{0}{1}, ", prop.Key, prop.Value);
            }
            result = result.Trim();

            if (searchObject.SearchItems.Count > 0 && result[result.Length - 1].Equals(','))
                result = result.TrimEnd(',');

            return result;
        }
    }
}

