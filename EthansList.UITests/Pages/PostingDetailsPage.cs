using System;
using Xamarin.UITest;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;
using NUnit.Framework;

namespace ethanslist.UITests
{
    public class PostingDetailsPage : BasePage
    {
        readonly Query SaveButton;
        readonly Query DoneButton;
        readonly Query TitleLabel;

        public PostingDetailsPage()
            : base ("androidTrait", "Posting Details")
        {
            if (OniOS)
            {
                SaveButton = x => x.Id("save.png");
                DoneButton = x => x.Marked("Done");
                TitleLabel = x => x.Class("UILabel").Index(2);
            }
        }

        public PostingDetailsPage SaveListing()
        {
            app.Tap(SaveButton);
            app.WaitForElement("Listing Saved!");
            app.Screenshot("Tapped Save Button");
            app.Tap("OK");
            app.Screenshot("Dismissed Dialog");

            var result = app.Query(SaveButton)[0].Enabled;
            Assert.IsFalse(result, "Save button not disabled after save");

            return this;
        }

        public string GetListingTitle()
        {
            return app.Query(TitleLabel)[0].Label;
        }

        public void ExitListing()
        {
            app.Tap(DoneButton);
        }
    }
}

