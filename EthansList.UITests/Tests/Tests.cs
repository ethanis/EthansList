using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace ethanslist.UITests
{
    public class Tests : AbstractSetup
    {
        public Tests(Platform platform)
            :base(platform)
        {
        }

        [Test]
        public void Repl()
        {
            app.Repl();
        }
            
        [Test]
        public void SearchBayArea()
        {
            string state = "California";
            string area = "San Francisco Bay Area";

            new CityPickerPage()
                .SelectStateAtRow(3, state)
                .SelectCityAtRow(19, area)
                .ProceedToSearchOptions();

            new SearchOptionsPage()
                .VerifyOnLocation(area);
        }
    }
}

