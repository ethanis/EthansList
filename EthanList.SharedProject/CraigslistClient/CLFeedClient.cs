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
using System.Diagnostics;

namespace EthansList.Shared
{
    public class CLFeedClient : IDisposable
    {
        public List<Posting> postings { get; private set; }
        private string query;
        private int pageCounter = 25;
        readonly int MaxListings;
        private int? WeeksOld;
        private int dup = 0;

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
            Console.WriteLine(this.query);
            if (Reachability.Reachability.IsNetworkAvailable())
            {
                get_craigslist_postings(query);
                return true;
            }
            else
            {
                return false;
            }
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
            Console.WriteLine(query);
            Stopwatch timer = Stopwatch.StartNew();
            Task.Factory.StartNew<string>(() => DownloadString(query))
                .ContinueWith(t =>
                {
                    //                    string html = t.Result;
                    //                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    //                    doc.LoadHtml(html);
                    //                    HtmlNode root = doc.DocumentNode; 
                    XmlDocument xmldocument = new XmlDocument();
                    try
                    {
                        xmldocument.LoadXml(t.Result);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        if (this.emptyPostingComplete != null)
                            this.emptyPostingComplete(this, new EventArgs());
                        return;
                    }
                    var done = ParsePostings(xmldocument);
                    Console.WriteLine(done);
                    timer.Stop();
                    TimeSpan timespan = timer.Elapsed;
                    Console.WriteLine(timespan.ToString());
                }, TaskScheduler.FromCurrentSynchronizationContext()).ConfigureAwait(false);
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

                    lock (postings)
                    {
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
                        else
                        {
                            Console.WriteLine("duplicate# " + dup);
                            dup++;
                        }
                    }
                }
            }

            if (postings.Count >= 25 && postings.Count < MaxListings && incremented)
            {
                if (this.asyncLoadingPartlyComplete != null)
                    this.asyncLoadingPartlyComplete(this, new EventArgs());

                //                System.Threading.Thread.Sleep(2000);
                get_craigslist_postings(String.Format("{1}&s={0}", pageCounter, this.query));
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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    asyncLoadingComplete = null;
                    asyncLoadingPartlyComplete = null;
                    emptyPostingComplete = null;
                }

                postings = null;

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}

