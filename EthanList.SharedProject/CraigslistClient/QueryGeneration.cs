using System;
using System.Collections.Generic;

namespace EthansList.Shared
{
    public class QueryGeneration
    {
        public string Generate(SearchObject search)
        {
            string result = String.Format("{0}{1}?format=rss", search.SearchLocation.Url, search.Category.Key);
            foreach (KeyValuePair<string, string> kvp in search.SearchItems)
            {
                if (kvp.Value != null && kvp.Value.Length > 0)
                {
                    var val = kvp.Value;
                    //HACK: Needs improvement for removing $
                    if (val.Length > 0 && val.Substring(0, 1) == "$")
                        val = val.Substring(1, val.Length - 1);
                    result += String.Format("&{0}={1}", kvp.Key, val);
                }
            }

            foreach (KeyValuePair<object, KeyValuePair<object, object>> kvp in search.Conditions)
            {
                if (kvp.Value.Key != null)
                {
                    result += String.Format("&{0}={1}", kvp.Value.Key, kvp.Value.Value);
                }
            }

            return result;
        }
    }
}

