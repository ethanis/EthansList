using System;
using System.Xml;
using System.Text;
using Foundation;

namespace ethanslist.ios
{
    public class CLFeedClient
    {
        nint count = 0;
        string query;
        string[] titles = new string[100];

        public CLFeedClient(string query)
        {
            this.query = query;
        }

        public String GetFeed()
        {
            XmlDocument rssXmlDoc = new XmlDocument();
            rssXmlDoc.Load("http://sfbay.craigslist.org/search/sss?format=rss&query=" + query);

            return ReadRss(rssXmlDoc);
        }

        private string ReadRss(XmlDocument feed)
        {
            // Parse the Items in the RSS file
            XmlDocument rssXmlDoc = new XmlDocument();
            rssXmlDoc.Load("http://sfbay.craigslist.org/search/sss?format=rss&query=" + query);

//            XmlNodeList rssNodes = rssXmlDoc.DocumentElement.SelectNodes("item");

            XmlNamespaceManager mgr = new XmlNamespaceManager(rssXmlDoc.NameTable);
            mgr.AddNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
            mgr.AddNamespace("x", "http://purl.org/rss/1.0/");
            XmlNodeList rssNodes = rssXmlDoc.SelectNodes("//rdf:RDF/x:item", mgr);

            StringBuilder rssContent = new StringBuilder();

            // Iterate through the items in the RSS file
            foreach (XmlNode rssNode in rssNodes)
            {
                XmlNode rssSubNode = rssNode.SelectSingleNode("x:title", mgr);
                string title = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("x:link", mgr);
                string link = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("x:description", mgr);
                string description = rssSubNode != null ? rssSubNode.InnerText : "";

                rssContent.Append("<a href='" + link + "'>" + title + "</a><br>" + description);

                Console.WriteLine("Title: " + title);
                Console.WriteLine("Link: " + link);
                Console.WriteLine("Description" + description);
                Console.WriteLine("/b");


                titles[count] = title;
                count++;
            }
            // Return the string that contain the RSS items
            return rssContent.ToString();
        }

        public nint FeedLength()
        {
            return count;
        }

        public string GetTitle(int index)
        {
            return titles[index];
        }
    }
}

