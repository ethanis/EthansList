using System;

namespace ethanslist.ios
{
    public class Location
    {
        public string Code;
        public string AreaId;
        public string Url;
        public string SiteName;
        public string State;
        public string Category;

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

