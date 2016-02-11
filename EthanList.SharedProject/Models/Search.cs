using System;
using SQLite;
using System.Collections.Generic;

namespace EthansList.Models
{
    [Table("searches")]
    public class Search
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set;}
        public string SerializedSearch { get; set;}

    }
}

