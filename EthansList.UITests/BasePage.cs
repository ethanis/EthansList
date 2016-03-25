using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using NUnit.Framework;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;
using System.Linq;
using Xamarin.UITest.Android;
using Xamarin.UITest.iOS;

namespace ethanslist.UITests
{
    public class BasePage
    {
        protected readonly IApp app;
        protected readonly bool OnAndroid;
        protected readonly bool OniOS;

        protected Func<AppQuery, AppQuery> Trait;

        protected BasePage()
        {
            app = AppInitializer.App;

            OnAndroid = app.GetType() == typeof(AndroidApp);
            OniOS = app.GetType() == typeof(iOSApp);

            InitializeQueries();
        }

        protected BasePage(Func<AppQuery, AppQuery> androidTrait, Func<AppQuery, AppQuery> iOSTrait)
            : this()
        {
            if (OnAndroid)
                Trait = androidTrait;
            if (OniOS)
                Trait = iOSTrait;

            AssertOnPage(TimeSpan.FromSeconds(30));

            app.Screenshot("On " + this.GetType().Name);
        }

        protected BasePage(string androidTrait, string iOSTrait)
            : this(x => x.Marked(androidTrait), x => x.Marked(iOSTrait))
        {
        }

        /// <summary>
        /// Verifies that the trait is still present. Defaults to no wait.
        /// </summary>
        /// <param name="timeout">Time to wait before the assertion fails</param>
        protected void AssertOnPage(TimeSpan? timeout = default(TimeSpan?))
        {
            if (Trait == null)
                throw new NullReferenceException("Trait not set");

            var message = "Unable to verify on page: " + this.GetType().Name;

            if (timeout == null)
                Assert.IsNotEmpty(app.Query(Trait), message);
            else
                Assert.DoesNotThrow(() => app.WaitForElement(Trait, timeout: timeout), message);
        }

        /// <summary>
        /// Verifies that the trait is no longer present. Defaults to a two second wait.
        /// </summary>
        /// <param name="timeout">Time to wait before the assertion fails</param>
        protected void WaitForPageToLeave(TimeSpan? timeout = default(TimeSpan?))
        {
            if (Trait == null)
                throw new NullReferenceException("Trait not set");

            timeout = timeout ?? TimeSpan.FromSeconds(2);
            var message = "Unable to verify *not* on page: " + this.GetType().Name;

            Assert.DoesNotThrow(() => app.WaitForNoElement(Trait, timeout: timeout), message);
        }

        #region CommonPageActions

        String BackButton;

        void InitializeQueries()
        {
            if (OnAndroid)
            {
            }
            if (OniOS)
            {
                BackButton = "Back";
            }
        }

        public virtual void GoBack()
        {
            if (OniOS)
                app.Tap(BackButton);
            if (OnAndroid)
            {
                app.Back();
                WaitForPageToLeave();
            }
        }

        #endregion
    }
}

