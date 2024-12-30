using OpenQA.Selenium;
using WebDriver;

namespace WebDriverTests
{
    [TestClass]
    public class BaseTest
    {
        protected IWebDriver Driver;
        protected HomePage HomePage;
        protected CareersPage CareersPage;
        protected SearchPage SearchPage;

        [TestInitialize]
        public void TestInitialize()
        {
            Browser.Initialize();
            Driver = Browser.Driver;

            HomePage = new HomePage(Driver);
            CareersPage = new CareersPage(Driver);
            SearchPage = new SearchPage(Driver);

            Driver.Navigate().GoToUrl("https://www.epam.com/");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Browser.Quit();
        }
    }
}

