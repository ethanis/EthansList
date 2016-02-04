using System;
using System.Collections.Generic;

namespace EthansList.Shared
{
    public class QueryGeneration
    {
        public string Generate(string url, string cat, Dictionary<string, string> searchTerms)
        {
            string result = String.Format("{0}{1}?format=rss", url, cat);
            foreach (KeyValuePair<string, string> kvp in searchTerms)
            {
                if (kvp.Value != null)
                    result += String.Format("&{0}={1}", kvp.Key, kvp.Value);
            }

            return result;
        }
    }
}

