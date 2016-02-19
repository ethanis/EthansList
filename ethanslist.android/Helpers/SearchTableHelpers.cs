using System;

namespace ethanslist.android
{
    public class SearchRow
    {
        public string Title {get;set;}
        public SearchRowTypes RowType {get;set;}
    }

    public enum SearchRowTypes
    {
        Heading,
        SearchTerms,
        Price,
        SqFeet,
        Odometer
    }
}

