using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverCore.Core.Logging;

namespace WebDriverCore.Core.Browser
{
    public class BrowserFactory : IBrowserFactory
    {
        private static readonly Lazy<BrowserFactory> _instance =
            new Lazy<BrowserFactory>(() => new BrowserFactory());

        private BrowserFactory() { }

        public static BrowserFactory Instance => _instance.Value;

        public IWebDriver CreateDriver(bool headless = false)
        {
            LoggerManager.LogInfo("Creating Chrome driver instance");
            try
            {
                var options = new ChromeOptions();
                string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                options.AddUserProfilePreference("download.default_directory", downloadPath);

                if (headless)
                {
                    options.AddArgument("--headless=new");
                    options.AddArgument("--window-size=1920,1080");
                }

                options.AddArguments(
                    "--start-maximized",
                    "--disable-notifications",
                    "--disable-logging",
                    "--disable-gpu",
                    "--no-sandbox",
                    "--disable-dev-shm-usage"
                );

                var driver = new ChromeDriver(options);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);

                LoggerManager.LogInfo("Chrome driver created successfully");
                return driver;
            }
            catch (Exception ex)
            {
                LoggerManager.LogError($"Failed to create driver: {ex.Message}");
                throw;
            }
        }

        public void QuitDriver(IWebDriver driver)
        {
            if (driver != null)
            {
                LoggerManager.LogInfo("Quitting driver");
                driver.Quit();
            }
        }
    }
}
