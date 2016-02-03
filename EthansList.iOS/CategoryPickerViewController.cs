using System;

using UIKit;
using System.Collections.Generic;
using EthansList.Shared;

namespace ethanslist.ios
{
    public partial class CategoryPickerViewController : UIViewController
    {
        UITableView categoryTableView;
        CategoryTableViewSource categoryTableSource;
        public Location SelectedCity { get; set;}


        public CategoryPickerViewController()
            : base("CategoryPickerViewController", null)
        {
        }

        public override void LoadView()
        {
            base.LoadView();

            categoryTableView = new UITableView(this.View.Frame, UITableViewStyle.Plain);

            this.View.AddSubview(categoryTableView);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            categoryTableSource = new CategoryTableViewSource(this, Categories.all);
            categoryTableView.Source = categoryTableSource;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }

    public class CategoryTableViewSource : UITableViewSource
    {
        CategoryPickerViewController owner;
        Dictionary<int, KeyValuePair<string, string>> categories;
        const string cellID = "cellID";

        public CategoryTableViewSource(CategoryPickerViewController owner, Dictionary<int, KeyValuePair<string, string>> categories)
        {
            this.owner = owner;
            this.categories = categories;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return categories.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(cellID);
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellID);

            cell.TextLabel.Text = categories[indexPath.Row].Value;

            return cell;
        }

        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            Console.WriteLine(categories[indexPath.Row].Key);

            var storyboard = UIStoryboard.FromName("Main", null);
            var searchViewController = (SearchOptionsViewController)storyboard.InstantiateViewController("SearchOptionsViewController");

            searchViewController.Location = owner.SelectedCity;
            searchViewController.Category = categories[indexPath.Row].Key;

            this.owner.ShowViewController(searchViewController, this.owner);
        }
    }
}


