using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using EthansList.Shared;

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

            if (post.ImageLink != "-1")
            {
                postingImageView.Image = FromUrl(post.ImageLink);
            }
            BackButton.Clicked += OnDismiss;
        }


        void OnDismiss(object sender, EventArgs e)
        {
            DismissViewController(true, null);
        }


        static UIImage FromUrl (string uri)
        {
            using (var url = new NSUrl (uri))
            using (var data = NSData.FromUrl (url))
                return UIImage.LoadFromData (data);
        }
	}
}
