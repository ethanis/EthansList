using System;
using System.Xml;
using System.Text;
using Foundation;
using System.Collections.Generic;

namespace ethanslist.ios
{
    public class CLFeedClient
    {
        public List<Posting> postings;

        public CLFeedClient(String query)
        {
            postings = new List<Posting>();

            GetFeed(query);
        }

        private void GetFeed(String query)
        {
            // Parse the Items in the RSS file
            XmlDocument rssXmlDoc = new XmlDocument();
            rssXmlDoc.Load(query);

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

                if (title != null && description != null && description != null)
                {
                    Posting posting = new Posting();
                    posting.Title = title;
                    posting.Description = description;
                    posting.Link = link;

                    postings.Add(posting);
                }
            }
        }

        public string GetTitle(int index)
        {
            return postings[index].Title;
        }
    }
}

