using System;
using UIKit;
using EthansList.Shared;
using System.Collections.Generic;

namespace ethanslist.ios
{
    public class FavoriteCategoryViewController : UIViewController
    {
        UIView BarTintPlaceholder;
        UITableView FavoriteCatTableView;
        FavoriteCategoryTableSource FavoriteCatTableSource;
        UINavigationBar myNavBar;
        UINavigationItem NavBarItem;
        UIBarButtonItem DismissButton;
        public event EventHandler<FavoriteSelectedEventArgs> FavoriteSelected;

        public FavoriteCategoryViewController()
            :base()
        {
        }

        public override void LoadView()
        {
            base.LoadView();

            BarTintPlaceholder = new UIView(new CoreGraphics.CGRect(0,0, this.View.Bounds.Height, 20));
            BarTintPlaceholder.BackgroundColor = UIColor.FromRGB(0.2745f, 0.3451f, 0.4157f);

            FavoriteCatTableView = new UITableView();
            FavoriteCatTableView.BackgroundColor = ColorScheme.Clouds;
            myNavBar = new UINavigationBar(new CoreGraphics.CGRect(0,20,this.View.Bounds.Width, 44));
            myNavBar.BackgroundColor = ColorScheme.WetAsphalt;
            NavBarItem = new UINavigationItem("Favorite Categories");
            this.View.AddSubviews(new UIView[]{BarTintPlaceholder, FavoriteCatTableView, myNavBar});

            FavoriteCatTableView.TranslatesAutoresizingMaskIntoConstraints = false;
            this.View.AddConstraints(new NSLayoutConstraint[]{
                NSLayoutConstraint.Create(FavoriteCatTableView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, myNavBar, NSLayoutAttribute.Bottom, 1, 0),
                NSLayoutConstraint.Create(FavoriteCatTableView, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Left, 1, 0),
                NSLayoutConstraint.Create(FavoriteCatTableView, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Right, 1, 0),
                NSLayoutConstraint.Create(FavoriteCatTableView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Bottom, 1, 0),
            });
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            DismissButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (s, e) =>
                {
                    this.DismissViewControllerAsync(true);
                });
            NavBarItem.RightBarButtonItem = DismissButton;
            myNavBar.SetItems(new UINavigationItem[]{NavBarItem}, true);

            var favorites = AppDelegate.databaseConnection.GetAllFavoriteCategoriesAsync().Result;
            favorites.Sort((s1, s2)=>s2.Updated.CompareTo(s1.Updated));

            FavoriteCatTableSource = new FavoriteCategoryTableSource(this, favorites);
            FavoriteCatTableView.Source = FavoriteCatTableSource;

            FavoriteCatTableSource.Selected += (object sender, FavoriteSelectedEventArgs e) => {
                if (this.FavoriteSelected != null)
                    this.FavoriteSelected(this, e);
            };
        }
    }

    public class FavoriteCategoryTableSource : UITableViewSource
    {
        FavoriteCategoryViewController owner;
        List<FavoriteCategory> favorites;
        const string cellID = "favoritesCell";
        public event EventHandler<FavoriteSelectedEventArgs> Selected;

        public FavoriteCategoryTableSource(FavoriteCategoryViewController owner, List<FavoriteCategory> favorites)
        {
            this.owner = owner;
            this.favorites = favorites;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return favorites.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(cellID);
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellID);

            cell.TextLabel.AttributedText = new Foundation.NSAttributedString(favorites[indexPath.Row].CategoryValue, Constants.CityPickerCellAttributes);

            return cell;
        }

        public override async void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath)
        {
            switch (editingStyle) {
                case UITableViewCellEditingStyle.Delete:
                    await AppDelegate.databaseConnection.DeleteFavoriteCategoryAsync(favorites[indexPath.Row]);
                    favorites.RemoveAt(indexPath.Row);
                    tableView.DeleteRows(new [] { indexPath }, UITableViewRowAnimation.Fade);
                    break;
                case UITableViewCellEditingStyle.None:
                    Console.WriteLine ("CommitEditingStyle:None called");
                    break;
            }
        }

        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            if (this.Selected != null)
                this.Selected(this, new FavoriteSelectedEventArgs(){ Selected = favorites[indexPath.Row] });
        }
    }

    public class FavoriteSelectedEventArgs : EventArgs
    {
        public FavoriteCategory Selected {get;set;}
    }
}

