using System;
using System.Collections.Generic;

namespace EthansList.Shared
{
    public static class Categories
    {
        static AllCategories catHelper = new AllCategories();

        public static Dictionary<int, KeyValuePair<string,string>> all { get { return catHelper.categories; } }
    }

    public class AllCategories
    {
        public Dictionary<int, KeyValuePair<string,string>> categories = new Dictionary<int,KeyValuePair<string,string>>()
        {
            { 0, new KeyValuePair<string, string>("sss", "all for sale / wanted" ) },
            { 1, new KeyValuePair<string, string>("ata", "antiques"              )},
            { 2, new KeyValuePair<string, string>("ppa", "appliances"            )},
            { 3, new KeyValuePair<string, string>("ara", "arts+crafts"           )},
            { 4, new KeyValuePair<string, string>("pta", "auto parts"            )},
            { 5, new KeyValuePair<string, string>("baa", "baby+kids"             )},
            { 6, new KeyValuePair<string, string>("bar", "barter"                )},
            { 7, new KeyValuePair<string, string>("haa", "beauty+health"         )},
            { 8, new KeyValuePair<string, string>("bia", "bikes"                 )},
            { 9, new KeyValuePair<string, string>("boo", "boats"                 )},
            { 10, new KeyValuePair<string, string>("bka", "books"                 )},
            { 11, new KeyValuePair<string, string>("bfa", "business"              )},
            { 12, new KeyValuePair<string, string>("cta", "cars+trucks"           )},
            { 13, new KeyValuePair<string, string>("ema", "cd/dvd/vhs"            )},
            { 14, new KeyValuePair<string, string>("moa", "cell phones"           )},
            { 15, new KeyValuePair<string, string>("cla", "clothing+accessories"  )},
            { 16, new KeyValuePair<string, string>("cba", "collectibles"          )},
            { 17, new KeyValuePair<string, string>("sya", "computers"             )},
            { 18, new KeyValuePair<string, string>("ela", "electronics"           )},
            { 19, new KeyValuePair<string, string>("gra", "farm+garden"           )},
            { 20, new KeyValuePair<string, string>("zip", "free stuff"            )},
            { 21, new KeyValuePair<string, string>("fua", "furniture"             )},
            { 22, new KeyValuePair<string, string>("gms", "garage sales"          )},
            { 23, new KeyValuePair<string, string>("foa", "general for sale"      )},
            { 24, new KeyValuePair<string, string>("hsa", "household"             )},
            { 25, new KeyValuePair<string, string>("wan", "items wanted"          )},
            { 26, new KeyValuePair<string, string>("jwa", "jewelry"               )},
            { 27, new KeyValuePair<string, string>("maa", "materials"             )},
            { 28, new KeyValuePair<string, string>("mca", "motorcycles"           )},
            { 29, new KeyValuePair<string, string>("msa", "musical instruments"   )},
            { 30, new KeyValuePair<string, string>("pha", "photo+video"           )},
            { 31, new KeyValuePair<string, string>("rva", "recreational vehicles" ) },
            { 32, new KeyValuePair<string, string>("sga", "sporting goods"        )},
            { 33, new KeyValuePair<string, string>("tia", "tickets"               )},
            { 34, new KeyValuePair<string, string>("tla", "tools"                 )},
            { 35, new KeyValuePair<string, string>("taa", "toys+games"            )},
            { 36, new KeyValuePair<string, string>("vga", "video gaming"          ) }
        };
    }
}

