using System;
using System.Collections.Generic;

namespace EthansList.Models
{
    public class AboutEntry
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
    }

    public static class AboutFields
    {
        static readonly Abouts fields = new Abouts();
        public static List<AboutEntry> All { get { return fields.All; } }
    }

    public class Abouts
    {
        public List<AboutEntry> All = new List<AboutEntry>
        {
            new AboutEntry
            {
                Title = "Author",
                Description = "Ethan Dennis",
            },
            new AboutEntry
            {
                Title = "Tools",
                Description = "Built with C# using Xamarin Android",
                Link = "http://www.xamarin.com"
            }
        };
    }
}

