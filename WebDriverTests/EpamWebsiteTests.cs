using OpenQA.Selenium;
using WebDriver.Pages;

namespace WebDriver
{
    [TestClass]
    public class EpamWebsiteTests
    {
        private IWebDriver? _driver;
        private HomePage? _homePage;
        private CareersPage? _careersPage;
        private SearchPage? _searchPage;

        [TestInitialize]
        public void TestSetup()
        {
            _driver = Browser.GetDriver();
            _homePage = new HomePage(_driver);
            _careersPage = new CareersPage(_driver);
            _searchPage = new SearchPage(_driver);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Browser.QuitDriver();
        }

        [TestMethod]
        [DataRow("Python")]
        [DataRow("Java")]
        [DataRow("C#")]
        public void Test1_SearchJobAndValidatePosition(string programLanguage)
        {
            try
            {
                _homePage?.NavigateToHomePage();
                _homePage?.ClickCareers();

                _careersPage?.SearchJob(programLanguage);

                Assert.IsTrue(_careersPage?.IsJobFound(programLanguage),
                    $"No results found for {programLanguage}");
            }
            catch (WebDriverException ex)
            {
                Assert.Fail($"Test failed: {ex.Message}");
            }
        }

        [TestMethod]
        [DataRow("BLOCKCHAIN")]
        [DataRow("Cloud")]
        [DataRow("Automation")]
        public void Test2_ValidateGlobalSearch(string searchTerm)
        {
            try
            {
                _homePage?.NavigateToHomePage();

                _homePage?.InitiateSearch();

                _searchPage?.PerformSearch(searchTerm);

                var searchResults = _searchPage?.GetSearchResults();

                var anyResultContainsSearchTerm = searchResults?.Any(result =>
                    result.Text.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

                Assert.IsTrue(anyResultContainsSearchTerm,
                    $"No result '{searchTerm}'");
            }
            catch (WebDriverException ex)
            {
                // В случае ошибки
                Assert.Fail($"Test failed: {ex.Message}");
            }
        }
    }
}
