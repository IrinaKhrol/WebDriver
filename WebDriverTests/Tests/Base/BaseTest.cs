using OpenQA.Selenium;
using WebDriver;
using WebDriverCore.Core.Logging;
using WebDriverCore.Core.Utils;

namespace WebDriverTests.Tests.Base
{
    [TestClass]
    public abstract class BaseTest
    {
        protected IWebDriver? Driver;
        protected TestContext? TestContext;

        public TestContext? CurrentContext
        {
            get { return TestContext; }
            set { TestContext = value; }
        }

        [TestInitialize]
        public virtual void TestInitialize()
        {
            try
            {
                Driver = Browser.GetDriver();
                LoggerManager.LogInfo($"Starting test: {TestContext?.TestName}");
            }
            catch (Exception ex)
            {
                LoggerManager.LogError($"Failed to initialize test: {ex.Message}");
                throw;
            }
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            try
            {
                if (TestContext?.CurrentTestOutcome != UnitTestOutcome.Passed)
                {
                    var screenshotMaker = new ScreenshotMaker(Driver!);
                    screenshotMaker.TakeScreenshot(TestContext?.TestName ?? "UnknownTest");
                }

                LoggerManager.LogInfo($"Finishing test: {TestContext?.TestName}");
                Browser.QuitDriver();
            }
            catch (Exception ex)
            {
                LoggerManager.LogError($"Error in test cleanup: {ex.Message}");
                throw;
            }
        }
    }
}
