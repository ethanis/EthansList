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
using System.Diagnostics;

namespace EthansList.Shared
{
    public class CLFeedClient
    {
        public List<Posting> postings;
        private string query;
        private int pageCounter = 25;
        readonly int MaxListings;
        private int? WeeksOld;

        public EventHandler<EventArgs> asyncLoadingComplete;
        public EventHandler<EventArgs> emptyPostingComplete;
        public EventHandler<EventArgs> asyncLoadingPartlyComplete;


        public CLFeedClient(String query, int MaxListings = 25, int? WeeksOld = null)
        {
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

        public bool GetAllPostingsAsync()
        {
            get_craigslist_postings(query);
            return true;
        }

        private string DownloadString(string add)
        {
            string html = "";            
            using (WebClient client = new WebClient())
            {
                client.Proxy = null;
                while (html == "")
                {
                    try
                    {
                        html = client.DownloadString(add);
                    }
                    catch (WebException e)
                    {
                        html = "";
                        Console.WriteLine(e.InnerException);
                    }
                }
                client.Dispose();
            }
            return html;
        }

        private void get_craigslist_postings(string query)
        {
            Stopwatch timer = Stopwatch.StartNew();
            Task.Factory.StartNew<string>(() => DownloadString(query))
                .ContinueWith(t => {
//                    string html = t.Result;
//                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
//                    doc.LoadHtml(html);
//                    HtmlNode root = doc.DocumentNode; 
                    XmlDocument xmldocument = new XmlDocument();
                    xmldocument.LoadXml(t.Result);

                    var done = ParsePostings(xmldocument);
                    Console.WriteLine (done);
                    timer.Stop();
                    TimeSpan timespan = timer.Elapsed;
                    Console.WriteLine (timespan.ToString());
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private bool ParsePostings(XmlDocument xmldocument)
        {
            XmlNamespaceManager mgr = new XmlNamespaceManager(xmldocument.NameTable);
            mgr.AddNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
            mgr.AddNamespace("x", "http://purl.org/rss/1.0/");

            XmlNodeList rssNodes = xmldocument.SelectNodes("//rdf:RDF/x:item", mgr);
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


                    //                    if (!postings.Exists(c => c.Link.Equals(posting.Link)))
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
                if (this.asyncLoadingPartlyComplete != null)
                    this.asyncLoadingPartlyComplete(this, new EventArgs());
                this.query += String.Format("&s={0}", pageCounter);
                get_craigslist_postings(this.query);
                pageCounter += 25;
            }
            else
            {
                if (this.asyncLoadingComplete != null)
                    this.asyncLoadingComplete(this, new EventArgs());

                if (postings.Count == 0 && this.emptyPostingComplete != null)
                    this.emptyPostingComplete(this, new EventArgs());
            }
            
            return true;
        }
    }
}

