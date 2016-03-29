using System;
using System.Collections.Generic;

namespace EthansList.Models
{
    public class Acknowledgement
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
    }

    public static class Acknowledgements
    {
        static readonly Acks helper = new Acks();
        public static List<Acknowledgement> All { get { return helper.All; } }
    }

    public class Acks
    {
        public List<Acknowledgement> All = new List<Acknowledgement>
        {
            new Acknowledgement
            {
                Title = "UrlImageViewHelper Android",
                Description = "UrlImageViewHelper is an Android library developed by Koushik Dutta that sets an ImageView's contents from a url. It manages image downloading, caching, and makes your coffee too. UrlImageViewHelper will fill an ImageView with an image that is found at a URL.",
                Link = "https://components.xamarin.com/view/urlimageviewhelper"
            },
            new Acknowledgement
            {
                Title = "PullToRefresharp",
                Description = "PullToRefresharp is the only C# library that provides pull-to-refresh functionality to ListView, GridView, and ScrollView on Android. It is simple to integrate with, customizable and extensible.",
                Link = "https://components.xamarin.com/view/pulltorefresharp"
            },
            new Acknowledgement
            {
                Title = "HTML Agility Pack",
                Description = "This is an agile HTML parser that builds a read/write DOM and supports plain XPATH or XSLT (you actually don't HAVE to understand XPATH nor XSLT to use it, don't worry...). It is a .NET code library that allows you to parse \"out of the web\" HTML files. The parser is very tolerant with \"real world\" malformed HTML. The object model is very similar to what proposes System.Xml, but for HTML documents (or streams). ",
                Link = "https://htmlagilitypack.codeplex.com/"
            },
            new Acknowledgement
            {
                Title = "Json.NET",
                Description = "Popular high-performance JSON framework for .NET",
                Link = "http://www.newtonsoft.com/json"
            },
            new Acknowledgement
            {
                Title = "SQLite-Net",
                Description = "SQLite-net is an open source and light weight library providing easy SQLite database storage for .NET, Mono, and Xamarin applications. This version uses SQLitePCL.raw_basic to provide platform independent versions of SQLite.",
                Link = "https://github.com/praeclarum/sqlite-net"
            },
        };
    }
}

