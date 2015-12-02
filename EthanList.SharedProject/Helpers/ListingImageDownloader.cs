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

namespace EthansList.Shared
{
    public class ListingImageDownloader
    {
        string url;

        public ListingImageDownloader(string url)
        {
            this.url = url;
        }

        public List<string> GetAllImages()
        {
            List<string> links = new List<string>();

            WebClient client = new WebClient();
            string html = client.DownloadString(url);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            List<HtmlNode> imageNodes = null;
            imageNodes = (from HtmlNode node in doc.DocumentNode.Descendants()
                where node.Name == "a"
                && node.Attributes["id"] != null
                && node.Attributes["data-imgid"] != null
                select node).ToList();

            foreach(HtmlNode node in imageNodes)
            {
                links.Add(node.Attributes["href"].Value);
            }

            return links;
        }
    }
}