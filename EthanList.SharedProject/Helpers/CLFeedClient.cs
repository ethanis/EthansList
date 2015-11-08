using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.IO;
using System.Net;

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

        public CLFeedClient(String query)
        {
            postings = new List<Posting>();
            this.query = query;

//            XmlDocument xml = new XmlDocument();
//            xml.Load(query);
//            XDocument doc = XDocument.Parse(xml.InnerXml);
//
//            List<Posting> postings2 = new List<Posting>();
//            //TODO:try rdf instead of rss
//            postings2 = (from item in doc.Element("rdf").Element("channel").Elements("item")
//                select new Posting
//                {
//                    Title = item.Element("title").Value,
//                    Link = item.Element("link").Value,
//                    Description = item.Element("description").Value,
//                    ImageLink = item.Element("enclosure").Value,
//                }).ToList();
//
//            foreach (Posting posting in postings2)
//            {
//                Console.WriteLine(posting.Title);
//
//            }

            GetFeed();
//            GetFeedAsync();
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
            if (this.loadingComplete != null)
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
            AsyncXmlDocument = new XmlDocument();
            AsyncXmlDocument.Load(query);
            WireUpPostings();
        }

        private void WireUpPostings()
        {
            XmlNamespaceManager mgr = new XmlNamespaceManager(AsyncXmlDocument.NameTable);
            mgr.AddNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
            mgr.AddNamespace("x", "http://purl.org/rss/1.0/");
            mgr.AddNamespace("enc", "http://purl.oclc.org/net/rss_2.0/enc#");
//
            XmlNodeList rssNodes = AsyncXmlDocument.SelectNodes("//rdf:RDF/x:item", mgr);
//            StringBuilder rssContent = new StringBuilder();
//            XmlNodeList rssNodes2 = AsyncXmlDocument.SelectNodes("//rdf:RDF/x:item/enc:enc", mgr);
//
//            foreach (XmlNode rssNode2 in rssNodes2)
//            {
//                XmlNode rssSubNode = rssNode2.SelectSingleNode("enc:enclosure", mgr);
//                string link = rssSubNode != null ? rssSubNode.InnerText : "";
//                Console.WriteLine(link != null ? link : "null");
//            }

            // Iterate through the items in the RSS file
            foreach (XmlNode rssNode in rssNodes)
            {
                XmlNode rssSubNode = rssNode.SelectSingleNode("x:title", mgr);
                string title = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("x:link", mgr);
                string link = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("x:description", mgr);
                string description = rssSubNode != null ? rssSubNode.InnerText : "";

//                rssSubNode = rssNode.SelectSingleNode("enc:resource", mgr);
//                string imageLink = rssNode != null ? rssSubNode.InnerText : "";
////
//                Console.WriteLine(imageLink == null ? "null" : imageLink);

//                rssContent.Append("<a href='" + link + "'>" + title + "</a><br>" + description);

                if (title != null && description != null && description != null)
                {
                    Posting posting = new Posting();
                    posting.Title = title;
                    posting.Description = description;
                    posting.Link = link;
//                    posting.ImageLink = imageLink;

                    postings.Add(posting);
                }
            }    
        }

        public string GetTitle(int index)
        {
            return postings[index].Title;
        }

        public void GetFeed()
        {
//            try
//            {
//                XmlDocument xml = new XmlDocument();
//                xml.Load(query);
//                foreach (XmlNode Children in xml.ChildNodes)
//                {
//                    Console.WriteLine(Children.GetType());
//                    if (Children.Name.Equals("rss", StringComparison.CurrentCultureIgnoreCase))
//                    {
//                        Console.WriteLine(Children.GetType());
//                        foreach (XmlNode child in Children.ChildNodes)
//                        {
//                            Console.WriteLine(Children.InnerXml);
//                        }
//                    }
//                    else if (Children.Name.Equals("rdf:rdf", StringComparison.CurrentCultureIgnoreCase))
//                    {
////                        List<Posting> items = new List<Posting>();
//                        foreach (XmlNode child in Children.ChildNodes)
//                        {
//                            if (child.Name.Equals("item", StringComparison.CurrentCultureIgnoreCase))
//                            {
////                                Console.WriteLine(child.OuterXml);
//                                XmlNode rssSubNode = child.SelectSingleNode("title");
//                                string title = rssSubNode != null ? rssSubNode.InnerText : "";
//                                Console.WriteLine(title);
//
//                            }
//                        }
//                    }
//                }
//            }
//            catch
//            {
//            }

            WebClient wc = new WebClient();

            Stream st = wc.OpenRead(query);
            string rss;

            using (StreamReader sr = new StreamReader(st)) 
            {
                rss = sr.ReadToEnd();
            }
            Console.WriteLine(rss);
        }
    }
}

