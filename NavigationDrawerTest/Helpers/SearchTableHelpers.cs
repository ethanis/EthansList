using System;
using System.Collections.Generic;

namespace EthansList.MaterialDroid
{
    public class SearchRow
    {
        public string Title {get;set;}
        public SearchRowTypes RowType {get;set;}
        public NumberPickerOptions NumberPickerOptions { get; set; }
        public List<KeyValuePair<object,object>> ComboPickerOptions { get; set; }
        public string QueryPrefix { get; set; }
    }

    public class NumberPickerOptions
    { 
        public int Initial { get; set; }
        public int Minimum { get; set; }
        public int Maximum { get; set; }
        public int Step { get; set; }
        public string DisplaySuffix { get; set; }
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

