using OpenQA.Selenium.Support.UI;
using WebDriverCore.Business;
using WebDriverCore.Core.Logging;
using WebDriverTests.Tests.Base;

namespace WebDriverTests.Tests.UI
{
    [TestClass]
    public class EpamWebsiteTests : BaseTest
    {
        private HomePage? _homePage;
        private CareersPage? _careersPage;
        private SearchPage? _searchPage;
        private AboutPage? _aboutPage;
        private InsightsPage? _insightsPage;

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            _homePage = new HomePage(Driver!);
            _careersPage = new CareersPage(Driver!);
            _searchPage = new SearchPage(Driver!);
            _aboutPage = new AboutPage(Driver!);
            _insightsPage = new InsightsPage(Driver!);
            LoggerManager.LogInfo("Test pages initialized");
        }

        [TestMethod]
        [DataRow("Python")]
        [DataRow("Java")]
        [DataRow("C#")]
        public void Test1_SearchJobAndValidatePosition(string programLanguage)
        {
            try
            {
                LoggerManager.LogInfo($"Starting job search test for {programLanguage}");
                _homePage?.NavigateToHomePage();
                _homePage?.ClickCareers();
                _careersPage?.SearchJob(programLanguage);

                Assert.IsTrue(_careersPage?.IsJobFound(programLanguage),
                    $"No results found for {programLanguage}");
                LoggerManager.LogInfo($"Job search test completed successfully for {programLanguage}");
            }
            catch (Exception ex)
            {
                LoggerManager.LogError($"Test failed: {ex.Message}");
                throw;
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
                LoggerManager.LogInfo($"Starting global search test for {searchTerm}");
                _homePage?.NavigateToHomePage();
                _homePage?.InitiateSearch();
                _searchPage?.PerformSearch(searchTerm);

                var searchResults = _searchPage?.GetSearchResults();
                var anyResultContainsSearchTerm = searchResults?.Any(result =>
                    result.Text.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

                Assert.IsTrue(anyResultContainsSearchTerm,
                    $"No result contains '{searchTerm}'");
                LoggerManager.LogInfo($"Global search test completed successfully for {searchTerm}");
            }
            catch (Exception ex)
            {
                LoggerManager.LogError($"Test failed: {ex.Message}");
                throw;
            }
        }

        [TestMethod]
        [DataRow("EPAM_Corporate_Overview_Q4_EOY.pdf")]
        public void Test3_ValidateFileDownload(string fileName)
        {
            try
            {
                LoggerManager.LogInfo($"Starting file download test for {fileName}");
                string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                LoggerManager.LogInfo($"Download path: {downloadPath}");

                string pattern = "EPAM_Corporate_Overview_Q4_EOY*.pdf";
                var files = Directory.GetFiles(downloadPath, pattern);
                foreach (var file in files)
                {
                    File.Delete(file);
                    LoggerManager.LogInfo($"Deleted existing file: {file}");
                }

                string filePath = Path.Combine(downloadPath, fileName);
                _homePage?.NavigateToHomePage();
                _homePage?.ClickAbout();
                _aboutPage?.DownloadCompanyOverview();

                var wait = new WebDriverWait(Driver!, TimeSpan.FromSeconds(30));
                wait.Until(driver =>
                {
                    var downloadedFiles = Directory.GetFiles(downloadPath, pattern);
                    return downloadedFiles.Length > 0;
                });

                var downloadedFile = Directory.GetFiles(downloadPath, pattern)
                    .FirstOrDefault(file => file.StartsWith(filePath.Substring(0, filePath.LastIndexOf('.'))));

                bool isFileDownloaded = downloadedFile != null;
                Assert.IsTrue(isFileDownloaded, $"File {fileName} was not downloaded");
                LoggerManager.LogInfo("File download test completed successfully");
            }
            catch (Exception ex)
            {
                LoggerManager.LogError($"Test failed: {ex.Message}");
                throw;
            }
        }

        [TestMethod]
        public void Test4_ValidateArticleTitle()
        {
            try
            {
                LoggerManager.LogInfo("Starting article title validation test");
                _homePage?.NavigateToHomePage();
                _homePage?.ClickInsights();

                _insightsPage?.SwipeCarouselTwice();
                string carouselTitle = _insightsPage?.GetRememberedTitle() ?? string.Empty;
                LoggerManager.LogInfo($"Carousel title: {carouselTitle}");

                _insightsPage?.ClickReadMore();
                string articleTitle = _insightsPage?.GetArticleTitle() ?? string.Empty;
                LoggerManager.LogInfo($"Article title: {articleTitle}");

                Assert.AreEqual(carouselTitle, articleTitle,
                    "Article title does not match with carousel title");
                LoggerManager.LogInfo("Article title validation test completed successfully");
            }
            catch (Exception ex)
            {
                LoggerManager.LogError($"Test failed: {ex.Message}");
                throw;
            }
        }
    }
}
