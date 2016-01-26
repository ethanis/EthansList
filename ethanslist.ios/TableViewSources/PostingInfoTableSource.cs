using System;
using UIKit;
using System.Collections.Generic;
using EthansList.Models;
using SDWebImage;
using Foundation;
using EthansList.Shared;
using System.Drawing;
using MapKit;

namespace ethanslist.ios
{
    public class PostingInfoTableSource : UITableViewSource
    {
        UIViewController owner;
        List<TableItem> tableItems;
        protected string cellIdentifier = "infoCell";
        readonly Posting post;
        bool result;
        ListingImageDownloader imageHelper;
        private ImageCollectionViewSource collectionSource {get;set;}
        protected SmallLoadingOverlay _loadingOverlay = null;

        public nfloat TitleHeight { get; set; }
        public nfloat DescriptionHeight { get; set; }
        public int CurrentImageIndex { get; set; }

        public event EventHandler<DescriptionLoadedEventArgs> DescriptionLoaded;

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

        private string DescriptionText 
        { 
            get { return descriptionText; } 
            set { descriptionText = value; } 
        }
        private string descriptionText;


        public PostingInfoTableSource(UIViewController owner, List<TableItem> tableItems, Posting post, ListingImageDownloader imageHelper)
        {
            this.owner = owner;
            this.tableItems = tableItems;
            this.post = post;
            this.descriptionText = post.Description;
            CurrentImageIndex = 0;

            this.imageHelper = imageHelper;
            result = imageHelper.GetAllImagesAsync();
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var cell = new UITableViewCell();
            var item = tableItems[(int)indexPath.Row];

            if (item.CellType == "PostingTitleCell")
            {
                cell = PostingTitleCell.Create();

                UIStringAttributes txtAttributes = new UIStringAttributes();
                txtAttributes.Font = UIFont.FromName("San Francisco", 18f);

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

                UITapGestureRecognizer singletap = new UITapGestureRecognizer(OnSingleTap)
                {
                    NumberOfTapsRequired = 1
                };

                UISwipeGestureRecognizer swipeRight = new UISwipeGestureRecognizer(OnSwipeRight)
                { 
                    Direction = UISwipeGestureRecognizerDirection.Right
                };
                UISwipeGestureRecognizer swipeLeft = new UISwipeGestureRecognizer(OnSwipeLeft)
                { 
                    Direction = UISwipeGestureRecognizerDirection.Left
                };

                PostingImageView.AddGestureRecognizer(singletap);
                PostingImageView.AddGestureRecognizer(swipeLeft);
                PostingImageView.AddGestureRecognizer(swipeRight);
            }
            else if (item.CellType == "ImageCollection")
            {
                cell = PostingImageCollectionCell.Create();
                if (post.ImageLink != "-1")
                {
                    if (!imageHelper.LoadingComplete)
                    {
                        var bounds = ((PostingImageCollectionCell)cell).Collection.Bounds;
                        _loadingOverlay = new SmallLoadingOverlay(bounds);
                        ((PostingImageCollectionCell)cell).Collection.Add(_loadingOverlay);
                    }
                    else
                    {
                        ((PostingImageCollectionCell)cell).Collection.RegisterClassForCell(typeof(ListingImageCell), "listingCell");
                        if (collectionSource == null)
                            collectionSource = new ImageCollectionViewSource(this, imageHelper.images);
                        ((PostingImageCollectionCell)cell).Collection.Source = collectionSource;
                    }
                    //Result contains whether or not there is internet connection available
                    if (!result)
                    {
                        Console.WriteLine("Not connected to internet");
                    }
//                    imageHelper.loadingComplete += ImageHelper_LoadingComplete;

                    imageHelper.postingRemoved += (object s, EventArgs ev) =>
                    {
                        if (_loadingOverlay != null)
                            _loadingOverlay.Hide();

                        UIAlertView alert = new UIAlertView();
                        alert.Message = String.Format("This Posting was removed.{0}No additional data available", Environment.NewLine);
                        alert.AddButton("OK");
                        this.owner.InvokeOnMainThread(() => alert.Show());
                    };

                    imageHelper.loadingComplete += (object sender, EventArgs e) =>
                    {
                        if (_loadingOverlay != null)
                            _loadingOverlay.Hide();

                        ((PostingImageCollectionCell)cell).Collection.RegisterClassForCell(typeof(ListingImageCell), "listingCell");
                        if (collectionSource == null)
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

                ((PostingDescriptionCell)cell).PostingDescription.AttributedText = new NSAttributedString(DescriptionText, txtAttributes);
                ((PostingDescriptionCell)cell).PostingDescription.TextAlignment = UITextAlignment.Left;

                CoreGraphics.CGRect bounds = ((PostingDescriptionCell)cell).PostingDescription.AttributedText.GetBoundingRect(
                                                 new SizeF((float)this.owner.View.Bounds.Width, float.MaxValue),
                                                 NSStringDrawingOptions.UsesLineFragmentOrigin | NSStringDrawingOptions.UsesFontLeading, null);

                imageHelper.loadingComplete += (object sender, EventArgs e) =>
                {
                    DescriptionText = imageHelper.postingDescription;

                    if (this.DescriptionLoaded != null)
                        this.DescriptionLoaded(this, new DescriptionLoadedEventArgs() { DescriptionRow = indexPath });
                };

                DescriptionHeight = bounds.Height;
            }
            else if (item.CellType == "PostingMap")
            {
                cell = PostingMapCell.Create();
                imageHelper.loadingComplete += (object sender, EventArgs e) =>
                {
                        ((PostingMapCell)cell).PostingMap.AddAnnotation(new MKPointAnnotation() 
                            {
                                Title = "Location", 
                                Coordinate = imageHelper.postingCoordinates,
                            });

                        MKCoordinateSpan span = new MKCoordinateSpan(MilesToLatitudeDegrees(20), MilesToLongitudeDegrees(20, imageHelper.postingCoordinates.Latitude));
                        ((PostingMapCell)cell).PostingMap.Region = new MKCoordinateRegion(imageHelper.postingCoordinates, span);
                };
            }
            else if (item.CellType == "PostingDate")
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, null);

                cell.TextLabel.Text = "Listed: " + post.Date.ToShortDateString() + " at " + post.Date.ToShortTimeString();
            }
            else if (item.CellType == "PostingLink")
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, null);

                cell.TextLabel.Text = "Original Posting";

                cell.TextLabel.TextColor = UIColor.Blue;
                cell.TextLabel.BackgroundColor = UIColor.Clear;
                cell.TextLabel.UserInteractionEnabled = true;

                UITapGestureRecognizer openLink = new UITapGestureRecognizer(()=> {
                    var storyboard = UIStoryboard.FromName("Main", null);
                    PostingWebViewController postingWebView = (PostingWebViewController)storyboard.InstantiateViewController("PostingWebViewController");
                    postingWebView.PostingLink = post.Link;

                    this.owner.ShowViewController(postingWebView, this);
                }) { NumberOfTapsRequired = 1 };

                cell.TextLabel.AddGestureRecognizer(openLink);
            }


            cell.BackgroundColor = ColorScheme.Clouds;
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;

            return cell;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            var item = tableItems[(int)indexPath.Row];
            nfloat height = new nfloat();

            if (item.CellType == "PostingTitleCell")
            {
                height = TitleHeight + 10f;
            }
            else if (item.CellType == "PostingImage")
            {
                height = this.owner.View.Bounds.Height * 0.4f;
            }
            else if (item.CellType == "ImageCollection")
            {
                height = 54f;
            }
            else if (item.CellType == "PostingDescription")
            {
                height = DescriptionHeight + 10f;
            }
            else if (item.CellType == "PostingMap")
            {
                height = this.owner.View.Bounds.Width;
            }
            else if (item.CellType == "PostingDate")
            {
                height = 40f;
            }
            else if (item.CellType == "PostingLink")
            {
                height = 40f;
            }

            return height;
        }

        public override nint RowsInSection (UITableView tableview, nint section)
        {
            return tableItems.Count;
        }

        private void OnSingleTap (UIGestureRecognizer gesture) 
        {
            if (imageHelper.images.Count > 0)
            {
                var storyboard = UIStoryboard.FromName("Main", null);
                postingImageViewController postingImageVC = (postingImageViewController)storyboard.InstantiateViewController("postingImageViewController");
                postingImageVC.ImageLinks = imageHelper.images;
                postingImageVC.ImageIndex = CurrentImageIndex;

                this.owner.ShowViewController(postingImageVC, this);
            }
        }

        private void OnSwipeRight (UIGestureRecognizer gesture)
        {
            if (CurrentImageIndex > 0)
            {
                CurrentImageIndex -= 1;
                Image = imageHelper.images[CurrentImageIndex];
            }
        }

        private void OnSwipeLeft (UIGestureRecognizer gesture)
        {
            if (CurrentImageIndex < imageHelper.images.Count - 1)
            {
                CurrentImageIndex += 1;
                Image = imageHelper.images[CurrentImageIndex];
            }
        }

        public double MilesToLatitudeDegrees(double miles)
        {
            double earthRadius = 3960.0; // in miles
            double radiansToDegrees = 180.0/Math.PI;
            return (miles/earthRadius) * radiansToDegrees;
        }

        public double MilesToLongitudeDegrees(double miles, double atLatitude)
        {
            double earthRadius = 3960.0; // in miles
            double degreesToRadians = Math.PI/180.0;
            double radiansToDegrees = 180.0/Math.PI;
            // derive the earth's radius at that point in latitude
            double radiusAtLatitude = earthRadius * Math.Cos(atLatitude * degreesToRadians);
            return (miles / radiusAtLatitude) * radiansToDegrees;
        }

//        void ImageHelper_LoadingComplete (object sender, EventArgs e)
//        {
//                                    if (_loadingOverlay != null)
//                                        _loadingOverlay.Hide();
//            
//                                    ((PostingImageCollectionCell)cell).Collection.RegisterClassForCell(typeof(ListingImageCell), "listingCell");
//                                    collectionSource = new ImageCollectionViewSource(this, imageHelper.images);
//                                    ((PostingImageCollectionCell)cell).Collection.Source = collectionSource;
//        }
    }

    public class DescriptionLoadedEventArgs : EventArgs
    {
        public NSIndexPath DescriptionRow { get; set;}
    }
}

