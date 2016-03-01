using System;

namespace EthansList.MaterialDroid
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
        SingleEntryLabel,
        PriceDoubleEntry,
        DoubleEntry,
        Space,
        SinglePicker,
        ComboPicker
    }
}

