using OpenQA.Selenium;
using WebDriverCore.Core.Browser;
using WebDriverCore.Core.Logging;

namespace WebDriver
{
    public class Browser
    {
        private static IWebDriver? _driver;
        private static readonly IBrowserFactory _browserFactory = BrowserFactory.Instance;

        public static IWebDriver GetDriver(bool headless = false)
        {
            if (_driver == null)
            {
                try
                {
                    LoggerManager.LogInfo("Initializing browser");
                    _driver = _browserFactory.CreateDriver(headless);
                }
                catch (Exception ex)
                {
                    LoggerManager.LogError("Failed to initialize browser", ex);
                    throw;
                }
            }
            return _driver;
        }

        public static void QuitDriver()
        {
            if (_driver != null)
            {
                LoggerManager.LogInfo("Quitting browser");
                _browserFactory.QuitDriver(_driver);
                _driver = null;
            }
        }
    }
}

