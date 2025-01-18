using OpenQA.Selenium;

namespace WebDriverCore.Business
{
    public class HomePage : BasePage
    {
        private readonly By _cookieAcceptButton = By.XPath("//*[@id=\"onetrust-accept-btn-handler\"]");
        private readonly By _careersLink = By.XPath("//a[contains(@href,'careers') and @class='top-navigation__item-link js-op']");
        private readonly By _searchButton = By.CssSelector("button.header-search__button");
        private readonly By _aboutLink = By.CssSelector("a[href*='about'].top-navigation__item-link");
        private readonly By _insightsLink = By.CssSelector("a[href*='insights'].top-navigation__item-link");

        public HomePage(IWebDriver driver) : base(driver) { }

        public void NavigateToHomePage()
        {
            Driver.Navigate().GoToUrl("https://www.epam.com/");
            AcceptCookies();
        }

        private void AcceptCookies()
        {
            try
            {
                var cookieButton = WaitForElementToBeClickable(_cookieAcceptButton);
                ClickElement(cookieButton);
            }
            catch { /* Cookie banner might not be present */ }
        }

        public void ClickCareers()
        {
            var careersLink = WaitForElementToBeClickable(_careersLink);
            ClickElement(careersLink);
            WaitForPageLoad();
        }

        public void InitiateSearch()
        {
            var searchButton = WaitForElementToBeClickable(_searchButton);
            ClickElement(searchButton);
        }

        public void ClickAbout()
        {
            var aboutLink = WaitForElementToBeClickable(_aboutLink);
            ClickElement(aboutLink);
            WaitForPageLoad();
        }

        public void ClickInsights()
        {
            var insightsLink = WaitForElementToBeClickable(_insightsLink);
            ClickElement(insightsLink);
            WaitForPageLoad();
        }
    }
}