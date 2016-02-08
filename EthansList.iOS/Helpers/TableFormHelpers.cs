using System;
using System.Collections.Generic;
using UIKit;

namespace ethanslist.ios
{
    /// <summary>
    /// A group that contains table items
    /// </summary>
    public class TableItemGroup
    {
        public string Name { get; set; }

        public string Footer { get; set; }

        public List<TableItem> Items
        {
            get { return items; }
            set { items = value; }
        }
        protected List<TableItem> items = new List<TableItem> ();
    }

    /// <summary>
    /// Represents our item in the table
    /// </summary>
    public class TableItem
    {
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public string ImageName { get; set; }
        public string CellType { get; set; }

        public Dictionary<string, object> ActionOptions 
        { 
            get { return actionOptions;} 
            set { actionOptions = value; } 
        }
        protected Dictionary<string, object> actionOptions = new Dictionary<string, object>();

        public UITableViewCellStyle CellStyle
        {
            get { return cellStyle; }
            set { cellStyle = value; }
        }
        protected UITableViewCellStyle cellStyle = UITableViewCellStyle.Default;

        public UITableViewCellAccessory CellAccessory
        {
            get { return cellAccessory; }
            set { cellAccessory = value; }
        }
        protected UITableViewCellAccessory cellAccessory = UITableViewCellAccessory.None;

        public TableItem () { }

        public TableItem (string heading)
        { this.Heading = heading; }

        public List<PickerOptions> PickerOptions { get; set; }
    }

    /// <summary>
    /// Represents our group in the Picker View
    /// </summary>
    public class PickerViewGroup
    {
        public List<PickerOptions> Options
        {
            get { return items; }
            set { items = value; }
        }
        protected List<PickerOptions> items = new List<PickerOptions> ();
    }

    /// <summary>
    /// Represents our component in the Picker View
    /// </summary>
    public class PickerOptions
    {
        public Dictionary<object, object> Options
        {
            get { return options; }
            set { options = value; }
        }
        protected Dictionary<object, object> options = new Dictionary<object, object>();

        public List<KeyValuePair<object, object>> PickerWheelOptions
        {
            get {return pickerWheelOptions;}
            set {pickerWheelOptions = value;}
        }
        protected List<KeyValuePair<object, object>> pickerWheelOptions = new List<KeyValuePair<object, object>>();

    }
}

