using OpenQA.Selenium;

namespace WebDriverCore.Business
{
    public class SearchPage : BasePage
    {
        private readonly By _searchInput = By.CssSelector(".header-search__input");
        private readonly By _findButton = By.CssSelector("button.custom-search-button");
        private readonly By _searchResults = By.CssSelector(".search-results__title-link");

        public SearchPage(IWebDriver driver) : base(driver) { }

        public void PerformSearch(string searchTerm)
        {
            var searchField = WaitForElement(_searchInput);
            SendKeys(searchField, searchTerm);

            var searchButton = WaitForElement(_findButton);
            ClickElement(searchButton);
            WaitForPageLoad();
        }

        public bool AreSearchResultsPresent()
        {
            return IsElementDisplayed(_searchResults);
        }

        public bool ValidateSearchResults(string searchTerm)
        {
            var results = Driver.FindElements(_searchResults)
                .Select(e => e.Text.ToUpper())
                .ToList();

            return results.Any() && results.All(text => text.Contains(searchTerm.ToUpper()));
        }
        public IList<IWebElement> GetSearchResults()
        {
            var results = Driver.FindElements(_searchResults);
            return results.ToList();
        }
    }
}

