using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WebDriver
{
    public class Browser
    {
        private static IWebDriver? _driver;

        public static IWebDriver GetDriver()
        {
            if (_driver == null)
            {
                var options = new ChromeOptions();
                options.AddArguments(
                    "--start-maximized",
                    "--disable-notifications",
                    "--disable-logging",
                    "--disable-gpu",
                    "--no-sandbox",
                    "--disable-dev-shm-usage"
                );

                _driver = new ChromeDriver(options);
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
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

