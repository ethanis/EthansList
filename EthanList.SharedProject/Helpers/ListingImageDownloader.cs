using System;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using HtmlAgilityPack;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel;

namespace EthansList.Shared
{
    public class ListingImageDownloader
    {
        readonly string url;
        readonly string rssImageUrl;
        public List<string> images = new List<string>();
        public string postingDescription;

        private static BackgroundWorker AsyncHtmlLoader;
        public EventHandler<EventArgs> loadingComplete;
        public EventHandler<EventArgs> loadingProgressChanged;

        public ListingImageDownloader(string url, string rssImageUrl)
        {
            this.url = url;
            this.rssImageUrl = rssImageUrl;
//            if (rssImageUrl != null || rssImageUrl != "-1")
//                images.Add(rssImageUrl);
            
            GetImagesAsync();
        }

        private void GetImagesAsync()
        {
            AsyncHtmlLoader = new BackgroundWorker();
            AsyncHtmlLoader.WorkerReportsProgress = true;

            AsyncHtmlLoader.ProgressChanged += AsyncXmlLoader_ProgressChanged;;
            AsyncHtmlLoader.DoWork += AsyncXmlLoader_DoWork;
            AsyncHtmlLoader.RunWorkerCompleted += AsyncXmlLoader_RunWorkerCompleted;

            AsyncHtmlLoader.RunWorkerAsync();
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
            Console.WriteLine(images.Count);
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
                AsyncHtmlLoader.ReportProgress(i); 
            }
//            AsyncXmlDocument = new XmlDocument();
//            AsyncXmlDocument.Load(url);
            WebClient client = new WebClient();
            string html = client.DownloadString(new Uri(url));

            ParseHtmlForImages(html);
        }

        public void ParseHtmlForImages(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
//            Console.WriteLine(html);
            List<HtmlNode> imageNodes = null;
            imageNodes = (from HtmlNode node in doc.DocumentNode.Descendants()
                where node.Name == "a"
                && node.Attributes["id"] != null
                && node.Attributes["data-imgid"] != null
                select node).ToList();
            foreach (HtmlNode node in imageNodes)
            {
                images.Add(node.Attributes["href"].Value);
            }

            postingDescription = doc.GetElementbyId("postingbody").InnerText;

            if (images.Count == 0 && rssImageUrl != "-1")
                images.Insert(0, rssImageUrl);
        }
    }
}