using System;
using UIKit;
using System.Collections.Generic;
using EthansList.Models;
using SDWebImage;
using Foundation;
using EthansList.Shared;
using System.Drawing;
using CoreText;

namespace ethanslist.ios
{
    public class PostingInfoTableSource : UITableViewSource
    {
        UIViewController owner;
        List<TableItem> tableItems;
        protected string cellIdentifier = "infoCell";
        readonly Posting post;
        ListingImageDownloader imageHelper;
        ImageCollectionViewSource collectionSource;
        protected SmallLoadingOverlay _loadingOverlay = null;

        public nfloat TitleHeight { get; set; }
        public nfloat DescriptionHeight { get; set; }
        public int CurrentImageIndex { get; set; }

        string image;
        public string Image
        {
            get { return image; }
            set 
            {
                PostingImageView.SetImage(
                    new NSUrl(value),
                    UIImage.FromBundle("placeholder.png"),
                    SDWebImageOptions.HighPriority,
                    null,
                    (image,error,cachetype,NSNull) => 
                    {
                        PostingImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
                    }
                );
                image = value;
            }
        }

        private UIImageView PostingImageView { get; set; }

        public PostingInfoTableSource(UIViewController owner, List<TableItem> tableItems, Posting post)
        {
            this.owner = owner;
            this.tableItems = tableItems;
            this.post = post;
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var cell = new UITableViewCell();
            var item = tableItems[(int)indexPath.Row];

            if (item.CellType == "PostingTitleCell")
            {
                cell = PostingTitleCell.Create();

                UIStringAttributes txtAttributes = new UIStringAttributes();
                txtAttributes.Font = UIFont.FromName("HelveticaNeue-Light", 18f);

                ((PostingTitleCell)cell).PostingTitle.AttributedText = new NSAttributedString(post.PostTitle, txtAttributes);
                ((PostingTitleCell)cell).PostingTitle.TextAlignment = UITextAlignment.Justified;

                CoreGraphics.CGRect bounds = ((PostingTitleCell)cell).PostingTitle.AttributedText.GetBoundingRect(
                    new SizeF((float)this.owner.View.Bounds.Width, float.MaxValue),
                    NSStringDrawingOptions.UsesLineFragmentOrigin | NSStringDrawingOptions.UsesFontLeading, null);

                TitleHeight = bounds.Height;
            }
            else if (item.CellType == "PostingImage")
            {
                cell = PostingImageCell.Create();
                this.PostingImageView = ((PostingImageCell)cell).MainImage;

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
                    var bounds = ((PostingImageCollectionCell)cell).Collection.Bounds;
                    _loadingOverlay = new SmallLoadingOverlay(bounds);
                    ((PostingImageCollectionCell)cell).Collection.Add(_loadingOverlay);

                    imageHelper = new ListingImageDownloader(post.Link, post.ImageLink);
                    var result = imageHelper.GetAllImagesAsync();
                    //Result contains whether or not there is internet connection available
                    if (!result)
                    {
                        Console.WriteLine("Not connected to internet");
                    }
                    imageHelper.loadingComplete += (object sender, EventArgs e) =>
                    {
                        //TODO: Update row height based on full description
                        //PostingDescription.Text = imageHelper.postingDescription;
                        if (_loadingOverlay != null)
                            _loadingOverlay.Hide();
                            
                        ((PostingImageCollectionCell)cell).Collection.RegisterClassForCell(typeof(ListingImageCell), "listingCell");
                            collectionSource = new ImageCollectionViewSource(this, imageHelper.images);
                        ((PostingImageCollectionCell)cell).Collection.Source = collectionSource;
                    };
                }
            }
            else if (item.CellType == "PostingDescription")
            {
                cell = PostingDescriptionCell.Create();

                UIStringAttributes txtAttributes = new UIStringAttributes();
                txtAttributes.Font = UIFont.FromName("HelveticaNeue-Light", 18f);

                ((PostingDescriptionCell)cell).PostingDescription.AttributedText = new NSAttributedString(post.Description, txtAttributes);
                ((PostingDescriptionCell)cell).PostingDescription.TextAlignment = UITextAlignment.Left;

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
                height = TitleHeight + 20f;
            }
            else if (item.CellType == "PostingImage")
            {
                height = this.owner.View.Bounds.Height * 0.4f;
            }
            else if (item.CellType == "ImageCollection")
            {
                height = 80f;
            }
            else if (item.CellType == "PostingDescription")
            {
                height = DescriptionHeight + 10f;
            }
            else if (item.CellType == "PostingDate")
            {
                height = 40f;
            }

            return height;
        }

        public override nint RowsInSection (UITableView tableview, nint section)
        {
            return tableItems.Count;
        }
    }
}

