using System;
using UIKit;
using System.Collections.Generic;
using SDWebImage;
using Foundation;

namespace ethanslist.ios
{
    public class ImageCollectionViewSource : UICollectionViewSource
    {
        List<string> urls;
        PostingDetailsViewController owner;
        static string CellID = "listingCell";

        public ImageCollectionViewSource(UIViewController owner, List<string> urls)
        {
            this.owner = (PostingDetailsViewController)owner;
            this.urls = urls;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return urls.Count;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, Foundation.NSIndexPath indexPath)
        {
            var cell = (ListingImageCell)collectionView.DequeueReusableCell(CellID, indexPath);
            cell.Image = urls[indexPath.Row];

            return cell;
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            owner.Image = urls[indexPath.Row];
        }
    }
}

