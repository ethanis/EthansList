using System;
using System.Collections.Generic;
using System.Linq;
using EthansList.Shared;
using SQLite;
using System.Threading.Tasks;
using EthansList.Models;

namespace EthansList.Models
{
    public partial class DatabaseConnection
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
            conn.CreateTableAsync<FavoriteCategory>().Wait();
        }
    }

    public enum codes
    {
        ok,
        bad
    }
}