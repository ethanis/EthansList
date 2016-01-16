using System;
using UIKit;
using System.Collections.Generic;
using Foundation;
using System.Threading.Tasks;

namespace ethanslist.ios
{
    public class SearchOptionsTableSource : UITableViewSource
    {
        protected List<TableItemGroup> tableItems;
        protected string cellIdentifier = "TableCell";
        UIViewController owner;
        public EventHandler<EventArgs> actionSheetSelected;

        protected SearchOptionsTableSource() {}

        public SearchOptionsTableSource (List<TableItemGroup> items, UIViewController owner)
        {
            tableItems = items;
            this.owner = owner;
        }

        #region -= data binding/display methods =-

        /// <summary>
        /// Called by the TableView to determine how many sections(groups) there are.
        /// </summary>
        public override nint NumberOfSections (UITableView tableView)
        {
            return tableItems.Count;
        }

        /// <summary>
        /// Called by the TableView to determine how many cells to create for that particular section.
        /// </summary>
        public override nint RowsInSection (UITableView tableview, nint section)
        {
            return tableItems[(int)section].Items.Count;
        }

        /// <summary>
        /// Called by the TableView to retrieve the header text for the particular section(group)
        /// </summary>
        public override string TitleForHeader (UITableView tableView, nint section)
        {
            return tableItems[(int)section].Name;
        }

        /// <summary>
        /// Called by the TableView to retrieve the footer text for the particular section(group)
        /// </summary>
        public override string TitleForFooter (UITableView tableView, nint section)
        {
            return tableItems[(int)section].Footer;
        }

        #endregion

        #region -= user interaction methods =-

        public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
        {
            //            new UIAlertView("Row Selected"
            //                , tableItems[indexPath.Section].Items[indexPath.Row].Heading, null, "OK", null).Show();
        }

        public override void RowDeselected (UITableView tableView, NSIndexPath indexPath)
        {
            Console.WriteLine("Row " + indexPath.Row.ToString() + " deselected");   
        }

        public override void AccessoryButtonTapped (UITableView tableView, NSIndexPath indexPath)
        {
            Console.WriteLine("Accessory for Section, " + indexPath.Section.ToString() + " and Row, " + indexPath.Row.ToString() + " tapped");
        }

        #endregion

        /// <summary>
        /// Called by the TableView to get the actual UITableViewCell to render for the particular section and row
        /// </summary>
        public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            // declare vars
            TableItem item = tableItems[indexPath.Section].Items[indexPath.Row];
            UITableViewCell cell = null;
            if (item.CellType == "SearchTermsCell")
            {
                cell = SearchTermsCell.Create();
                ((SearchTermsCell)cell).TermsField.EditingChanged += delegate
                {
                    ((SearchOptionsViewController)(this.owner)).SearchTerms = ((SearchTermsCell)cell).TermsField.Text;
                };
            }
            else if (item.CellType == "PriceSelectorCell")
            {
                cell = PriceSelectorCell.Create();
                ((PriceSelectorCell)cell).LabelText = item.Heading;
                ((PriceSelectorCell)cell).MinPrice.EditingChanged += delegate
                {
                    ((SearchOptionsViewController)(this.owner)).MinPrice = ((PriceSelectorCell)cell).MinPrice.Text;
                };
                ((PriceSelectorCell)cell).MaxPrice.EditingChanged += delegate
                {
                    ((SearchOptionsViewController)(this.owner)).MaxPrice = ((PriceSelectorCell)cell).MaxPrice.Text;
                };
            }
            else if (item.CellType == "BedBathCell")
            {
                cell = BedBathCell.Create();

                ((BedBathCell)cell).Title = item.Heading;

                if (item.SubHeading != null)
                    ((BedBathCell)cell).MinimumLabel.Text = item.SubHeading;
                
                UITapGestureRecognizer tap = new UITapGestureRecognizer(async () =>
                    {
                        var result = await ShowNumberOptions(this.owner, item.Heading, "Select an option below", item.ActionOptions);
                        Console.WriteLine(result);
                        ((BedBathCell)cell).MinimumLabel.Text = result;
                        if (item.Heading == "Min Bedrooms")
                            ((SearchOptionsViewController)(this.owner)).MinBedrooms = item.ActionOptions[result];
                        else if (item.Heading == "Min Bathrooms")
                            ((SearchOptionsViewController)(this.owner)).MinBathrooms = item.ActionOptions[result];
                        else if (item.Heading == "Max Listings")
                            ((SearchOptionsViewController)(this.owner)).MaxListings = Convert.ToInt32(item.ActionOptions[result]);
                    });

                ((BedBathCell)cell).MinimumLabel.AddGestureRecognizer(tap);
            }

            return cell;
        }

        public static Task<String> ShowNumberOptions(UIViewController parent, string strTitle, string strMsg, Dictionary<string, string> options)
        {
            var taskCompletionSource = new System.Threading.Tasks.TaskCompletionSource<string>();

            UIAlertController actionSheetAlert = UIAlertController.Create(strTitle, strMsg, UIAlertControllerStyle.ActionSheet);

            foreach (KeyValuePair<string, string> option in options)
            {
                actionSheetAlert.AddAction(UIAlertAction.Create(option.Key,UIAlertActionStyle.Default, (a) => taskCompletionSource.SetResult(option.Key)));
            }

            actionSheetAlert.AddAction(UIAlertAction.Create("Cancel",UIAlertActionStyle.Cancel, (action) => Console.WriteLine ("Cancel button pressed.")));

            // Required for iPad - You must specify a source for the Action Sheet since it is
            // displayed as a popover
            UIPopoverPresentationController presentationPopover = actionSheetAlert.PopoverPresentationController;
            if (presentationPopover!=null) {
                presentationPopover.SourceView = parent.View;
                presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
            }
            // Display the alert
            parent.PresentViewController(actionSheetAlert,true,null);
            return taskCompletionSource.Task;
        }
    }

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

        public Dictionary<string, string> ActionOptions 
        { 
            get { return actionOptions;} 
            set { actionOptions = value; } 
        }
        protected Dictionary<string, string> actionOptions = new Dictionary<string, string>();

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
    }
}

