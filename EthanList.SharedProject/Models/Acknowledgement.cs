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
        };
    }
}

