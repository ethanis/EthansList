using System;
using SQLite;

namespace EthansList.Models
{
    [Table("recentCities")]
    public class RecentCity
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set;}
        [Unique]
        public string City { get; set;}
        public string Url { get; set; }
    }
}

