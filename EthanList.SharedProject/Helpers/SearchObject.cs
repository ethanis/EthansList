using System;
using System.Collections.Generic;

namespace EthansList.Shared
{
    public class SearchObject
    {
        public Location SearchLocation { get; set;}
        public String Category { get; set; }
        public Dictionary<string, string> SearchItems { get; set;}
        public Dictionary<object,KeyValuePair<object, object>> Conditions { get; set;}
    }
}

