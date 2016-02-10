using System;
using System.Collections.Generic;

namespace EthansList.Shared
{
    public class QueryGeneration
    {
        public string Generate(string url, string cat, Dictionary<string, string> searchTerms, Dictionary<object, object> conditions)
        {
            string result = String.Format("{0}{1}?format=rss", url, cat);
            foreach (KeyValuePair<string, string> kvp in searchTerms)
            {
                if (kvp.Value != null)
                {
                    var val = kvp.Value;
                    //HACK: Needs improvement for removing $
                    if (val.Length > 0 && val.Substring(0, 1) == "$")
                        val = val.Substring(1, val.Length - 1);
                    result += String.Format("&{0}={1}", kvp.Key, val);
                }
            }

            foreach (KeyValuePair<object, object> kvp in conditions)
            {
                if (kvp.Value != null)
                {
                    result += String.Format("&{0}={1}", kvp.Key, kvp.Value);
                }
            }

            return result;
        }
    }
}

