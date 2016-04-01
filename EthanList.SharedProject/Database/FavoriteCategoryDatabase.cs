using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using EthansList.Shared;
using System.Linq;

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

        public async Task<bool> DeleteFavoriteCategoryAsync(FavoriteCategory category)
        {
            try
            {
                var result = await conn.DeleteAsync(category).ConfigureAwait(continueOnCapturedContext: false);
                StatusMessage = string.Format("{0} dropped from database [Cat: {1}]", result, category);
                StatusCode = codes.ok;
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to delete record: {0}, Error: {1}", category, ex.Message);
                StatusCode = codes.bad;
                return false;
            }
        }

        public async Task<bool> DeleteFavoriteCategoryAsync(string categoryKey)
        {
            FavoriteCategory current = conn.Table<FavoriteCategory>().Where(x => x.CategoryKey.Equals(categoryKey)).ToListAsync().Result.First();
            try
            {
                var result = await conn.DeleteAsync(current).ConfigureAwait(continueOnCapturedContext: false);
                StatusMessage = string.Format("{0} dropped from database [Cat: {1}]", result, current);
                StatusCode = codes.ok;
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to delete record: {0}, Error: {1}", current, ex.Message);
                StatusCode = codes.bad;
                return false;
            }
        }

        public async Task<bool> DeleteFavoriteCategoryAsync(string categoryValue, bool val)
        {
            FavoriteCategory current = conn.Table<FavoriteCategory>().Where(x => x.CategoryValue.Equals(categoryValue)).ToListAsync().Result.First();
            try
            {
                var result = await conn.DeleteAsync(current).ConfigureAwait(continueOnCapturedContext: false);
                StatusMessage = string.Format("{0} dropped from database [Cat: {1}]", result, current);
                StatusCode = codes.ok;
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to delete record: {0}, Error: {1}", current, ex.Message);
                StatusCode = codes.bad;
                return false;
            }
        }

        public bool FavoriteCategoryAlreadyPresent(String categoryKey)
        {
            var current = conn.Table<FavoriteCategory>().Where(x => x.CategoryKey.Equals(categoryKey)).ToListAsync().Result;
            return current.Count > 0;
        }

        public Task<List<FavoriteCategory>> GetAllFavoriteCategoriesAsync()
        {
            return conn.Table<FavoriteCategory>().ToListAsync();
        }

    }
}

