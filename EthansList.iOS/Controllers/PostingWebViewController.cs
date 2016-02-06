using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ethanslist.ios
{
	partial class PostingWebViewController : UIViewController
	{
        public string PostingLink { get; set; }

		public PostingWebViewController (IntPtr handle) : base (handle)
		{
		}

        public override void LoadView()
        {
            base.LoadView();

            this.View.Layer.BackgroundColor = ColorScheme.Clouds.CGColor;
            this.navBar.BarTintColor = ColorScheme.WetAsphalt;
            statusBarPlaceholder.BackgroundColor = UIColor.FromRGB(0.2745f, 0.3451f, 0.4157f);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            webView.LoadRequest(new NSUrlRequest(new NSUrl(PostingLink)));
            webView.ScalesPageToFit = true;

            DoneButton.Clicked += (object sender, EventArgs e) => {
                this.DismissViewController(true, null);
            };
        }
	}
}
