using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

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
//                UpdateItem();
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            PostingTitle.Text = post.Title;
            PostingDescription.Text = post.Description;

//            UpdateItem();
        }

//        public void UpdateItem()
//        {
//            if (post.Title != null) {
//                Title = (post != null) ? post.ToString() : "";
//            }
//        }

        partial void OnDismiss(UIButton sender)
        {
            DismissViewController(true, null);
        }  
	}
}
