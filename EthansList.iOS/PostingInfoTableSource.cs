using System;
using UIKit;
using System.Collections.Generic;
using EthansList.Models;
using SDWebImage;
using Foundation;
using EthansList.Shared;
using System.Drawing;

namespace ethanslist.ios
{
    public class PostingInfoTableSource : UITableViewSource
    {
        UIViewController owner;
        List<TableItem> tableItems;
        protected string cellIdentifier = "infoCell";
        Posting post;
        ListingImageDownloader imageHelper;
        ImageCollectionViewSource collectionSource;

        public nfloat DescriptionHeight { get; set; }

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
                        (image, error, cachetype, NSNull) =>
                        {
                            ((PostingImageCell)cell).MainImage.ContentMode = UIViewContentMode.ScaleAspectFit;
                        }
                    );
                }
            }
            else if (item.CellType == "ImageCollection")
            {
                cell = PostingImageCollectionCell.Create();
                if (post.ImageLink != "-1")
                {
                    imageHelper = new ListingImageDownloader(post.Link, post.ImageLink);
                    var result = imageHelper.GetAllImagesAsync();
                    //Result contains whether or not there is internet connection available
                    if (!result)
                    {
                        Console.WriteLine("Not connected to internet");
                    }
                    imageHelper.loadingComplete += (object sender, EventArgs e) =>
                    {
//                            PostingDescription.Text = imageHelper.postingDescription;
                        ((PostingImageCollectionCell)cell).Collection.RegisterClassForCell(typeof(ListingImageCell), "listingCell");
                        collectionSource = new ImageCollectionViewSource(this.owner, imageHelper.images);
                        ((PostingImageCollectionCell)cell).Collection.Source = collectionSource;
                    };
                }
            }
            else if (item.CellType == "PostingDescription")
            {
                cell = PostingDescriptionCell.Create();
                ((PostingDescriptionCell)cell).PostingDescription.Text = post.Description;
                CoreGraphics.CGRect bounds = ((PostingDescriptionCell)cell).PostingDescription.AttributedText.GetBoundingRect(
                    new SizeF((float)this.owner.View.Bounds.Width, float.MaxValue),
                    NSStringDrawingOptions.UsesLineFragmentOrigin | NSStringDrawingOptions.UsesFontLeading, null);

                DescriptionHeight = bounds.Height;
            }
            else if (item.CellType == "PostingDate")
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);

                cell.TextLabel.Text = "Listed: " + post.Date.ToShortDateString() + " at " + post.Date.ToShortTimeString();
            }

            return cell;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            var item = tableItems[(int)indexPath.Row];
            nfloat height = new nfloat();

            if (item.CellType == "PostingTitleCell")
            {
                height = 40f;
            }
            else if (item.CellType == "PostingImage")
            {
                height = 200f;
            }
            else if (item.CellType == "ImageCollection")
            {
                height = 80f;
            }
            else if (item.CellType == "PostingDescription")
            {
                height = DescriptionHeight;
            }
            else if (item.CellType == "PostingDate")
            {
                height = 40f;
            }

            return height;
        }

        #region -= data binding/display methods =-
            
        public override nint RowsInSection (UITableView tableview, nint section)
        {
            return tableItems.Count;
        }

        #endregion
    }
}

