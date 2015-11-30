using System;
using System.Net;
using System.IO;
using System.Text;

namespace EthansList.Shared
{
    public class ListingImageDownloader
    {
        string url;

        public ListingImageDownloader(string url)
        {
            this.url = url;
        }

        public string GetAllImages()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string htmlData = "nothing here";

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                htmlData = readStream.ReadToEnd();

                response.Close();
                readStream.Close();
//                Console.WriteLine(data);
            }

            return htmlData;
        }
    }
}

