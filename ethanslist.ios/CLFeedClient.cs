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
            XmlNodeList rssNodes = feed.SelectNodes("channel");

//            XmlDocument rssXmlDoc = new XmlDocument();
//
//            // Load the RSS file from the RSS URL
//            rssXmlDoc.Load("http://feeds.feedburner.com/techulator/articles");
//
//            // Parse the Items in the RSS file
//            XmlNodeList rssNodes = rssXmlDoc.SelectNodes("rss/channel/item");

            StringBuilder rssContent = new StringBuilder();

            Console.WriteLine(feed);

            // Iterate through the items in the RSS file
            foreach (XmlNode rssNode in rssNodes)
            {
                XmlNode rssSubNode = rssNode.SelectSingleNode("title");
                string title = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("link");
                string link = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("description");
                string description = rssSubNode != null ? rssSubNode.InnerText : "";

                rssContent.Append("<a href='" + link + "'>" + title + "</a><br>" + description);

                Console.WriteLine(title);

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

