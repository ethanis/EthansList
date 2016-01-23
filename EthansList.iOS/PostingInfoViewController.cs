using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Collections.Generic;

namespace ethanslist.ios
{
	partial class PostingInfoViewController : UIViewController
	{
        PostingInfoTableSource tableSource;

		public PostingInfoViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            tableSource = new PostingInfoTableSource(this, GetTableSetup());
            PostingInfoTableView.Source = tableSource;
        }

        private List<TableItem> GetTableSetup()
        {
            List<TableItem> tableItems = new List<TableItem>();

            tableItems.Add(new TableItem()
                { 
                    Heading = "Posting Title",
                    CellType = "PostingTitle",
                });
            tableItems.Add(new TableItem()
                {
                    Heading = "Posting Image",
                    CellType = "PostingImage",
                });
            tableItems.Add(new TableItem()
                {
                    Heading = "Image Collection",
                    CellType = "ImageCollection",
                });
            tableItems.Add(new TableItem()
                {
                    Heading = "Posting Description",
                    CellType = "PostingDescription",
                });
            tableItems.Add(new TableItem()
                {
                    Heading = "Posting Date",
                    CellType = "PostingDate",
                });

            return tableItems;
        }
	}
}
