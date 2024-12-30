using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace WebDriver
{
    public class HomePage : BasePage
    {
        public HomePage(IWebDriver driver) : base(driver)
        {
        }

        private IWebElement CareersLink => FindElement(By.XPath("/ html / body / div / div[2] / div[2] / div[1] / header / div / div / nav / ul / li[5] / span[1] / a"));
        private IWebElement SearchButton => FindElement(By.CssSelector("button.search-icon"));

        public void ClickCareers()
        {
            CareersLink.Click();
        }

        public void ClickSearch()
        {
            SearchButton.Click();
        }
    }
}

