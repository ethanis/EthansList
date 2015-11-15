using System;
using SQLite;

namespace EthansList.Shared
{
    [Table("listings")]
    public class Listing
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Unique]
        public Posting information { get; set;}
    }
}

