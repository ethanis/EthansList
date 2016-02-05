using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using EthansList.Shared;

namespace EthansList.Models
{
    public partial class DatabaseConnection
    {
        public async Task AddNewFavoriteCategoryAsync(String categoryKey, string categoryValue)
        {
            var current = conn.Table<FavoriteCategory>().Where(x => x.CategoryKey.Equals(categoryKey)).ToListAsync().Result;
            try
            {
                if (current.Count > 0)
                {
                    foreach (var past in current)
                    {
                        past.Updated = System.DateTime.UtcNow;
                        var result = conn.UpdateAsync(past).Result;
                        StatusMessage = string.Format("{0} favorite category updated [Cat: {1})", result, past);
                        StatusCode = codes.ok;
                    }
                }
                else 
                {
                    //basic validation to ensure a name was entered
                    if (categoryKey == null)
                        throw new Exception("Valid city required");

                    var result = await conn.InsertAsync(new FavoriteCategory() { CategoryKey = categoryKey, CategoryValue = categoryValue, Updated = System.DateTime.UtcNow })
                        .ConfigureAwait(continueOnCapturedContext: false);
                    StatusMessage = string.Format("{0} category added [Cat: {1})", result, categoryKey);
                    StatusCode = codes.ok;
                }
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", categoryKey, ex.Message);
                StatusCode = codes.bad;
            }
        }

        public async Task DeleteFavoriteCategoryAsync(FavoriteCategory category)
        {
            try
            {
                var result = await conn.DeleteAsync(category).ConfigureAwait(continueOnCapturedContext: false);
                StatusMessage = string.Format("{0} dropped from database [Cat: {1}]", result, category);
                StatusCode = codes.ok;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to delete record: {0}, Error: {1}", category, ex.Message); 
                StatusCode = codes.bad;
            }
        }

        public Task<List<FavoriteCategory>> GetAllFavoriteCategoriesAsync()
        {
            return conn.Table<FavoriteCategory>().ToListAsync();
        }

    }
}

