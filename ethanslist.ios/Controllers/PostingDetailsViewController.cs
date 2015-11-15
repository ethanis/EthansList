using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using EthansList.Shared;
using SDWebImage;

namespace ethanslist.ios
{
	partial class PostingDetailsViewController : UIViewController
	{
		public PostingDetailsViewController (IntPtr handle) : base (handle)
		{
		}

        Posting post;
        public Posting Post {
            get {
                return post;
            }
            set {
                post = value;
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            PostingTitle.Text = post.Title;
            PostingDescription.Text = post.Description;
            Console.WriteLine(post.ImageLink);
            Console.WriteLine(post.Date);

            dateLabel.Text = "Listed: " + post.Date.ToShortDateString() + " at " + post.Date.ToShortTimeString();

            if (post.ImageLink != "-1")
            {
                postingImageView.SetImage(
                    url: new NSUrl(post.ImageLink),
                    placeholder: UIImage.FromBundle("placeholder.png")
                );
            }
            DoneButton.Clicked += OnDismiss;

            SaveButton.Clicked += SaveListing;
        }


        void OnDismiss(object sender, EventArgs e)
        {
            DismissViewController(true, null);
        }

        void SaveListing(object sender, EventArgs e)
        {
//            listingRepository.
        }


        static UIImage FromUrl (string uri)
        {
            using (var url = new NSUrl (uri))
            using (var data = NSData.FromUrl (url))
                return UIImage.LoadFromData (data);
        }
	}
}
