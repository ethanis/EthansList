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
//            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
//            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
//            string htmlData = "nothing yet";
//
//
//            if (response.StatusCode == HttpStatusCode.OK)
//            {
//                
//                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
//                {
//                    links = FetchLinksFromSource(sr.ReadToEnd());
//                }
//                foreach (Uri link in links)
//                {
//                    Console.WriteLine(link);
//                }
//                Stream receiveStream = response.GetResponseStream();
//                StreamReader readStream = null;
//
//                if (response.CharacterSet == null)
//                {
//                    readStream = new StreamReader(receiveStream);
//                }
//                else
//                {
//                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
//                }
//
//                htmlData = readStream.ReadToEnd();
//
//                response.Close();
//                readStream.Close();
//
//                XmlDocument xdoc = new XmlDocument();
//                xdoc.LoadXml(htmlData);
//                Console.WriteLine(xdoc.InnerXml);
//            }
//
//            HtmlWeb web = new HtmlWeb();
//            HtmlDocument doc = web.LoadFromWebAsync(url).Result;
//            doc.LoadHtml(htmlData);
//            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//a"))
//            {
//                
//            }
//                
//            HtmlDocument doc = new HtmlDocument();
//            doc.LoadHtml(htmlData);
//            var des = doc.DocumentNode.Descendants("a");
//            foreach (var d in des)
//            {
//                Console.WriteLine(d.Attributes["href"].Value);
//            }

            // For speed of dev, I use a WebClient
            WebClient client = new WebClient();
            string html = client.DownloadString(url);

            // Load the Html into the agility pack
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Now, using LINQ to get all Images
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
//
//        public List<Uri> FetchLinksFromSource(string htmlSource)
//        {
//            List<Uri> links = new List<Uri>();
//            string regexImgSrc = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
//            MatchCollection matchesImgSrc = Regex.Matches(htmlSource, regexImgSrc, RegexOptions.IgnoreCase | RegexOptions.Singleline);
//            foreach (Match m in matchesImgSrc)
//            {
//                string href = m.Groups[1].Value;
//                Console.WriteLine(href);
//                links.Add(new Uri(href));
//            }
//            return links;
//        }
    }
}

