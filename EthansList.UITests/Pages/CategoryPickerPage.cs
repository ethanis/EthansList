using System;
using Xamarin.UITest;
using NUnit.Framework;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;
using System.Linq;

namespace ethanslist.UITests
{
    public class CategoryPickerPage : BasePage
    {
        readonly Query CategoryTable;

        public CategoryPickerPage()
            : base("Housing", "Category")
        {
            if (OniOS)
            {
                CategoryTable = x => x.Id("CategoryPickerTable");
            }
        }

        public void SelectCategory(string cat)
        {
            if (OniOS)
                app.ScrollDownTo(x => x.Marked(cat), CategoryTable, timeout: TimeSpan.FromSeconds(20));
            else
            {
                //int count = 0;
                //while (!app.Query(cat).Any() && count < 10)
                //{ 
                //    app.ScrollDown();
                //    count++;
                //}
                app.ScrollTo(cat);
            }
            app.Screenshot("Scrolled down to category: " + cat);
            app.Tap(cat);
        }
    }
}

