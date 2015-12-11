using System;
using SQLite;

namespace EthansList.Models
{
    [Table("postings")]
    public class Posting
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string PostTitle { get; set; }
        public string Description { get; set; }
        [Unique]
        public string Link { get; set;}
        public string ImageLink { get; set;}
        public DateTime Date { get; set; }
    }
}

