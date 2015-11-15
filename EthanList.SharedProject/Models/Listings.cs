using System;
using SQLite;
using EthansList.Shared;

namespace Listings.Models
{
    [Table("listings")]
    public class Listing
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Title { get; set;}
        public string Description { get; set; }

        [Unique]
        public string Link { get; set;}
        public string ImageLink { get; set;}
        public DateTime Date { get; set; }
    }
}

