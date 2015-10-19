using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ethanslist.ios
{
	partial class SearchViewController : UIViewController
	{
		public SearchViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.Title = "Ethan's List";

            SearchButton.Enabled = true;
        }

        partial void SearchCL(UIButton sender)
        {
            if (SearchField.Text == null)
            {
                return;
            }
            else
            {
                var storyboard = UIStoryboard.FromName("Main", null);
                var feedViewController = (FeedResultsTableViewController)storyboard.InstantiateViewController("FeedResultsTableViewController");

                feedViewController.Query = SearchField.Text;

                this.ShowViewController(feedViewController, this);
            }
        }
	}
}
