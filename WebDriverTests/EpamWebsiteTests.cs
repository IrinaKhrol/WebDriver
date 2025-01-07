using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using WebDriver.Pages;
using WebDriverCore;

namespace WebDriver
{
    [TestClass]
    public class EpamWebsiteTests
    {
        private IWebDriver? _driver;
        private HomePage? _homePage;
        private CareersPage? _careersPage;
        private SearchPage? _searchPage;
        private AboutPage? _aboutPage;
        private InsightsPage? _insightsPage;

        [TestInitialize]
        public void TestSetup()
        {
            _driver = Browser.GetDriver();
            _homePage = new HomePage(_driver);
            _careersPage = new CareersPage(_driver);
            _searchPage = new SearchPage(_driver);
            _aboutPage = new AboutPage(_driver);
            _insightsPage = new InsightsPage(_driver);
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
                Assert.Fail($"Test failed: {ex.Message}");
            }
        }

        [TestMethod]
        [DataRow("EPAM_Corporate_Overview_Q4_EOY.pdf")]
        public void Test3_ValidateFileDownload(string fileName)
        {
            try
            {
                string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

                Console.WriteLine($"Looking for files in: {downloadPath}");

                string pattern = "EPAM_Corporate_Overview_Q4_EOY*.pdf";

                var files = Directory.GetFiles(downloadPath, pattern);
                foreach (var file in files)
                {
                    File.Delete(file);
                }

                string filePath = Path.Combine(downloadPath, fileName);

                _homePage?.NavigateToHomePage();
                _homePage?.ClickAbout();
                _aboutPage?.DownloadCompanyOverview();

                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));

                wait.Until(driver =>
                {
                    var downloadedFiles = Directory.GetFiles(downloadPath, pattern);
                    return downloadedFiles.Length > 0;
                });

                var downloadedFile = Directory.GetFiles(downloadPath, pattern)
                                               .FirstOrDefault(file => file.StartsWith(filePath.Substring(0, filePath.LastIndexOf('.'))));

                bool isFileDownloaded = downloadedFile != null;
                Assert.IsTrue(isFileDownloaded, $"File {fileName} was not downloaded");

            }
            catch (WebDriverException ex)
            {
                Assert.Fail($"Test failed: {ex.Message}");
            }
        }

        [TestMethod]
        public void Test4_ValidateArticleTitle()
        {
            try
            {
                _homePage?.NavigateToHomePage();
                _homePage?.ClickInsights();

                _insightsPage?.SwipeCarouselTwice();
                string carouselTitle = _insightsPage?.GetRememberedTitle() ?? string.Empty;

                Console.WriteLine($"Remembered title: {carouselTitle}"); // для отладки

                _insightsPage?.ClickReadMore();
                string articleTitle = _insightsPage?.GetArticleTitle() ?? string.Empty;

                Assert.AreEqual(carouselTitle, articleTitle,
                    "Article title does not match with carousel title");
            }
            catch (WebDriverException ex)
            {
                Assert.Fail($"Test failed: {ex.Message}");
            }
        }
    }
}
