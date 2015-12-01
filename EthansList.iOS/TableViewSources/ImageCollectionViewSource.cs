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

        public ImageCollectionViewSource(List<string> urls)
        {
            this.urls = urls;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return urls.Count;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, Foundation.NSIndexPath indexPath)
        {
            var cell = (ListingImageCell)collectionView.DequeueReusableCell("listingCell", indexPath);
            var image = urls[indexPath.Row];

            cell.imageView.SetImage(
                url: new NSUrl(image),
                placeholder: UIImage.FromBundle("placeholder.png")
            );

            return cell;
        }
    }
}

