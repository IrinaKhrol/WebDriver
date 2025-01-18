using OpenQA.Selenium;

namespace WebDriverCore.Core.Browser
{
    public interface IBrowserFactory
    {
        IWebDriver CreateDriver(bool headless = false);
        void QuitDriver(IWebDriver driver);
    }
}