using System;
using UIKit;
using System.Collections.Generic;
using EthansList.Models;
using SDWebImage;
using Foundation;

namespace ethanslist.ios
{
    public class PostingInfoTableSource : UITableViewSource
    {
        UIViewController owner;
        List<TableItem> tableItems;
        protected string cellIdentifier = "infoCell";
        Posting post;

        public PostingInfoTableSource(UIViewController owner, List<TableItem> tableItems, Posting post)
        {
            this.owner = owner;
            this.tableItems = tableItems;
            this.post = post;
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var cell = new UITableViewCell();
            Console.WriteLine(tableItems[(int)indexPath.Row].CellType);
            var item = tableItems[(int)indexPath.Row];

            if (item.CellType == "PostingTitleCell")
            {
                cell = PostingTitleCell.Create();
                ((PostingTitleCell)cell).PostingTitle.Text = post.PostTitle;
            }
            else if (item.CellType == "PostingImage")
            {
                cell = PostingImageCell.Create();
                if (post.ImageLink != "-1")
                {
                    ((PostingImageCell)cell).MainImage.SetImage(
                        new NSUrl(post.ImageLink),
                        UIImage.FromBundle("placeholder.png"),
                        SDWebImageOptions.HighPriority,
                        null,
                        (image,error,cachetype,NSNull) => {
                        ((PostingImageCell)cell).MainImage.ContentMode = UIViewContentMode.ScaleAspectFit;
                    }
                    );
                }
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

