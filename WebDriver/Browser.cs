using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WebDriver
{
    public class Browser
    {
        private static IWebDriver? _driver;

        public static IWebDriver GetDriver(bool headless = false)
        {
            if (_driver == null)
            {
                var options = new ChromeOptions();
                string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                options.AddUserProfilePreference("download.default_directory", downloadPath);
                if (headless)
                {
                    options.AddArgument("--headless=new");
                    options.AddArgument("--window-size=1920,1080");
                    options.AddArgument("--disable-extensions");
                    options.AddArgument("--proxy-server='direct://'");
                    options.AddArgument("--proxy-bypass-list=*");
                }

                options.AddArguments(
                    "--start-maximized",
                    "--disable-notifications",
                    "--disable-logging",
                    "--disable-gpu",
                    "--no-sandbox",
                    "--disable-dev-shm-usage",
                    "--ignore-certificate-errors"
                );

                _driver = new ChromeDriver(options);
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
                _driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(30);
            }
            return _driver;
        }

        public static void QuitDriver()
        {
            _driver?.Quit();
            _driver = null;
        }
    }
}

