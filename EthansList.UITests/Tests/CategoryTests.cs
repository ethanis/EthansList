using System;
using Xamarin.UITest;
using NUnit.Framework;

namespace ethanslist.UITests
{
    public class CategoryTests : AbstractSetup
    {
        public CategoryTests(Platform platform)
            :base(platform)
        {
        }

        [Category("CategoryTests")]
        [TestCase("apartments / housing rentals")]
        [TestCase("housing swap")]
        [TestCase("housing wanted")]
        [TestCase("office / commercial")]
        [TestCase("parking / storage")]
        [TestCase("real estate for sale")]
        [TestCase("rooms / shared")]
        [TestCase("rooms wanted")]
        [TestCase("sublets / temporary")]
        [TestCase("vacation rentals")]
        [TestCase("antiques")]
        [TestCase("appliances")]
        [TestCase("arts+crafts")]
        [TestCase("atv/utv/sno")]
        [TestCase("auto parts")]
        [TestCase("baby+kid")]
        [TestCase("barter")]
        [TestCase("beauty+hlth")]
        [TestCase("bikes")]
        [TestCase("boats")]
        [TestCase("books")]
        [TestCase("business")]
        [TestCase("cars+trucks")]
        [TestCase("cds/dvd/vhs")]
        [TestCase("cell phones")]
        [TestCase("clothes+acc")]
        [TestCase("collectibles")]
        [TestCase("computers")]
        [TestCase("electronics")]
        [TestCase("farm+garden")]
        [TestCase("free")]
        [TestCase("furniture")]
        [TestCase("garage sale")]
        [TestCase("general")]
        [TestCase("heavy equip")]
        [TestCase("household")]
        [TestCase("jewelry")]
        [TestCase("materials")]
        [TestCase("motorcycles")]
        [TestCase("music instr")]
        [TestCase("photo+video")]
        [TestCase("rvs+camp")]
        [TestCase("sporting")]
        [TestCase("tickets")]
        [TestCase("tools")]
        [TestCase("toys+games")]
        [TestCase("video gaming")]
        [TestCase("wanted")]
        [TestCase("automotive")]
        [TestCase("beauty")]
        [TestCase("computer")]
        [TestCase("creative")]
        [TestCase("cycle")]
        [TestCase("event")]
        [TestCase("farm+garden")]
        [TestCase("financial")]
        [TestCase("household")]
        [TestCase("labor/move")]
        [TestCase("legal")]
        [TestCase("lessons")]
        [TestCase("marine")]
        [TestCase("pet")]
        [TestCase("real estate")]
        [TestCase("skill'd trade")]
        [TestCase("sm biz ads")]
        [TestCase("therapeutic")]
        [TestCase("travel/vac")]
        [TestCase("write/ed/tr8")]
        [TestCase("accounting+finance")]
        [TestCase("admin / office")]
        [TestCase("arch / engineering")]
        [TestCase("art / media / design")]
        [TestCase("biotech / science")]
        [TestCase("business / mgmt")]
        [TestCase("customer service")]
        [TestCase("education")]
        [TestCase("food / bev / hosp")]
        [TestCase("general labor")]
        [TestCase("government")]
        [TestCase("human resources")]
        [TestCase("internet engineers")]
        [TestCase("legal / paralegal")]
        [TestCase("manufacturing")]
        [TestCase("marketing / pr / ad")]
        [TestCase("medical / health")]
        [TestCase("nonprofit sector")]
        [TestCase("real estate")]
        [TestCase("retail / wholesale")]
        [TestCase("sales / biz dev")]
        [TestCase("salon / spa / fitness")]
        [TestCase("security")]
        [TestCase("skilled trade / craft")]
        [TestCase("software / qa / dba")]
        [TestCase("systems / network")]
        [TestCase("technical support")]
        [TestCase("transport")]
        [TestCase("tv / film / video")]
        [TestCase("web / info design")]
        [TestCase("writing / editing")]
        [TestCase("[ETC]")]
        [TestCase("[ part-time ]")]
        [TestCase("computer")]
        [TestCase("creative")]
        [TestCase("crew")]
        [TestCase("domestic")]
        [TestCase("event")]
        [TestCase("labor")]
        [TestCase("talent")]
        [TestCase("writing")]
        public void CheckCategories(string cat)
        {
            string state = "California";
            string area = "San Francisco Bay Area";

            new CityPickerPage()
                .SelectState(state)
                .SelectCity(area);

            new CategoryPickerPage()
                .SelectCategory(cat);
            
            new SearchOptionsPage()
                .ProceedToSearch();

            new FeedResultsPage()
                .VerifyCatValid();
        }
    }
}

