using System;
using UIKit;
using System.Collections.Generic;

namespace ethanslist.ios
{
    public class PostingInfoTableSource : UITableViewSource
    {
        UIViewController owner;
        List<TableItem> tableItems;
        protected string cellIdentifier = "infoCell";

        public PostingInfoTableSource(UIViewController owner, List<TableItem> tableItems)
        {
            this.owner = owner;
            this.tableItems = tableItems;
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var cell = new UITableViewCell();
            Console.WriteLine(tableItems[(int)indexPath.Row].CellType);
            if (tableItems[(int)indexPath.Row].CellType == "PostingTitleCell")
            {
                cell = PostingTitleCell.Create();
                ((PostingTitleCell)cell).PostingTitle.Text = tableItems[(int)indexPath.Row].SubHeading;
            }
            else
            {
                cell = tableView.DequeueReusableCell(cellIdentifier);
                if (cell == null)
                    cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);

                cell.TextLabel.Text = tableItems[(int)indexPath.Row].Heading;
            }

            return cell;
        }


        #region -= data binding/display methods =-
            
        public override nint RowsInSection (UITableView tableview, nint section)
        {
            return tableItems.Count;
        }

        #endregion
    }
}

