using System;
using System.Collections.Generic;

namespace EthansList.Shared
{
    public class QueryGeneration
    {
        public string Generate(string url, Dictionary<string, string> searchTerms)
        {
            string result = url + "/search/apa?format=rss";
            foreach (KeyValuePair<string, string> kvp in searchTerms)
            {
                if (kvp.Value != null)
                    result += String.Format("&{0}={1}", kvp.Key, kvp.Value);
            }

            return result;
        }
    }
}

