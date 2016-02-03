using System;
using Xamarin.UITest;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;
using System.Linq;

namespace ethanslist.UITests
{
    public class SearchOptionsPage : BasePage
    {
        readonly string SearchButton = "Search";
        readonly string SaveButton;
        readonly Query SearchTermsField;
        readonly Query MaxPriceField, MinBedroomsField, MinBathroomsField, PostedDateField, MaxListingsField;
        readonly string DoneButton = "Done";

        public SearchOptionsPage()
            : base ("androidTrait", "save.png")
        {
            if (OniOS)
            {
                SaveButton = "save.png";
                SearchTermsField = x => x.Marked("Search rental properties...");
                MaxPriceField = x => x.Text("Price").Sibling().Class("UITextField");
                MinBedroomsField = x => x.Text("Min Bedrooms").Sibling().Marked("PickerTextField");
                MinBathroomsField = x => x.Text("Min Bathrooms").Sibling().Marked("PickerTextField");
                PostedDateField = x => x.Text("Posted Date").Sibling().Marked("PickerTextField");
                MaxListingsField = x => x.Text("Max Listings").Sibling().Marked("PickerTextField");
            }
        }

        public SearchOptionsPage VerifyOnLocation(string state)
        {
            app.WaitForElement(string.Format("Search {0} for:", state));
            app.Screenshot("Verified searching: " + state);
            return this;
        }

        public void ProceedToSearch()
        {
            app.ScrollUpTo(SearchButton);
            app.Tap(SearchButton);
            app.Screenshot("Tapped Search Button");
        }

        public SearchOptionsPage SaveSearch()
        {
            app.Tap(SaveButton);
            app.Screenshot("Tapped Save Button");
            app.Tap("OK");

            return this;
        }

        public SearchOptionsPage EnterSearchTerms(string[] terms)
        {
            app.Tap(SearchTermsField);
            app.EnterText(string.Join(" ", terms));
            app.Screenshot("Entered terms into search box");
            app.DismissKeyboard();

            return this;
        }

        public SearchOptionsPage SelectMinMaxPrice(int min, int max)
        {
            app.ScrollDownTo(MaxPriceField);
            app.Tap(MaxPriceField);
            app.Screenshot("Max and min price entered");
            app.Tap(DoneButton);

            return this;
        }

        public SearchOptionsPage SelectMinBedrooms(int i)
        {
            app.ScrollDownTo(MinBedroomsField);
            app.Tap(MinBedroomsField);
            app.Tap(i + "+");
            app.Screenshot("Min bedrooms selected");
            app.Tap(DoneButton);

            return this;
        }

        public SearchOptionsPage SelectMinBathrooms(int i)
        {
            app.ScrollDownTo(MinBathroomsField);
            app.Tap(MinBathroomsField);
            app.Tap(i + "+");
            app.Screenshot("Min bathrooms selected");
            app.Tap(DoneButton);

            return this;
        }

        public SearchOptionsPage SelectPostedDate(string i)
        {
            app.ScrollDownTo(PostedDateField);
            app.Tap(PostedDateField);
            app.Tap(i);
            app.Screenshot("Posted date selected");
            app.Tap(DoneButton);

            return this;
        }

        public SearchOptionsPage SelectMaxListings(int i)
        {
            app.ScrollDownTo(MaxListingsField);
            app.Tap(MaxListingsField);
            app.Tap(string.Format("{0}",i));
            app.Screenshot("Max listings selected");
            app.Tap(DoneButton);

            return this;
        }
    }
}

