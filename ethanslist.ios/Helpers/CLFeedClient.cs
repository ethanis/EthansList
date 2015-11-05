using System;
using System.Xml;
using System.Text;
using Foundation;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ethanslist.ios
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

            XmlNodeList rssNodes = AsyncXmlDocument.SelectNodes("//rdf:RDF/x:item", mgr);

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

