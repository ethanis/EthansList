using System;
using System.Collections.Generic;

namespace EthansList.Shared
{
    public static class Categories
    {
        static AllCategories catHelper = new AllCategories();

        public static List<KeyValuePair<string,string>> all { get { return catHelper.categories; } }
    }

    public class AllCategories
    {
        public List<KeyValuePair<string,string>> categories = new List<KeyValuePair<string,string>>()
        {
            { new KeyValuePair<string, string>("sss", "all for sale / wanted" )},
            { new KeyValuePair<string, string>("ata", "antiques"              )},
            { new KeyValuePair<string, string>("apa", "apartments/housing rentals")},
            { new KeyValuePair<string, string>("ppa", "appliances"            )},
            { new KeyValuePair<string, string>("ara", "arts+crafts"           )},
            { new KeyValuePair<string, string>("pta", "auto parts"            )},
            { new KeyValuePair<string, string>("baa", "baby+kids"             )},
            { new KeyValuePair<string, string>("bar", "barter"                )},
            { new KeyValuePair<string, string>("haa", "beauty+health"         )},
            { new KeyValuePair<string, string>("bia", "bikes"                 )},
            { new KeyValuePair<string, string>("boo", "boats"                 )},
            { new KeyValuePair<string, string>("bka", "books"                 )},
            { new KeyValuePair<string, string>("bfa", "business"              )},
            { new KeyValuePair<string, string>("cta", "cars+trucks"           )},
            { new KeyValuePair<string, string>("ema", "cd/dvd/vhs"            )},
            { new KeyValuePair<string, string>("moa", "cell phones"           )},
            { new KeyValuePair<string, string>("cla", "clothing+accessories"  )},
            { new KeyValuePair<string, string>("cba", "collectibles"          )},
            { new KeyValuePair<string, string>("sya", "computers"             )},
            { new KeyValuePair<string, string>("ela", "electronics"           )},
            { new KeyValuePair<string, string>("gra", "farm+garden"           )},
            { new KeyValuePair<string, string>("zip", "free stuff"            )},
            { new KeyValuePair<string, string>("fua", "furniture"             )},
            { new KeyValuePair<string, string>("gms", "garage sales"          )},
            { new KeyValuePair<string, string>("foa", "general for sale"      )},
            { new KeyValuePair<string, string>("hsa", "household"             )},
            { new KeyValuePair<string, string>("wan", "items wanted"          )},
            { new KeyValuePair<string, string>("jwa", "jewelry"               )},
            { new KeyValuePair<string, string>("maa", "materials"             )},
            { new KeyValuePair<string, string>("mca", "motorcycles"           )},
            { new KeyValuePair<string, string>("msa", "musical instruments"   )},
            { new KeyValuePair<string, string>("pha", "photo+video"           )},
            { new KeyValuePair<string, string>("rva", "recreational vehicles" ) },
            { new KeyValuePair<string, string>("sga", "sporting goods"        )},
            { new KeyValuePair<string, string>("tia", "tickets"               )},
            { new KeyValuePair<string, string>("tla", "tools"                 )},
            { new KeyValuePair<string, string>("taa", "toys+games"            )},
            { new KeyValuePair<string, string>("vga", "video gaming"          )}
        };
    }
}

