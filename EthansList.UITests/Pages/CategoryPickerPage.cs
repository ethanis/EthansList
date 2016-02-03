using System;
using Xamarin.UITest;
using NUnit.Framework;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace ethanslist.UITests
{
    public class CategoryPickerPage : BasePage
    {
        readonly Query CategoryTable;

        public CategoryPickerPage()
            : base ("android trait", "Category")
        {
            if (OniOS)
            {
                CategoryTable = x => x.Id("CategoryPickerTable");
            }
        }

        public void SelectCategory(string cat)
        {
            app.ScrollDownTo(x => x.Marked(cat), CategoryTable, timeout: TimeSpan.FromSeconds(20));
            app.Screenshot("Scrolled down to category: " + cat);
            app.Tap(cat);
        }
    }
}

