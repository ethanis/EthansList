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
                var coords = app.Query(x => x.Id("frameLayout"))[0].Rect;
                int count = 0;
                while (!app.Query(cat).Any() && count < 25)
                {
                    //app.ScrollDown();
                    app.DragCoordinates(coords.CenterX, coords.Height * 0.75f, coords.CenterX, coords.Height * 0.25f);
                    System.Threading.Thread.Sleep(500);
                    count++;
                }
                //app.ScrollDownTo(cat, timeout: TimeSpan.FromSeconds(20));
            }
            app.Screenshot("Scrolled down to category: " + cat);
            app.Tap(cat);
        }
    }
}

