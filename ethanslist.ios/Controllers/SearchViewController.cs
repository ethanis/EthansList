using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Reflection;
using System.IO;
using EthansList.Models;
using EthansList.Shared;
using System.Collections.Generic;

namespace ethanslist.ios
{
	partial class SearchViewController : UIViewController
	{
        private int minBed = 0;
        private int minBath = 1;
        private Dictionary<string, string> searchTerms = new Dictionary<string, string>();
        private UIScrollView scrollView;

		public SearchViewController (IntPtr handle) : base (handle)
		{
            searchTerms.Add("min_price", null);
            searchTerms.Add("max_price", null);
            searchTerms.Add("bedrooms", null);
            searchTerms.Add("bathrooms", null);
            searchTerms.Add("query", null);
		}

        public String Url { get; set;}
        public String City { get; set; }

        public override void LoadView()
        {
            base.LoadView();

            SearchButton.Layer.BackgroundColor = ColorScheme.MidnightBlue.CGColor;
            SearchButton.SetTitleColor(ColorScheme.Clouds, UIControlState.Normal);
            SearchButton.Layer.CornerRadius = 10;
            SearchButton.ClipsToBounds = true;

            saveSearchButton.Layer.BackgroundColor = ColorScheme.MidnightBlue.CGColor;
            saveSearchButton.SetTitleColor(ColorScheme.Clouds, UIControlState.Normal);
            saveSearchButton.Layer.CornerRadius = 10;
            saveSearchButton.ClipsToBounds = true;

            this.View.Layer.BackgroundColor = ColorScheme.Clouds.CGColor;
            MinRentSlider.MinimumTrackTintColor = ColorScheme.MidnightBlue;
            MaxRentSlider.MinimumTrackTintColor = ColorScheme.MidnightBlue;

            MinBedCountStep.TintColor = ColorScheme.MidnightBlue;
            MinBathCountStep.TintColor = ColorScheme.MidnightBlue;
            MinBedCountStep.Layer.BackgroundColor = ColorScheme.Silver.CGColor;
            MinBathCountStep.Layer.BackgroundColor = ColorScheme.Silver.CGColor;
            MinBedCountStep.Layer.CornerRadius = 5;
            MinBathCountStep.Layer.CornerRadius = 5;
            MinBedCountStep.ClipsToBounds = true;
            MinBathCountStep.ClipsToBounds = true;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            scrollView = new UIScrollView();
            scrollView.Frame = this.View.Bounds;
            scrollView.ContentSize = UIScreen.MainScreen.Bounds.Size;
            scrollView.AddSubviews(this.View.Subviews);
            this.View.InsertSubview(scrollView, 0);
            AddLayoutConstraints();

            this.Title = "Ethan's List";
            searchCLLabel.Text = String.Format("Search {0} for:", City);

            SearchButton.Enabled = true;
            saveSearchButton.Enabled = true;
            MinLabel.Text = String.Format("{0:C0}", MinRentSlider.Value);
            MaxLabel.Text = String.Format("{0:C0}", MaxRentSlider.Value);
            MinBedLabel.Text = FormatBeds(minBed);
            MinBathLabel.Text = FormatBeds(minBath);

            MinRentSlider.ValueChanged += (object sender, EventArgs e) => {
                MinLabel.Text = String.Format("{0:C0}", MinRentSlider.Value);
            };

            MaxRentSlider.ValueChanged += (object sender, EventArgs e) => {
                MaxLabel.Text = String.Format("{0:C0}", MaxRentSlider.Value);
            };

            MinBedCountStep.ValueChanged += (object sender, EventArgs e) => {
                MinBedLabel.Text = FormatBeds((int)MinBedCountStep.Value);
            };

            MinBathCountStep.ValueChanged += (object sender, EventArgs e) => {
                MinBathLabel.Text = FormatBeds((int)MinBathCountStep.Value);
            };

            SearchField.EditingDidBegin += delegate { SearchField.BecomeFirstResponder(); };

            SearchField.ShouldReturn += delegate {
                SearchField.ResignFirstResponder();
                return true;
            };

            saveSearchButton.TouchUpInside += SaveSearchButton_TouchUpInside;
        }

        async void SaveSearchButton_TouchUpInside (object sender, EventArgs e)
        {
            await AppDelegate.databaseConnection.AddNewSearchAsync(Url, City, MinLabel.Text, MaxLabel.Text, 
                MinBedLabel.Text.ToString(), MinBathLabel.Text.ToString(), SearchField.Text);
            Console.WriteLine(AppDelegate.databaseConnection.StatusMessage);

            if (AppDelegate.databaseConnection.StatusCode == codes.ok)
            {
                UIAlertView alert = new UIAlertView();
                alert.Message = "Search Saved!";
                alert.AddButton("OK");
                alert.Show();

                saveSearchButton.Enabled = false;
            }
            else
            {
                UIAlertView alert = new UIAlertView();
                alert.Message = String.Format("Oops, something went wrong{0}Please try again...", Environment.NewLine);
                alert.AddButton("OK");
                alert.Show();

                saveSearchButton.Enabled = true;
            }
        }

        partial void SearchCL(UIButton sender)
        {
            var storyboard = UIStoryboard.FromName("Main", null);
            var feedViewController = (FeedResultsTableViewController)storyboard.InstantiateViewController("FeedResultsTableViewController");

            QueryGeneration helper = new QueryGeneration();
            searchTerms["min_price"] = MinRentSlider.Value.ToString();
            searchTerms["max_price"] = MaxRentSlider.Value.ToString();
            searchTerms["bedrooms"] = MinBedCountStep.Value.ToString();
            searchTerms["bathrooms"] = MinBathCountStep.Value.ToString();
            searchTerms["query"] = SearchField.Text;

            feedViewController.Query = helper.Generate(Url, searchTerms);

            this.ShowViewController(feedViewController, this);
        }

        public string FormatBeds(int beds)
        {
            return String.Format("{0}+", beds);
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            SearchField.ResignFirstResponder();
        }

        void AddLayoutConstraints()
        {
            this.View.RemoveConstraints(constraints: this.View.Constraints);

            SearchButton.TranslatesAutoresizingMaskIntoConstraints = false;//
            SearchField.TranslatesAutoresizingMaskIntoConstraints = false;//
            searchCLLabel.TranslatesAutoresizingMaskIntoConstraints = false;//
            MinRentLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            MaxRentLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            MinLabel.TranslatesAutoresizingMaskIntoConstraints = false;//
            MaxLabel.TranslatesAutoresizingMaskIntoConstraints = false;//
            MinRentSlider.TranslatesAutoresizingMaskIntoConstraints = false;//
            MaxRentSlider.TranslatesAutoresizingMaskIntoConstraints = false;//
            MinBathTitleLabel.TranslatesAutoresizingMaskIntoConstraints = false;//
            MinBedTitleLabel.TranslatesAutoresizingMaskIntoConstraints = false;//
            MinBedLabel.TranslatesAutoresizingMaskIntoConstraints = false;// 
            MinBathLabel.TranslatesAutoresizingMaskIntoConstraints = false;//
            MinBathCountStep.TranslatesAutoresizingMaskIntoConstraints = false;
            MinBathCountStep.TranslatesAutoresizingMaskIntoConstraints = false;
            saveSearchButton.TranslatesAutoresizingMaskIntoConstraints = false;

            //Seach CL Label Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(searchCLLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .90f, 0),
                NSLayoutConstraint.Create(searchCLLabel, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(searchCLLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Top, 1, 100),
            });
            //Seach Field Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(SearchField, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .90f, 0),
                NSLayoutConstraint.Create(SearchField, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(SearchField, NSLayoutAttribute.Top, NSLayoutRelation.Equal, searchCLLabel, NSLayoutAttribute.Bottom, 1, 25),
            });
            //Seach Button Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(SearchButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .90f, 0),
                NSLayoutConstraint.Create(SearchButton, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(SearchButton, NSLayoutAttribute.Top, NSLayoutRelation.Equal, SearchField, NSLayoutAttribute.Bottom, 1, 25),
            });
            //Min Rent Label Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(MinRentLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .50f, 0),
                NSLayoutConstraint.Create(MinRentLabel, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 0.50f, 0),
                NSLayoutConstraint.Create(MinRentLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, SearchButton, NSLayoutAttribute.Bottom, 1, 50),
            });
            //Max Rent Label Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(MaxRentLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .50f, 0),
                NSLayoutConstraint.Create(MaxRentLabel, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1.5f, 0),
                NSLayoutConstraint.Create(MaxRentLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, SearchButton, NSLayoutAttribute.Bottom, 1, 50),
            });
            //Min Label Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(MinLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .50f, 0),
                NSLayoutConstraint.Create(MinLabel, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 0.50f, 0),
                NSLayoutConstraint.Create(MinLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, MinRentLabel, NSLayoutAttribute.Bottom, 1, 25),
            });
            //Max Label Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(MaxLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .50f, 0),
                NSLayoutConstraint.Create(MaxLabel, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1.5f, 0),
                NSLayoutConstraint.Create(MaxLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, MaxRentLabel, NSLayoutAttribute.Bottom, 1, 25),
            });
            //Min Rent Slider Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(MinRentSlider, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .45f, 0),
                NSLayoutConstraint.Create(MinRentSlider, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 0.50f, 0),
                NSLayoutConstraint.Create(MinRentSlider, NSLayoutAttribute.Top, NSLayoutRelation.Equal, MinLabel, NSLayoutAttribute.Bottom, 1, 25),
            });
            //Max Rent Slider Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(MaxRentSlider, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .45f, 0),
                NSLayoutConstraint.Create(MaxRentSlider, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1.5f, 0),
                NSLayoutConstraint.Create(MaxRentSlider, NSLayoutAttribute.Top, NSLayoutRelation.Equal, MaxLabel, NSLayoutAttribute.Bottom, 1, 25),
            });
            //Min Bed Title Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(MinBedTitleLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .45f, 0),
                NSLayoutConstraint.Create(MinBedTitleLabel, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 0.50f, 0),
                NSLayoutConstraint.Create(MinBedTitleLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, MinRentSlider, NSLayoutAttribute.Bottom, 1, 25),
            });
            //Min Bath Title Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(MinBathTitleLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .45f, 0),
                NSLayoutConstraint.Create(MinBathTitleLabel, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1.5f, 0),
                NSLayoutConstraint.Create(MinBathTitleLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, MaxRentSlider, NSLayoutAttribute.Bottom, 1, 25),
            });
            //Min Bed Label Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(MinBedLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .50f, 0),
                NSLayoutConstraint.Create(MinBedLabel, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 0.5f, 0),
                NSLayoutConstraint.Create(MinBedLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, MinBedTitleLabel, NSLayoutAttribute.Bottom, 1, 25),
            });
            //Min Bath Label Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(MinBathLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .45f, 0),
                NSLayoutConstraint.Create(MinBathLabel, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1.50f, 0),
                NSLayoutConstraint.Create(MinBathLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, MinBathTitleLabel, NSLayoutAttribute.Bottom, 1, 25),
            });
            //Min Bed Stepper Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(MinBedCountStep, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .50f, 0),
                NSLayoutConstraint.Create(MinBedCountStep, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 0.5f, 0),
                NSLayoutConstraint.Create(MinBedCountStep, NSLayoutAttribute.Top, NSLayoutRelation.Equal, MinBedLabel, NSLayoutAttribute.Bottom, 1, 25),
            });
            //Min Bath Stepper Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(MinBathCountStep, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .45f, 0),
                NSLayoutConstraint.Create(MinBathCountStep, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1.50f, 0),
                NSLayoutConstraint.Create(MinBathCountStep, NSLayoutAttribute.Top, NSLayoutRelation.Equal, MinBathLabel, NSLayoutAttribute.Bottom, 1, 25),
            });
            //Save Search Button Constraints
            this.View.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(saveSearchButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.Width, .45f, 0),
                NSLayoutConstraint.Create(saveSearchButton, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this.View, NSLayoutAttribute.CenterX, 1.50f, 0),
                NSLayoutConstraint.Create(saveSearchButton, NSLayoutAttribute.Top, NSLayoutRelation.Equal, MinBedCountStep, NSLayoutAttribute.Bottom, 1, 25),
            });

            this.View.LayoutIfNeeded();
        }
	}
}
