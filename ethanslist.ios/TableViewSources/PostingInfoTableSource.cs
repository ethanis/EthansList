using System;
using UIKit;
using System.Collections.Generic;
using EthansList.Models;
using SDWebImage;
using Foundation;
using EthansList.Shared;
using System.Drawing;
using MapKit;
using CoreGraphics;

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
            var item = tableItems[(int)indexPath.Row];

            switch (item.CellType)
            {
                case "PostingTitleCell":
                    var titleCell = PostingTitleCell.Create();
                    UIStringAttributes txtAttributes = new UIStringAttributes();
                    txtAttributes.Font = UIFont.FromName("San Francisco", 18f);

                    titleCell.PostingTitle.AttributedText = new NSAttributedString(post.PostTitle, txtAttributes);
                    titleCell.PostingTitle.TextAlignment = UITextAlignment.Justified;

                    CoreGraphics.CGRect bounds = titleCell.PostingTitle.AttributedText.GetBoundingRect(
                                                     new SizeF((float)this.owner.View.Bounds.Width, float.MaxValue),
                                                     NSStringDrawingOptions.UsesLineFragmentOrigin | NSStringDrawingOptions.UsesFontLeading, null);

                    TitleHeight = bounds.Height;

                    titleCell.BackgroundColor = ColorScheme.Clouds;
                    titleCell.SelectionStyle = UITableViewCellSelectionStyle.None;

                    return titleCell;
                case "PostingImage":
                    var imageCell = PostingImageCell.Create();
                    this.PostingImageView = imageCell.MainImage;

                    if (post.ImageLink != "-1")
                    {
                        imageCell.MainImage.SetImage(
                            new NSUrl(post.ImageLink),
                            UIImage.FromBundle("placeholder.png"),
                            SDWebImageOptions.HighPriority,
                            null,
                            (image, error, cachetype, NSNull) =>
                            {
                                imageCell.MainImage.ContentMode = UIViewContentMode.ScaleAspectFit;
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

                    imageCell.BackgroundColor = ColorScheme.Clouds;
                    imageCell.SelectionStyle = UITableViewCellSelectionStyle.None;

                    return imageCell;
                case "ImageCollection":
                    var collectionCell = PostingImageCollectionCell.Create();
                    if (post.ImageLink != "-1")
                    {
                        if (!imageHelper.LoadingComplete)
                        {
                            var collectionBounds = collectionCell.Collection.Bounds;
                            _loadingOverlay = new SmallLoadingOverlay(collectionBounds);
                            collectionCell.Collection.Add(_loadingOverlay);
                        }
                        else
                        {
                            collectionCell.Collection.RegisterClassForCell(typeof(ListingImageCell), "listingCell");
                            if (collectionSource == null)
                                collectionSource = new ImageCollectionViewSource(this, imageHelper.images);
                            collectionCell.Collection.Source = collectionSource;
                        }
                        //Result contains whether or not there is internet connection available
                        if (!result)
                        {
                            Console.WriteLine("Not connected to internet");
                        }

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

                            collectionCell.Collection.RegisterClassForCell(typeof(ListingImageCell), "listingCell");
                            if (collectionSource == null)
                                collectionSource = new ImageCollectionViewSource(this, imageHelper.images);
                            collectionCell.Collection.Source = collectionSource;
                        };
                    }

                    collectionCell.BackgroundColor = ColorScheme.Clouds;
                    collectionCell.SelectionStyle = UITableViewCellSelectionStyle.None;

                    return collectionCell;
                case "PostingDescription":
                    var descriptioncell = PostingDescriptionCell.Create();

                    UIStringAttributes desctxtAttributes = new UIStringAttributes();
                    desctxtAttributes.Font = UIFont.FromName("HelveticaNeue-Light", 18f);

                    descriptioncell.PostingDescription.AttributedText = new NSAttributedString(DescriptionText, desctxtAttributes);
                    descriptioncell.PostingDescription.TextAlignment = UITextAlignment.Left;

                    CoreGraphics.CGRect descbounds = descriptioncell.PostingDescription.AttributedText.GetBoundingRect(
                                                         new SizeF((float)this.owner.View.Bounds.Width, float.MaxValue),
                                                         NSStringDrawingOptions.UsesLineFragmentOrigin | NSStringDrawingOptions.UsesFontLeading, null);

                    imageHelper.loadingComplete += (object sender, EventArgs e) =>
                    {
                        if (imageHelper.PostingBodyAdded)
                        {
                            DescriptionText = imageHelper.postingDescription;

                            CoreGraphics.CGRect newBounds = descriptioncell.PostingDescription.AttributedText.GetBoundingRect(
                                                                    new SizeF((float)this.owner.View.Bounds.Width, float.MaxValue),
                                                                    NSStringDrawingOptions.UsesLineFragmentOrigin | NSStringDrawingOptions.UsesFontLeading, null);

                            DescriptionHeight = newBounds.Height;

                            if (this.DescriptionLoaded != null)
                                this.DescriptionLoaded(this, new DescriptionLoadedEventArgs() { DescriptionRow = indexPath });
                        }
                    };

                    DescriptionHeight = descbounds.Height;

                    descriptioncell.BackgroundColor = ColorScheme.Clouds;
                    descriptioncell.SelectionStyle = UITableViewCellSelectionStyle.None;

                    return descriptioncell;
                case "PostingMap":
                    var mapCell = PostingMapCell.Create();

                    var coords = imageHelper.postingCoordinates;
                    mapCell.PostingMap.AddAnnotation(new MKPointAnnotation()
                        {
                            Title = "Location", 
                            Coordinate = coords,
                        });

                    MKCoordinateSpan span = new MKCoordinateSpan(MilesToLatitudeDegrees(10.5), MilesToLongitudeDegrees(10.5, coords.Latitude));
                    mapCell.PostingMap.Region = new MKCoordinateRegion(coords, span);

                    mapCell.ZoomStepper.ValueChanged += (s, ev) =>
                    {
                        var value = mapCell.ZoomStepper.Value;
                        value = (value * 50) * (1 / value) - value + 0.5;
                        MKCoordinateSpan newSpan = new MKCoordinateSpan(MilesToLatitudeDegrees(value), MilesToLongitudeDegrees(value, coords.Latitude));
                        mapCell.PostingMap.Region = new MKCoordinateRegion(coords, newSpan);
                    };

                    mapCell.BackgroundColor = ColorScheme.Clouds;
                    mapCell.SelectionStyle = UITableViewCellSelectionStyle.None;

                    return mapCell;
                case "PostingDate":
                    var dateCell = new UITableViewCell(UITableViewCellStyle.Default, null);
                    dateCell.TextLabel.Text = "Listed: " + post.Date.ToShortDateString() + " at " + post.Date.ToShortTimeString();

                    dateCell.BackgroundColor = ColorScheme.Clouds;
                    dateCell.SelectionStyle = UITableViewCellSelectionStyle.None;

                    return dateCell;
                case "PostingLink":
                    var linkcell = new UITableViewCell(UITableViewCellStyle.Default, null);

                    linkcell.TextLabel.Text = "Original Posting";

                    linkcell.TextLabel.TextColor = UIColor.Blue;
                    linkcell.TextLabel.BackgroundColor = UIColor.Clear;
                    linkcell.TextLabel.UserInteractionEnabled = true;

                    UITapGestureRecognizer openLink = new UITapGestureRecognizer(() =>
                        {
                            var storyboard = UIStoryboard.FromName("Main", null);
                            PostingWebViewController postingWebView = (PostingWebViewController)storyboard.InstantiateViewController("PostingWebViewController");
                            postingWebView.PostingLink = post.Link;

                            this.owner.ShowViewController(postingWebView, this);
                        }) { NumberOfTapsRequired = 1 };

                    linkcell.TextLabel.AddGestureRecognizer(openLink);

                    linkcell.BackgroundColor = ColorScheme.Clouds;
                    linkcell.SelectionStyle = UITableViewCellSelectionStyle.None;

                    return linkcell;
                default:
                    return new UITableViewCell();
            }
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            var item = tableItems[(int)indexPath.Row];
            switch (item.CellType)
            {
                case "PostingTitleCell":
                    return TitleHeight + 10f;
                case "PostingImage":
                    return  this.owner.View.Bounds.Height * 0.4f;
                case "ImageCollection":
                    return 54f;
                case "PostingDescription":
                    return DescriptionHeight + 10f;
                case "PostingMap":
                    return this.owner.View.Bounds.Width;
                case "PostingDate":
                    return 40f;
                case "PostingLink":
                    return 40f;
                default:
                    return 40f;
            }
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
    }

    public class DescriptionLoadedEventArgs : EventArgs
    {
        public NSIndexPath DescriptionRow { get; set;}
    }
}

