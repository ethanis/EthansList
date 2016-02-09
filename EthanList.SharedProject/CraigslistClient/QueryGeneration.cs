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
                {
                    var val = kvp.Value;
                    if (val.Length > 0 && val.Substring(0, 1) == "$")
                        val = val.Substring(1, val.Length - 1);
                    result += String.Format("&{0}={1}", kvp.Key, val);
                }
            }

            return result;
        }
    }
}

