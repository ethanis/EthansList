using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Collections.Generic;

namespace ethanslist.ios
{
	partial class SearchOptionsViewController : UIViewController
	{
        List<TableItemGroup> tableItems;

		public SearchOptionsViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            tableItems = new List<TableItemGroup>();


            TableItemGroup searchterms = new TableItemGroup()
                { Name = "Search Terms"};
            searchterms.Items.Add(new TableItem() { 
                Heading = "Search Terms",
                CellType = "SearchTermsCell",
            });

            TableItemGroup prices = new TableItemGroup()
            { 
                Name = "Prices",
            };
            prices.Items.Add(new TableItem() {
                Heading = "Max. Price",
                CellType = "PriceSelectorCell"
            });

            TableItemGroup options = new TableItemGroup()
                { 
                    Name = "Options",
                };
            options.Items.Add(new TableItem() {
                Heading = "Min Bedrooms",
                CellType = "BedBathCell"
            });
            options.Items.Add(new TableItem() {
                Heading = "Min Bathrooms",
                CellType = "BedBathCell"
            });

            tableItems.Add(searchterms);
            tableItems.Add(prices);
            tableItems.Add(options);

            SearchTableView.Source = new TableSource(tableItems);
        }
	}
        
    /// <summary>
    /// Combined DataSource and Delegate for our UITableView
    /// </summary>
    public class TableSource : UITableViewSource
    {
        // declare vars
        protected List<TableItemGroup> tableItems;
        protected string cellIdentifier = "TableCell";

        protected TableSource() {}

        public TableSource (List<TableItemGroup> items)
        {
            tableItems = items;
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
            new UIAlertView("Row Selected"
                , tableItems[indexPath.Section].Items[indexPath.Row].Heading, null, "OK", null).Show();
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
            }
            else if (item.CellType == "PriceSelectorCell")
            {
                cell = PriceSelectorCell.Create();
                ((PriceSelectorCell)cell).LabelText = item.Heading;
            }
            else if (item.CellType == "BedBathCell")
            {
                cell = BedBathCell.Create();
                ((BedBathCell)cell).Title = item.Heading;
            }
            else
            {
                cell = tableView.DequeueReusableCell(cellIdentifier);

                // if there are no cells to reuse, create a new one
                if (cell == null)
                    cell = new UITableViewCell(item.CellStyle, cellIdentifier);

                // set the item text
                cell.TextLabel.Text = tableItems[indexPath.Section].Items[indexPath.Row].Heading;

                // if it's a cell style that supports a subheading, set it
                if (item.CellStyle == UITableViewCellStyle.Subtitle
                || item.CellStyle == UITableViewCellStyle.Value1
                || item.CellStyle == UITableViewCellStyle.Value2)
                {
                    cell.DetailTextLabel.Text = item.SubHeading;
                }
                
                // set the accessory
                cell.Accessory = item.CellAccessory;
            }
            return cell;
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
