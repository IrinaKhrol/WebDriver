using OpenQA.Selenium;
using System.Collections.Generic;

namespace WebDriver
{
    public class SearchPage : BasePage
    {
        public SearchPage(IWebDriver driver) : base(driver) { }

        private IWebElement SearchInput => FindElement(By.XPath("//input[@id='new_form_search']"));
        private IWebElement FindButton => FindElement(By.PartialLinkText("Find"));
        private IList<IWebElement> SearchResults => Driver.FindElements(By.CssSelector(".search-results__item-link"));

        public void PerformSearch(string searchTerm)
        {
            SearchInput.SendKeys(searchTerm);
            FindButton.Click();
        }

        public bool AreSearchResultsPresent()
        {
            return SearchResults.Count > 0;
        }
    }
}

