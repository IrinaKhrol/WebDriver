using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace WebDriver
{
    public static class Browser
    {
        private static IWebDriver _driver;

        public static IWebDriver Driver
        {
            get
            {
                if (_driver == null)
                {
                    Initialize();
                }
                return _driver;
            }
        }

        public static void Initialize()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            _driver = new ChromeDriver(options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

        }

        public static void Quit()
        {
            _driver?.Quit();
            _driver = null;
        }
    }
}

