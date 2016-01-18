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
        PickerModel picker_model;
        UIPickerView picker;

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


                picker_model = new PickerModel (item.PickerOptions);
                picker =  new UIPickerView ();
                picker.Model = picker_model;
                picker.ShowSelectionIndicator = true;

                UIToolbar toolbar = new UIToolbar ();
                toolbar.BarStyle = UIBarStyle.Default;
                toolbar.Translucent = true;
                toolbar.SizeToFit ();

                UIBarButtonItem doneButton = new UIBarButtonItem("Done",UIBarButtonItemStyle.Done,(s,e) =>
                    {
//                        foreach (UIView view in this.picker.Subviews) 
//                        {
//                            if (view.IsFirstResponder)
//                            {
//                                UITextField textview = (UITextField)view;
//                                textview.Text = picker_model.values[(int)picker.SelectedRowInComponent((nint)0)].ToString();
//                                textview.ResignFirstResponder ();
//                            }
//                        }

                    });
                toolbar.SetItems (new UIBarButtonItem[]{doneButton},true);



                ((PriceSelectorCell)cell).MinPrice.EditingChanged += delegate
                {
                    ((SearchOptionsViewController)(this.owner)).MinPrice = ((PriceSelectorCell)cell).MinPrice.Text;
                };
                ((PriceSelectorCell)cell).MaxPrice.EditingChanged += delegate
                {
                    ((SearchOptionsViewController)(this.owner)).MaxPrice = ((PriceSelectorCell)cell).MaxPrice.Text;
                };

                ((PriceSelectorCell)cell).MinPrice.InputView = picker;
                ((PriceSelectorCell)cell).MinPrice.InputAccessoryView = toolbar;
            }
            else if (item.CellType == "ActionSheetCell")
            {
                cell = ActionSheetCell.Create();

                ((ActionSheetCell)cell).Title = item.Heading;

                if (item.SubHeading != null)
                    ((ActionSheetCell)cell).MinimumLabel.Text = item.SubHeading;
                
                UITapGestureRecognizer tap = new UITapGestureRecognizer(async () =>
                    {
                        var result = await ShowNumberOptions(this.owner, item.Heading, "Select an option below", item.ActionOptions);
                        Console.WriteLine(result);
                        ((ActionSheetCell)cell).MinimumLabel.Text = result;
                        if (item.Heading == "Min Bedrooms")
                            ((SearchOptionsViewController)(this.owner)).MinBedrooms = (string)item.ActionOptions[result];
                        else if (item.Heading == "Min Bathrooms")
                            ((SearchOptionsViewController)(this.owner)).MinBathrooms = (string)item.ActionOptions[result];
                        else if (item.Heading == "Posted Date")
                            ((SearchOptionsViewController)(this.owner)).WeeksOld = Convert.ToInt16(item.ActionOptions[result]);
                        else if (item.Heading == "Max Listings")
                            ((SearchOptionsViewController)(this.owner)).MaxListings = Convert.ToInt32(item.ActionOptions[result]);
                    });

                ((ActionSheetCell)cell).MinimumLabel.AddGestureRecognizer(tap);
            }

            return cell;
        }

        public static Task<String> ShowNumberOptions(UIViewController parent, string strTitle, string strMsg, Dictionary<string, object> options)
        {
            var taskCompletionSource = new System.Threading.Tasks.TaskCompletionSource<string>();

            UIAlertController actionSheetAlert = UIAlertController.Create(strTitle, strMsg, UIAlertControllerStyle.ActionSheet);

            foreach (KeyValuePair<string, object> option in options)
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
//        {
//            get { return pickerOptions; }
//            set { pickerOptions = value; }
//        }
//        protected PickerViewGroup pickerOptions = new PickerViewGroup();
    }

    public class PickerModel : UIPickerViewModel
    {
        public List<PickerOptions> values;

        public event EventHandler<PickerChangedEventArgs> PickerChanged;

        public PickerModel(List<PickerOptions> values)
        {
            this.values = values;
        }

        public override nint GetComponentCount (UIPickerView picker)
        {
            return values.Count;
        }

        public override nint GetRowsInComponent (UIPickerView picker, nint component)
        {
            return values[(int)component].Options.Count;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            return values[(int)component].Options[(int)row].ToString ();
        }

        public override void Selected (UIPickerView picker, nint row, nint component)
        {
            if (this.PickerChanged != null)
            {
                this.PickerChanged(this, new PickerChangedEventArgs{SelectedValue = values[(int)row]});
            }
        }
    }

    public class PickerChangedEventArgs : EventArgs{
        public object SelectedValue {get;set;}
    }

    public class PickerViewGroup
    {
        public List<PickerOptions> Options
        {
            get { return items; }
            set { items = value; }
        }
        protected List<PickerOptions> items = new List<PickerOptions> ();
    }

    public class PickerOptions
    {
        public Dictionary<object, object> Options
        {
            get { return options; }
            set { options = value; }
        }
        protected Dictionary<object, object> options = new Dictionary<object, object>();
    }
}

