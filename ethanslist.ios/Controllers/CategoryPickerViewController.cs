using System;

using UIKit;
using System.Collections.Generic;
using EthansList.Shared;
using Foundation;

namespace ethanslist.ios
{
    public partial class CategoryPickerViewController : UIViewController
    {
        UITableView categoryTableView;
        CategoryTableViewSource categoryTableSource;
        public Location SelectedCity { get; set;}


        public CategoryPickerViewController()
            : base()
        {
        }

        public override void LoadView()
        {
            base.LoadView();

            this.View.BackgroundColor = ColorScheme.Clouds;
            this.Title = "Category";

            categoryTableView = new UITableView(this.View.Bounds, UITableViewStyle.Plain);
            categoryTableView.BackgroundColor = ColorScheme.Clouds;
            categoryTableView.AccessibilityIdentifier = "CategoryPickerTable";
            this.View.AddSubview(categoryTableView);

            categoryTableView.TranslatesAutoresizingMaskIntoConstraints = false;
            this.View.AddConstraints(new NSLayoutConstraint[]{
                NSLayoutConstraint.Create(categoryTableView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.TopMargin, 1, 0),
                NSLayoutConstraint.Create(categoryTableView, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Left, 1, 0),
                NSLayoutConstraint.Create(categoryTableView, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Right, 1, 0),
                NSLayoutConstraint.Create(categoryTableView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Bottom, 1, 0),
            });
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
        List<KeyValuePair<string, string>> categories;
        const string cellID = "cellID";

        public CategoryTableViewSource(CategoryPickerViewController owner, List<KeyValuePair<string, string>> categories)
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

            cell.TextLabel.AttributedText = new NSAttributedString(categories[indexPath.Row].Value, Constants.CityPickerCellAttributes);
            cell.BackgroundColor = ColorScheme.Clouds;

            return cell;
        }

        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            Console.WriteLine(categories[indexPath.Row].Key);

            var storyboard = UIStoryboard.FromName("Main", null);
            var searchViewController = (SearchOptionsViewController)storyboard.InstantiateViewController("SearchOptionsViewController");

            searchViewController.Location = owner.SelectedCity;
            searchViewController.Category = categories[indexPath.Row];

            this.owner.ShowViewController(searchViewController, this.owner);
        }

        public override nfloat GetHeightForRow(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            return Constants.CityPickerRowHeight;
        }
    }
}


