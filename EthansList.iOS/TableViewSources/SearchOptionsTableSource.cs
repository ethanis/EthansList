using System;
using UIKit;
using System.Collections.Generic;
using Foundation;
using System.Threading.Tasks;
using System.Linq;

namespace ethanslist.ios
{
    public class SearchOptionsTableSource : UITableViewSource
    {
        protected List<TableItemGroup> tableItems;
        protected string cellIdentifier = "TableCell";
        SearchViewController owner;
        public EventHandler<EventArgs> actionSheetSelected;
        private SearchPickerModel picker_model;
        private UIPickerView picker;

        protected SearchOptionsTableSource() {}

        public SearchOptionsTableSource (List<TableItemGroup> items, UIViewController owner)
        {
            this.tableItems = items;
            this.owner = (SearchViewController)owner;
        }

        public override nint NumberOfSections (UITableView tableView)
        {
            return tableItems.Count;
        }

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

        private void AddSearchItem (string itemKey, string itemValue)
        {
            if (this.owner.SearchItems.ContainsKey(itemKey))
                this.owner.SearchItems[itemKey] = itemValue;
            else
                this.owner.SearchItems.Add(itemKey, itemValue);
        }

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
                    var searchTermsCell = (SearchTermsCell)tableView.DequeueReusableCell(SearchTermsCell.Key);
                    if (searchTermsCell == null)
                        searchTermsCell = new SearchTermsCell();

                    searchTermsCell.TermsField.Placeholder = "Search: " + this.owner.Category.Value;
                    searchTermsCell.AccessibilityIdentifier = "SearchTermsField";
                    searchTermsCell.TermsField.EditingChanged += delegate
                    {
                            AddSearchItem("query", searchTermsCell.TermsField.Text);
                    };
//                    searchTermsCell.TermsField.EditingDidBegin += (object sender, EventArgs e) =>
//                    {
//                            this.owner.FieldSelected = searchTermsCell.TermsField.InputView;
//                    };
                    searchTermsCell.TermsField.TouchUpInside += delegate
                    {
                            this.owner.FieldSelected = searchTermsCell.TermsField.InputView;
                    };

                    return searchTermsCell;
                case "PriceInputCell":
                    var priceInputCell = (PriceCell)tableView.DequeueReusableCell(PriceCell.Key);
                    if (priceInputCell == null)
                        priceInputCell = new PriceCell();

                    priceInputCell.HeaderLabel.AttributedText = new NSAttributedString(item.Heading, Constants.LabelAttributes);

                    priceInputCell.NumChanged += (object sender, EventArgs e) =>
                    {
                            AddSearchItem("min_price", priceInputCell.MinPriceField.Text);
                            AddSearchItem("max_price", priceInputCell.MaxPriceField.Text);
                    };

                    priceInputCell.MaxPriceField.EditingDidBegin += (object sender, EventArgs e) =>
                    {
                            this.owner.FieldSelected = priceInputCell.MaxPriceField.InputView;
                    };
                        
                    priceInputCell.MinPriceField.EditingDidBegin += (object sender, EventArgs e) =>
                    {
                            this.owner.FieldSelected = priceInputCell.MinPriceField.InputView;
                    };

                    return priceInputCell;
                case "MinMaxCell":
                    var minMaxCell = (GenericPriceCell)tableView.DequeueReusableCell(GenericPriceCell.Key);
                    if (minMaxCell == null)
                        minMaxCell = new GenericPriceCell();

                    minMaxCell.HeaderLabel.AttributedText = new NSAttributedString(item.Heading, Constants.LabelAttributes);

                    minMaxCell.NumChanged += delegate
                    {
                            switch (item.Heading) 
                            {
                                case "Sq Feet":
                                    AddSearchItem("minSqft", minMaxCell.MinField.Text);
                                    AddSearchItem("maxSqft", minMaxCell.MaxField.Text);

                                    break;
                                case "Year":
                                    AddSearchItem("min_auto_year", minMaxCell.MinField.Text);
                                    AddSearchItem("max_auto_year", minMaxCell.MaxField.Text);

                                    break;
                                case "Odometer":
                                    AddSearchItem("min_auto_miles", minMaxCell.MinField.Text);
                                    AddSearchItem("max_auto_miles", minMaxCell.MaxField.Text);

                                    break;
                                default:
                                    break;
                            }
                    };

                    minMaxCell.MaxField.EditingDidBegin += (object sender, EventArgs e) =>
                        {
                            this.owner.FieldSelected = minMaxCell.MaxField.InputView;
                        };

                    minMaxCell.MinField.EditingDidBegin += (object sender, EventArgs e) =>
                        {
                            this.owner.FieldSelected = minMaxCell.MinField.InputView;
                        };

                    return minMaxCell;
                case "MakeModelCell":
                    var makeModelCell = (TitledSearchCell)tableView.DequeueReusableCell(TitledSearchCell.Key);
                    if (makeModelCell == null)
                        makeModelCell = new TitledSearchCell();

                    makeModelCell.Title.AttributedText = new NSAttributedString(item.Heading, Constants.LabelAttributes);
                    makeModelCell.TermsField.Placeholder = item.SubHeading;

                    makeModelCell.TermsField.EditingChanged += delegate
                    {
                            AddSearchItem("auto_make_model", makeModelCell.TermsField.Text);
                    };
                    makeModelCell.TermsField.EditingDidBegin += delegate
                    {
                            this.owner.FieldSelected = makeModelCell.TermsField.InputView;
                    };
                    
                    return makeModelCell;
                case "PickerSelectorCell":
                    var pickerSelectorCell = (PickerSelectorCell)tableView.DequeueReusableCell(PickerSelectorCell.Key);
                    if (pickerSelectorCell == null)
                        pickerSelectorCell = PickerSelectorCell.Create();

                    pickerSelectorCell.Title.AttributedText = new NSAttributedString(item.Heading, Constants.LabelAttributes);

                    picker_model = new SearchPickerModel(item.PickerOptions, false);
                    picker = new UIPickerView();
                    picker.Model = picker_model;
                    picker.ShowSelectionIndicator = true;

                    if (item.Heading == "Sub Category")
                    {
                        var firstItem = item.PickerOptions[0].PickerWheelOptions[0];
                        this.owner.SubCategory = firstItem;
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
                                AddSearchItem("bedrooms", resultValue);
                            else if (item.Heading == "Min Bathrooms")
                                AddSearchItem("bathrooms", resultValue);
                            else if (item.Heading == "Posted Date")
                                this.owner.WeeksOld = (int?)Convert.ToInt16(resultValue);
                            else if (item.Heading == "Max Listings")
                                this.owner.MaxListings = Convert.ToInt16(resultValue);
                            else if (item.Heading == "Sub Category")
                                this.owner.SubCategory = new KeyValuePair<object, object>(resultKey, resultValue);
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
                case "ComboTableCell":
                    var tableSelectorCell = (PickerSelectorCell)tableView.DequeueReusableCell(PickerSelectorCell.Key);
                    if (tableSelectorCell == null)
                        tableSelectorCell = PickerSelectorCell.Create();
                    
                    tableSelectorCell.Title.AttributedText = new NSAttributedString(item.Heading, Constants.LabelAttributes);

                    ComboPickerTableSource comboSource = new ComboPickerTableSource(item.PickerOptions);
                    UITableView comboPicker = new UITableView();
                    comboPicker.Source = comboSource;
                    comboPicker.Bounds = new CoreGraphics.CGRect(0,0,this.owner.View.Bounds.Width, 0.4f * this.owner.View.Bounds.Height);

                    comboSource.ValueChanged += (object sender, PickerChangedEventArgs e) => {
                        string resultKey = e.SelectedKey.ToString();
                        string resultValue = null;
                        if (e.SelectedValue != null)
                            resultValue = e.SelectedValue.ToString();

                        if (this.owner.Conditions.ContainsKey(resultKey))
                        {
                            this.owner.Conditions.Remove(resultKey);
                            Console.WriteLine ("Removed Key: " + resultKey + ", Value: " + resultValue);
                        }
                        else
                        {
                            this.owner.Conditions.Add(resultKey, new KeyValuePair<object, object>(item.SubHeading, resultValue));
                            Console.WriteLine ("Added Key: " + resultKey + ", Value: " + resultValue);
                        }

                        var keys = (from kvp in this.owner.Conditions where (string)kvp.Value.Key == item.SubHeading select (string)kvp.Key).ToList();
                        var text = keys.Count > 0 ? String.Join(", ", keys.ToArray()) : "Any";
                        tableSelectorCell.Display.AttributedText = new NSAttributedString(text, Constants.LabelAttributes);
                    };

                    tableSelectorCell.InputTextField.InputView = comboPicker;
                    tableSelectorCell.InputTextField.InputAccessoryView = toolbar;

                    tableSelectorCell.InputTextField.EditingDidBegin += (object sender, EventArgs e) => {
                        this.owner.KeyboardBounds = comboPicker.Bounds;
                        this.owner.FieldSelected = tableSelectorCell;
                        tableSelectorCell.Accessory = UITableViewCellAccessory.Checkmark;
                    };

                    tableSelectorCell.InputTextField.EditingDidEnd += delegate
                        {
                            tableSelectorCell.Accessory = UITableViewCellAccessory.None;
                        };
                    
                    return tableSelectorCell;
            }
        }
    }
}

