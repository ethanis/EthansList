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

namespace EthansList.Shared
{
    public class CLFeedClient
    {
        public List<Posting> postings;
        readonly string query;
        private static BackgroundWorker AsyncXmlLoader;
        private static XmlDocument AsyncXmlDocument;
        public EventHandler<EventArgs> loadingComplete;
        public EventHandler<EventArgs> loadingProgressChanged;
        public EventHandler<EventArgs> emptyPostingComplete;

        public CLFeedClient(String query)
        {
            postings = new List<Posting>();
            this.query = query;
        }

        public void GetPostings()
        {
            GetFeedAsync();
        }

        private void GetFeedAsync()
        {
            AsyncXmlLoader = new BackgroundWorker();
            AsyncXmlLoader.WorkerReportsProgress = true;
            
            AsyncXmlLoader.ProgressChanged += AsyncXmlLoader_ProgressChanged;;
            AsyncXmlLoader.DoWork += AsyncXmlLoader_DoWork;
            AsyncXmlLoader.RunWorkerCompleted += AsyncXmlLoader_RunWorkerCompleted;

            AsyncXmlLoader.RunWorkerAsync();
        }

        void AsyncXmlLoader_ProgressChanged (object sender, ProgressChangedEventArgs e)
        {
            if (this.loadingProgressChanged != null)
            {
                this.loadingProgressChanged(this, new EventArgs());
            }
        }

        void AsyncXmlLoader_RunWorkerCompleted (object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine(postings.Count);
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
            //TODO: have this report actual progress done
            for (int i = 10; i <= 100; i+=10)
            {
                AsyncXmlLoader.ReportProgress(i); 
            }
//            AsyncXmlDocument = new XmlDocument();
//            AsyncXmlDocument.Load(query);
//            WireUpPostings();

            WebClient client = new WebClient();
            string html = client.DownloadString(new Uri(query));

            ParseHtmlForPostings(html);
        }

        public void ParseHtmlForPostings(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            List<HtmlNode> itemNodes = null;
            itemNodes = (from HtmlNode node in doc.DocumentNode.Descendants()
                where node.Name == "item"
                select node).ToList();

            foreach (HtmlNode node in itemNodes)
            {
                string imageLink = "-1";
                string title = String.Empty;
                string description = String.Empty;
                string link = string.Empty;
                DateTime date = new DateTime();

                foreach (HtmlNode child in node.ChildNodes)
                {

                        if (child.Name == "title")
                        {
                            string pot = child.InnerText;
                            title = pot != null ? pot : "";
                        }
                        if (child.Name == "description")
                        {
                            string pot = child.InnerText;
                            description = pot != null ? pot : "";
                        }
                        if (child.Name == "link")
                        {
                            string pot = child.InnerText;
                            link = pot != null ? pot : "";
                        }

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

                {
                    Posting posting = new Posting();
                    posting.PostTitle = System.Net.WebUtility.HtmlDecode(title);
                    posting.Description = System.Net.WebUtility.HtmlDecode(description);
                    posting.Link = link;
                    posting.ImageLink = imageLink;
                    posting.Date = date;

                    if (!postings.Exists(c => c.Serialized == posting.Serialized))
                        postings.Add(posting);
                }
            }
        }
//
//        private void WireUpPostings()
//        {
//            XmlNamespaceManager mgr = new XmlNamespaceManager(AsyncXmlDocument.NameTable);
//            mgr.AddNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
//            mgr.AddNamespace("x", "http://purl.org/rss/1.0/");
//
//            XmlNodeList rssNodes = AsyncXmlDocument.SelectNodes("//rdf:RDF/x:item", mgr);
//
//            // Iterate through the items in the RSS file
//            foreach (XmlNode rssNode in rssNodes)
//            {
//                string imageLink = "-1";
//                string title = String.Empty;
//                string description = String.Empty;
//                string link = string.Empty;
//                DateTime date = new DateTime();
//                foreach (XmlNode child in rssNode.ChildNodes)
//                {
//                    if (child.Name == "title")
//                    {
//                        string pot = child.InnerText;
//                        title = pot != null ? pot : "";
//                    }
//                    if (child.Name == "description")
//                    {
//                        string pot = child.InnerText;
//                        description = pot != null ? pot : "";
//                    }
//                    if (child.Name == "link")
//                    {
//                        string pot = child.InnerText;
//                        link = pot != null ? pot : "";
//                    }
//
//                    if (child.Name == "enc:enclosure")
//                    {
//                        string pot = child.Attributes["resource"].Value;
//                        imageLink = pot != null ? pot : "-1";
////                        Console.WriteLine(imageLink);
//                    }
//
//                    if (child.Name == "dc:date")
//                    {
//                        date = DateTime.Parse(child.InnerText);
//                    }
////                    Console.WriteLine(child.Name + " = " + child.InnerText);
//                }
//
////                XmlNode rssSubNode = rssNode.SelectSingleNode("x:title", mgr);
////                string title = rssSubNode != null ? rssSubNode.InnerText : "";
////
////                rssSubNode = rssNode.SelectSingleNode("x:link", mgr);
////                string link = rssSubNode != null ? rssSubNode.InnerText : "";
////
////                rssSubNode = rssNode.SelectSingleNode("x:description", mgr);
////                string description = rssSubNode != null ? rssSubNode.InnerText : "";
//
//                if (title != null)
//                {
//                    Posting posting = new Posting();
//                    posting.PostTitle = System.Net.WebUtility.HtmlDecode(title);
//                    posting.Description = System.Net.WebUtility.HtmlDecode(description);
//                    posting.Link = link;
//                    posting.ImageLink = imageLink;
//                    posting.Date = date;
//
//                    if (!postings.Exists(c => c.Serialized == posting.Serialized))
//                        postings.Add(posting);
//                }
//            }
//        }

        public string GetTitle(int index)
        {
            return postings[index].PostTitle;
        }
    }
}

