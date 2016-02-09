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
        SearchOptionsViewController owner;
        public EventHandler<EventArgs> actionSheetSelected;
        private SearchPickerModel picker_model;
        private UIPickerView picker;

        private SearchTermsCell searchTermsCell { get; set; }
        private PriceInputCell priceInputCell { get; set; }
        private PriceSelectorCell priceCell { get; set; }
        private PriceInputCell footageCell { get; set; }

        protected SearchOptionsTableSource() {}

        public SearchOptionsTableSource (List<TableItemGroup> items, UIViewController owner)
        {
            this.tableItems = items;
            this.owner = (SearchOptionsViewController)owner;
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

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            UILabel headerLbl = new UILabel(new CoreGraphics.CGRect(0, 0, tableView.Bounds.Width, Constants.ButtonHeight));
            headerLbl.AttributedText = new NSAttributedString(" " + tableItems[(int)section].Name, Constants.LabelAttributes);

            return headerLbl;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return Constants.ButtonHeight;
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

        public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            TableItem item = tableItems[indexPath.Section].Items[indexPath.Row];

            UIToolbar toolbar = new UIToolbar();
            toolbar.BarStyle = UIBarStyle.Default;
            toolbar.Translucent = true;
            toolbar.SizeToFit();

            UIBarButtonItem doneButton = new UIBarButtonItem("Done", UIBarButtonItemStyle.Done, (s, e) =>
                {
                    this.owner.View.EndEditing(true);
                });
            toolbar.SetItems(new UIBarButtonItem[]{ doneButton }, true);


            switch (item.CellType)
            {
                default:
                    return new UITableViewCell();
                case "SearchTermsCell":
                    if (searchTermsCell == null)
                    {
                        searchTermsCell = SearchTermsCell.Create();

                        searchTermsCell.TermsField.Placeholder = "Search: " + this.owner.Category.Value;
                        searchTermsCell.AccessibilityIdentifier = "SearchTermsField";
                        searchTermsCell.TermsField.EditingChanged += delegate
                        {
                                this.owner.SearchTerms = searchTermsCell.TermsField.Text;
                        };
                        searchTermsCell.TermsField.EditingDidBegin += (object sender, EventArgs e) =>
                        {
                                this.owner.FieldSelected = searchTermsCell.TermsField.InputView;
                        };
                    }

                    return searchTermsCell;
//                case "PriceSelectorCell":
//                    if (priceCell == null)
//                    {
//                        priceCell = PriceSelectorCell.Create();
//                        priceCell.LabelText = item.Heading;
//
//
//                        picker_model = new SearchPickerModel(item.PickerOptions, true);
//                        picker = new UIPickerView();
//                        picker.Model = picker_model;
//                        picker.ShowSelectionIndicator = true;
//
//
//                        picker_model.PickerChanged += (object sender, PickerChangedEventArgs e) =>
//                        {
//                            var result = e.SelectedValue.ToString();
//                            Console.WriteLine(e.SelectedValue + "From" + e.FromComponent);
//                            var text = result != "Any" ? "$" + result : result;
//                            if (e.FromComponent == 0)
//                            {
//                                priceCell.MinPrice.AttributedText = new NSAttributedString(text, Constants.LabelAttributes);
//                                this.owner.MinPrice = result;
//                            }
//                            else
//                            {
//                                priceCell.MaxPrice.AttributedText = new NSAttributedString(text, Constants.LabelAttributes);
//                                this.owner.MaxPrice = result;
//                            }
//
//                            if (priceCell.MinPrice.Text != "Any" || priceCell.MaxPrice.Text != "Any")
//                                priceCell.ToLabel.Hidden = false;
//                            else
//                                priceCell.ToLabel.Hidden = true;
//                        };
//                            
//                        priceCell.PickerField.InputView = picker;
//                        priceCell.PickerField.InputAccessoryView = toolbar;
//
//                        priceCell.PickerField.EditingDidBegin += (object sender, EventArgs e) =>
//                        {
//                            this.owner.KeyboardBounds = picker.Bounds;
//                            this.owner.FieldSelected = priceCell.PickerField.InputView;
//                            priceCell.Accessory = UITableViewCellAccessory.Checkmark;
//                        };
//
//                        priceCell.PickerField.EditingDidEnd += delegate
//                        {
//                            priceCell.Accessory = UITableViewCellAccessory.None;
//                        };
//                    }
//                    return priceCell;
                case "PriceInputCell":
                    if (priceInputCell == null)
                    {
                        priceInputCell = PriceInputCell.Create();
                        priceInputCell.HeaderLabel.AttributedText = new NSAttributedString(item.Heading, Constants.LabelAttributes);

                        priceInputCell.NumChanged += (object sender, EventArgs e) =>
                        {
                            this.owner.MinPrice = priceInputCell.MinPrice.Text;
                            this.owner.MaxPrice = priceInputCell.MaxPrice.Text;
                        };

                        priceInputCell.MaxPrice.EditingDidBegin += (object sender, EventArgs e) =>
                        {
                            this.owner.FieldSelected = priceInputCell.MaxPrice.InputView;
                        };
                            
                        priceInputCell.MinPrice.EditingDidBegin += (object sender, EventArgs e) =>
                        {
                            this.owner.FieldSelected = priceInputCell.MinPrice.InputView;
                        };
                    }
                    return priceInputCell;
                case "SqFootageCell":
                    if (footageCell == null)
                    {
                        footageCell = PriceInputCell.Create();
                        footageCell.HeaderLabel.AttributedText = new NSAttributedString(item.Heading, Constants.LabelAttributes);

                        footageCell.NumChanged += (object sender, EventArgs e) =>
                        {
                                this.owner.MinFootage = footageCell.MinPrice.Text;
                                this.owner.MaxFootage = footageCell.MaxPrice.Text;
                        };

                        footageCell.MaxPrice.EditingDidBegin += (object sender, EventArgs e) =>
                        {
                                this.owner.FieldSelected = footageCell.MaxPrice.InputView;
                        };

                        footageCell.MinPrice.EditingDidBegin += (object sender, EventArgs e) =>
                        {
                                this.owner.FieldSelected = footageCell.MinPrice.InputView;
                        };
                    }
                    return footageCell;
                case "PickerSelectorCell":
                    var pickerSelectorCell = PickerSelectorCell.Create();

                    pickerSelectorCell.Title.AttributedText = new NSAttributedString(item.Heading, Constants.LabelAttributes);

                    picker_model = new SearchPickerModel(item.PickerOptions, false);
                    picker = new UIPickerView();
                    picker.Model = picker_model;
                    picker.ShowSelectionIndicator = true;

                    if (item.Heading == "Sub Category")
                    {
                        var firstItem = item.PickerOptions[0].PickerWheelOptions[0];
                        this.owner.SubCategory = (string)firstItem.Value;
                        pickerSelectorCell.Display.AttributedText = new NSAttributedString((string)firstItem.Key, Constants.LabelAttributes);
                    }

                    picker_model.PickerChanged += (object sender, PickerChangedEventArgs e) =>
                        {
                            string resultKey = e.SelectedKey.ToString();
                            string resultValue = null;

                            if (e.SelectedValue != null)
                                resultValue = e.SelectedValue.ToString();

                            Console.WriteLine(resultKey + " From " + e.FromComponent);
                            pickerSelectorCell.Display.AttributedText = new NSAttributedString(resultKey, Constants.LabelAttributes);
                            if (item.Heading == "Min Bedrooms")
                                this.owner.MinBedrooms = resultValue;
                            else if (item.Heading == "Min Bathrooms")
                                this.owner.MinBathrooms = resultValue;
                            else if (item.Heading == "Posted Date")
                                this.owner.WeeksOld = (int?)Convert.ToInt16(resultValue);
                            else if (item.Heading == "Max Listings")
                                this.owner.MaxListings = Convert.ToInt16(resultValue);
                            else if (item.Heading == "Sub Category")
                                this.owner.SubCategory = resultValue;
                        };
                    
                    pickerSelectorCell.InputTextField.InputView = picker;
                    pickerSelectorCell.InputTextField.InputAccessoryView = toolbar;

                    pickerSelectorCell.InputTextField.EditingDidBegin += (object sender, EventArgs e) => {
                        this.owner.KeyboardBounds = picker.Bounds;
                        this.owner.FieldSelected = pickerSelectorCell;
                        pickerSelectorCell.Accessory = UITableViewCellAccessory.Checkmark;
                    };

                    pickerSelectorCell.InputTextField.EditingDidEnd += delegate
                    {
                            pickerSelectorCell.Accessory = UITableViewCellAccessory.None;
                    };
                    return pickerSelectorCell;
            }
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

