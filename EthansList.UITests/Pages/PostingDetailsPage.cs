using System;
using Xamarin.UITest;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;
using NUnit.Framework;
using System.Linq;

namespace ethanslist.UITests
{
    public class PostingDetailsPage : BasePage
    {
        readonly Query SaveButton;
        readonly Query DoneButton;
        readonly Query TitleLabel;

        public PostingDetailsPage()
            : base("save_action_button", "Posting Info")
        {
            if (OnAndroid)
            {
                SaveButton = x => x.Id("save_action_button");
                TitleLabel = x => x.Class("TextView").Index(1);
            }
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
            app.Screenshot("Tapped Save Button");

            if (OniOS)
                app.WaitForElement("Listing Saved!");
            if (OnAndroid)
                app.WaitForElement("Saved Posting!");

            if (OniOS)
            {
                app.Tap("OK");
                app.Screenshot("Dismissed Dialog");
                var result = app.Query(SaveButton)[0].Enabled;
                Assert.IsFalse(result, "Save button not disabled after save");
            }
            if (OnAndroid)
            {
                //TODO: implement this for android
            }
            return this;
        }

        public string GetListingTitle()
        {
            return app.Query(TitleLabel)[0].Label;
        }

        public PostingDetailsPage LookAtPosting()
        {
            int count = 0;
            while (!app.Query("Original Listing").Any() && count < 5)
            {
                app.ScrollDown();
                app.Screenshot("Scrolled down " + count + " times");
                count++;
            }

            for (int i = count; i < 5; i++)
                app.Screenshot("Scrolled down " + count + " times");

            return this;
        }

        public void ExitListing()
        {
            if (OniOS)
            {
                app.Tap(DoneButton);
            }
            if (OnAndroid)
                base.GoBack();
        }
    }
}

