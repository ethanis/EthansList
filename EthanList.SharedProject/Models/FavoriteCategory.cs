using System;
using SQLite;

namespace EthansList.Shared
{
    [Table("FavoriteCategories")]
    public class FavoriteCategory
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set;}
        [Unique]
        public string CategoryKey { get; set;}
        public string CategoryValue {get;set;}
        public DateTime Updated { get; set;}
    }
}

