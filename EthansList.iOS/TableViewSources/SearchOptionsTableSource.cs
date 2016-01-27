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
        SearchPickerModel picker_model;
        UIPickerView picker;

        protected SearchOptionsTableSource() {}

        public SearchOptionsTableSource (List<TableItemGroup> items, UIViewController owner)
        {
            this.tableItems = items;
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
                ((SearchTermsCell)cell).TermsField.EditingDidBegin += (object sender, EventArgs e) => {
                    ((SearchOptionsViewController)this.owner).FieldSelected = ((SearchTermsCell)cell).TermsField.InputView;
                };
            }
            else if (item.CellType == "PriceSelectorCell")
            {
                cell = PriceSelectorCell.Create();
                ((PriceSelectorCell)cell).LabelText = item.Heading;


                picker_model = new SearchPickerModel(item.PickerOptions, true);
                picker = new UIPickerView();
                picker.Model = picker_model;
                picker.ShowSelectionIndicator = true;

                UIToolbar toolbar = new UIToolbar();
                toolbar.BarStyle = UIBarStyle.Default;
                toolbar.Translucent = true;
                toolbar.SizeToFit();

                UIBarButtonItem doneButton = new UIBarButtonItem("Done", UIBarButtonItemStyle.Done, (s, e) =>
                    {
                        this.owner.View.EndEditing(true);
                    });
                toolbar.SetItems(new UIBarButtonItem[]{ doneButton }, true);

                picker_model.PickerChanged += (object sender, PickerChangedEventArgs e) =>
                {
                    var result = e.SelectedValue.ToString();
                    Console.WriteLine(e.SelectedValue + "From" + e.FromComponent);
                    if (e.FromComponent == 0)
                    {
                        ((PriceSelectorCell)cell).MinPrice.Text = result != "Any" ? "$" + result : result;
                        ((SearchOptionsViewController)(this.owner)).MinPrice = result;
                    }
                    else
                    {
                        ((PriceSelectorCell)cell).MaxPrice.Text = result != "Any" ? "$" + result : result;
                        ((SearchOptionsViewController)(this.owner)).MaxPrice = result;
                    }
                };
                        
                ((PriceSelectorCell)cell).PickerField.InputView = picker;
                ((PriceSelectorCell)cell).PickerField.InputAccessoryView = toolbar;

                ((PriceSelectorCell)cell).PickerField.EditingDidBegin += (object sender, EventArgs e) => {
                    ((SearchOptionsViewController)this.owner).KeyboardBounds = picker.Bounds;
                    ((SearchOptionsViewController)this.owner).FieldSelected = ((PriceSelectorCell)cell).PickerField.InputView;
                };

            }
            else if (item.CellType == "PickerSelectorCell")
            {
                cell = PickerSelectorCell.Create();
                ((PickerSelectorCell)cell).Title.Text = item.Heading;


                picker_model = new SearchPickerModel(item.PickerOptions, false);
                picker = new UIPickerView();
                picker.Model = picker_model;
                picker.ShowSelectionIndicator = true;

                UIToolbar toolbar = new UIToolbar();
                toolbar.BarStyle = UIBarStyle.Default;
                toolbar.Translucent = true;
                toolbar.SizeToFit();

                UIBarButtonItem doneButton = new UIBarButtonItem("Done", UIBarButtonItemStyle.Done, (s, e) =>
                    {
                        this.owner.View.EndEditing(true);
                    });
                toolbar.SetItems(new UIBarButtonItem[]{ doneButton }, true);

                picker_model.PickerChanged += (object sender, PickerChangedEventArgs e) =>
                    {
                        string resultKey = e.SelectedKey.ToString();
                        string resultValue = null;

                        if (e.SelectedValue != null)
                            resultValue = e.SelectedValue.ToString();

                        Console.WriteLine(resultKey + " From " + e.FromComponent);
                        ((PickerSelectorCell)cell).Display.Text = resultKey;
                        if (item.Heading == "Min Bedrooms")
                            ((SearchOptionsViewController)(this.owner)).MinBedrooms = resultValue;
                        else if (item.Heading == "Min Bathrooms")
                            ((SearchOptionsViewController)(this.owner)).MinBathrooms = resultValue;
                        else if (item.Heading == "Posted Date")
                            ((SearchOptionsViewController)(this.owner)).WeeksOld = (int?)Convert.ToInt16(resultValue);
                        else if (item.Heading == "Max Listings")
                            ((SearchOptionsViewController)(this.owner)).MaxListings = Convert.ToInt16(resultValue);
                    };

                ((PickerSelectorCell)cell).InputTextField.InputView = picker;
                ((PickerSelectorCell)cell).InputTextField.InputAccessoryView = toolbar;

                ((PickerSelectorCell)cell).InputTextField.EditingDidBegin += (object sender, EventArgs e) => {
                    ((SearchOptionsViewController)this.owner).KeyboardBounds = picker.Bounds;
                    ((SearchOptionsViewController)this.owner).FieldSelected = ((PickerSelectorCell)cell);
                };
            }

            return cell;
        }

        private static Task<String> ShowNumberOptions(UIViewController parent, string strTitle, string strMsg, Dictionary<string, object> options)
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
}

