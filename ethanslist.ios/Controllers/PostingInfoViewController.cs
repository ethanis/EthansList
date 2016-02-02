using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Collections.Generic;
using EthansList.Models;
using SDWebImage;
using EthansList.Shared;
using System.Drawing;

namespace ethanslist.ios
{
	partial class PostingInfoViewController : UIViewController
	{
        PostingInfoTableSource tableSource;
        UIBarButtonItem saveButton;
        UIBarButtonItem deleteButton;
        ListingImageDownloader imageHelper;
        List<TableItem> tableItems;
        public Posting Post { get; set; }
        public event EventHandler<EventArgs> ItemDeleted;

		public PostingInfoViewController (IntPtr handle) : base (handle)
		{
		}

        public override void LoadView()
        {
            base.LoadView();

            this.View.Layer.BackgroundColor = ColorScheme.Clouds.CGColor;
            this.myNavBar.BarTintColor = ColorScheme.WetAsphalt;
            NavBarPlaceholder.BackgroundColor = UIColor.FromRGB(0.2745f, 0.3451f, 0.4157f);
            PostingInfoTableView.BackgroundColor = ColorScheme.Clouds;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            imageHelper = new ListingImageDownloader(Post.Link, Post.ImageLink);
            tableItems = GetTableSetup();
            tableSource = new PostingInfoTableSource(this, tableItems, Post, imageHelper);
            PostingInfoTableView.Source = tableSource;
            PostingInfoTableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            myNavItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Done, DismissClicked),true);

            if (!AppDelegate.databaseConnection.PostingAlreadySaved(Post))
            {
                saveButton = new UIBarButtonItem (
                    UIImage.FromFile ("save.png"),
                    UIBarButtonItemStyle.Plain,
                    SaveListing
                );
                saveButton.Enabled = true;
                myNavItem.LeftBarButtonItem = saveButton;
            }
            else
            {
                deleteButton = new UIBarButtonItem(UIBarButtonSystemItem.Trash, DeleteListing);
                myNavItem.SetLeftBarButtonItem(deleteButton, true);
            }

            tableSource.DescriptionLoaded += (object sender, DescriptionLoadedEventArgs e) => {
                PostingInfoTableView.ReloadRows(new NSIndexPath[] {e.DescriptionRow}, UITableViewRowAnimation.Automatic);
            };

            imageHelper.loadingComplete += ImageHelper_loadingComplete;
        }

        void ImageHelper_loadingComplete (object sender, EventArgs e)
        {
            if (imageHelper.PostingMapFound)
            {
                tableItems.Insert(4, new TableItem()
                    {
                        Heading = "Posting Map",
                        CellType = "PostingMap",
                    });
            }

            PostingInfoTableView.ReloadData();
        }

        async void SaveListing(object sender, EventArgs e)
        {
            await AppDelegate.databaseConnection.AddNewListingAsync(Post.PostTitle, Post.Description, Post.Link, Post.ImageLink, Post.Date);
            Console.WriteLine(AppDelegate.databaseConnection.StatusMessage);

            if (AppDelegate.databaseConnection.StatusCode == codes.ok)
            {
                UIAlertView alert = new UIAlertView();
                alert.Message = "Listing Saved!";
                alert.AddButton("OK");
                alert.Show();

                saveButton.Enabled = false;
            }
            else
            {
                UIAlertView alert = new UIAlertView();
                alert.Message = String.Format("Oops, something went wrong{0}Please try again...", Environment.NewLine);
                alert.AddButton("OK");
                alert.Show();

                saveButton.Enabled = true;
            }
        }

        void DeleteListing(object sender, EventArgs e)
        {
            if (this.ItemDeleted != null)
                this.ItemDeleted(this, new EventArgs());
        }

        void DismissClicked(object sender, EventArgs e)
        {
            this.DismissViewController(true, null);
        }

        private List<TableItem> GetTableSetup()
        {
            List<TableItem> tableItems = new List<TableItem>();

            tableItems.Add(new TableItem()
                { 
                    Heading = "Posting Title",
                    CellType = "PostingTitleCell",
                });
            if (Post.ImageLink != "-1")
            {
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
            }
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
            tableItems.Add(new TableItem()
                {
                    
                    Heading = "Posting Link",
                    CellType = "PostingLink",
                });
            return tableItems;
        }
	}
}
