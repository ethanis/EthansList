using System;

namespace EthansList.Shared
{
    public class Location
    {
        public string Code { get; set;}
        public string AreaId { get; set;}
        public string Url { get; set;}
        public string SiteName { get; set;}
        public string State { get; set;}
        public string Category { get; set;}

        public Location(string Code, String AreaId, String Url, String SiteName, String State, String Category)
        {
            this.Code = Code;
            this.AreaId = Code;
            this.Url = Url;
            this.SiteName = SiteName;
            this.State = State;
            this.Category = Category;
        }            
    }
}

