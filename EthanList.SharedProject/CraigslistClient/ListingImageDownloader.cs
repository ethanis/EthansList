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
using System.Diagnostics;

namespace EthansList.Shared
{
    public class ListingImageDownloader
    {
        readonly string url;
        readonly string rssImageUrl;
        public string postingDescription;
        public List<string> images = new List<string>();

        public EventHandler<EventArgs> loadingComplete;

        public ListingImageDownloader(string url, string rssImageUrl)
        {
            this.url = url;
            this.rssImageUrl = rssImageUrl;
        }

        public bool GetAllImagesAsync()
        {
            if (Reachability.Reachability.IsHostReachable("http://www.craigslist.org"))
            {
                GetImages();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void GetImages()
        {
            Stopwatch timer = Stopwatch.StartNew();
            Task.Factory.StartNew<string>(() => DownloadString(url))
                .ContinueWith(t => {
                    //                    string html = t.Result;
                    //                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    //                    doc.LoadHtml(html);
                    //                    HtmlNode root = doc.DocumentNode; 
                    ParseHtmlForImages(t.Result);
                    timer.Stop();
                    TimeSpan timespan = timer.Elapsed;
                    Console.WriteLine (timespan.ToString());
                }, TaskScheduler.FromCurrentSynchronizationContext());
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

        private void ParseHtmlForImages(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

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

            if (this.loadingComplete != null)
                this.loadingComplete(this, new EventArgs());
        }
    }
}