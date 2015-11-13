using System;

namespace EthansList.Shared
{
    public class Posting
    {
        public Posting()
        {
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set;}
        public string ImageLink { get; set;}
        public DateTime Date { get; set; }
    }
}

