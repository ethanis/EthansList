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
        public string CityName {get;set;}
        public string LinkUrl { get; set;}
        public string MinPrice { get; set;}
        public string MaxPrice { get; set;}
        public string MinBedrooms { get; set;}
        public string MinBathrooms { get; set;}
        public string SearchQuery { get; set;}
        public int? PostedDate { get; set;}
        public int MaxListings { get; set;}
        public string CategoryKey {get;set;}
        public string CategoryValue {get;set;}

    }
}

