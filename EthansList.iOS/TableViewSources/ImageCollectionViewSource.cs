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
        PostingInfoTableSource tableSource;

        static string CellID = "listingCell";

        public ImageCollectionViewSource(PostingInfoTableSource tableSource, List<string> urls)
        {
            this.urls = urls;
            this.tableSource = tableSource;
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
            tableSource.CurrentImageIndex = indexPath.Row;
            tableSource.Image = urls[indexPath.Row];
        }
    }
}

