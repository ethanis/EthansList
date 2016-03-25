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
        readonly Query MinPriceField, MaxPriceField, MinBedroomsField, MinBathroomsField, PostedDateField, MaxListingsField;
        readonly string DoneButton = "Done";

        public SearchOptionsPage()
            : base("Search Terms", "save.png")
        {
            if (OnAndroid)
            {
                SaveButton = "save_action_button";
                SearchTermsField = x => x.Class("TextView").Index(2);
                MinPriceField = x => x.Marked("Price").Sibling().Descendant().Index(1);
                MaxPriceField = x => x.Marked("Price").Sibling().Descendant().Index(2);
                MinBedroomsField = x => x.Marked("Bedrooms").Sibling();
                MinBathroomsField = x => x.Marked("Bathrooms").Sibling();
                PostedDateField = x => x.Marked("Posted Date").Sibling();
                MaxListingsField = x => x.Marked("Max Listings").Sibling();
            }
            if (OniOS)
            {
                SaveButton = "save.png";
                SearchTermsField = x => x.Id("SearchTermsField");
                MaxPriceField = x => x.Id("MaxPriceField");
                MinPriceField = x => x.Id("MinPriceField");
                MinBedroomsField = x => x.Text("Min Bedrooms").Sibling().Marked("PickerTextField");
                MinBathroomsField = x => x.Text("Min Bathrooms").Sibling().Marked("PickerTextField");
                PostedDateField = x => x.Text("Posted Date").Sibling().Marked("PickerTextField");
                MaxListingsField = x => x.Text("Max Listings").Sibling().Marked("PickerTextField");
            }
        }

        public SearchOptionsPage VerifyOnLocation(string state)
        {
            if (OniOS)
                app.WaitForElement(string.Format("Search {0} for:", state));
            if (OnAndroid)
                app.WaitForElement(string.Format("Search {0} for: ", state));

            app.Screenshot("Verified searching: " + state);
            return this;
        }

        public void ProceedToSearch()
        {
            app.DismissKeyboard();
            app.ScrollUpTo(SearchButton);
            app.Tap(SearchButton);
            app.Screenshot("Tapped Search Button");
        }

        public SearchOptionsPage SaveSearch()
        {
            app.Tap(SaveButton);
            app.Screenshot("Tapped Save Button");
            if (OniOS)
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
            app.Tap(MinPriceField);
            app.EnterText(min.ToString());
            app.Tap(MaxPriceField);
            app.EnterText(max.ToString());
            app.Screenshot("Max and min price entered");
            app.DismissKeyboard();

            return this;
        }

        public SearchOptionsPage SelectMinBedrooms(int i)
        {
            app.ScrollDownTo(MinBedroomsField);
            app.Tap(MinBedroomsField);
            if (OniOS)
            {
                app.Tap(i + "+");
                app.Screenshot("Min bedrooms selected");
                app.Tap(DoneButton);
            }
            if (OnAndroid)
            {
                app.Query(x => x.Id("numberPicker").Invoke("setValue", i));
                app.Tap(i + "+");
                app.Screenshot("Min bedrooms selected");
                app.Tap("Ok");
            }

            return this;
        }

        public SearchOptionsPage SelectMinBathrooms(int i)
        {
            app.ScrollDownTo(MinBathroomsField);
            app.Tap(MinBathroomsField);
            if (OniOS)
            {
                app.Tap(i + "+");
                app.Screenshot("Min bathrooms selected");
                app.Tap(DoneButton);
            }
            if (OnAndroid)
            {
                app.Query(x => x.Id("numberPicker").Invoke("setValue", i));
                app.Tap(i + "+");
                app.Screenshot("Min bathrooms selected");
                app.Tap("Ok");
            }

            return this;
        }

        public SearchOptionsPage SelectPostedDate(string i)
        {
            app.ScrollDownTo(PostedDateField);
            app.Tap(PostedDateField);
            app.Tap(i);
            app.Screenshot("Posted date selected");
            if (OniOS)
                app.Tap(DoneButton);

            return this;
        }

        public SearchOptionsPage SelectMaxListings(int i)
        {
            app.ScrollDownTo(MaxListingsField);
            app.Tap(MaxListingsField);
            app.Tap(string.Format("{0}", i));
            app.Screenshot("Max listings selected");
            if (OniOS)
                app.Tap(DoneButton);

            return this;
        }
    }
}

