using System;
using NUnit.Framework;
using Xamarin.UITest;

namespace ethanslist.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public abstract class AbstractSetup
    {
        protected IApp app;
        protected Platform platform;

        protected AbstractSetup(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        protected virtual void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        //        [TearDown]
        protected virtual void AfterEachTest()
        {
            if (TestContext.CurrentContext.Result.Status == TestStatus.Failed)
                app.Repl();
        }
    }
}

