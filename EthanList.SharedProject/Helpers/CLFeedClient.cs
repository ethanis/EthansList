using System;
using System.Linq;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using EthansList.Models;
using System.Net;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace EthansList.Shared
{
    public class CLFeedClient
    {
        public List<Posting> postings;
        private string query;
        private static BackgroundWorker AsyncXmlLoader;
        private static XmlDocument AsyncXmlDocument;
        public EventHandler<EventArgs> loadingComplete;
        public EventHandler<EventArgs> emptyPostingComplete;
        private int pageCounter = 25;
        readonly int MaxListings;
        private int? WeeksOld;

        public CLFeedClient(String query, int MaxListings = 25, int? WeeksOld = null)
        {
            //HACK: This needs to be disposed better before release
            postings = new List<Posting>();
            this.query = query;
            this.MaxListings = MaxListings;
            this.WeeksOld = WeeksOld;
            if (WeeksOld == -1)
            {
                this.query += "&postedToday=1";
                this.WeeksOld = null;
            }
        }

        public void GetPostings()
        {
            GetFeedAsync();
        }

        private void GetFeedAsync()
        {
            AsyncXmlLoader = new BackgroundWorker();
            AsyncXmlLoader.WorkerReportsProgress = true;
            AsyncXmlLoader.DoWork += AsyncXmlLoader_DoWork;
            AsyncXmlLoader.RunWorkerCompleted += AsyncXmlLoader_RunWorkerCompleted;

            AsyncXmlLoader.RunWorkerAsync();
        }

        void AsyncXmlLoader_RunWorkerCompleted (object sender, RunWorkerCompletedEventArgs e)
        {
            if (postings.Count == 0 && this.emptyPostingComplete != null)
            {
                this.emptyPostingComplete(this, new EventArgs());
            }
            else if (this.loadingComplete != null)
            {
                this.loadingComplete(this, new EventArgs());
            }
        }

        void AsyncXmlLoader_DoWork (object sender, DoWorkEventArgs e)
        {
            AsyncXmlDocument = new XmlDocument();
            WebClient client = new WebClient();
            string html = client.DownloadString(new Uri(query));

            AsyncXmlDocument.LoadXml(html);
            WireUpPostings();
        }

        private void WireUpPostings()
        {
            XmlNamespaceManager mgr = new XmlNamespaceManager(AsyncXmlDocument.NameTable);
            mgr.AddNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
            mgr.AddNamespace("x", "http://purl.org/rss/1.0/");

            XmlNodeList rssNodes = AsyncXmlDocument.SelectNodes("//rdf:RDF/x:item", mgr);
            bool incremented = false;
            // Iterate through the items in the RSS file
            foreach (XmlNode rssNode in rssNodes)
            {
                string imageLink = "-1";
                DateTime date = new DateTime();
                foreach (XmlNode child in rssNode.ChildNodes)
                {
                    if (child.Name == "enc:enclosure")
                    {
                        string pot = child.Attributes["resource"].Value;
                        imageLink = pot != null ? pot : "-1";
                    }

                    if (child.Name == "dc:date")
                    {
                        date = DateTime.Parse(child.InnerText);
                    }
                }

                XmlNode rssSubNode = rssNode.SelectSingleNode("x:title", mgr);
                string title = rssSubNode != null ? rssSubNode.InnerText : "";
                if (title.Length > 12 && title.Substring(title.Length - 12) == "<sup>2</sup>")
                {
                    title = title.Remove(title.Length - 12);
                }

                rssSubNode = rssNode.SelectSingleNode("x:link", mgr);
                string link = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("x:description", mgr);
                string description = rssSubNode != null ? rssSubNode.InnerText : "";

                if (title != null)
                {
                    Posting posting = new Posting();
                    posting.PostTitle = System.Net.WebUtility.HtmlDecode(title);
                    posting.Description = System.Net.WebUtility.HtmlDecode(description);
                    posting.Link = link;
                    posting.ImageLink = imageLink;
                    posting.Date = date;


                    if (!postings.Exists(c => c.Link.Equals(posting.Link)))
                    {
                        if (WeeksOld != null && DateTime.Compare(date, DateTime.Today.AddDays(Convert.ToDouble(-7 * WeeksOld))) != -1)
                        {
                            postings.Add(posting);
                            incremented = true;
                        }
                        else if (WeeksOld == null)
                        {
                            postings.Add(posting);
                            incremented = true;
                        }
                    }
                }
            }

            if (postings.Count >= 25 && postings.Count < MaxListings && incremented)
            {
                this.query += String.Format("&s={0}", pageCounter);
                GetPostings();
                pageCounter += 25;
            }
            Console.WriteLine(postings.Count);
        }
    }
}

