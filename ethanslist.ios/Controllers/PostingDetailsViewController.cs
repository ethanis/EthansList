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
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            PostingTitle.Text = post.Title;
            PostingDescription.Text = post.Description;

            BackButton.Clicked += OnDismiss;
        }


        void OnDismiss(object sender, EventArgs e)
        {
            DismissViewController(true, null);
        }  
	}
}
