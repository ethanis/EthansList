using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace EthansList.Models
{
    public partial class DatabaseConnection
    {
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
}

