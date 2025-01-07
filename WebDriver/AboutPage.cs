using OpenQA.Selenium;
using WebDriver.Core;

namespace WebDriverCore
{
    public class AboutPage : BasePage
    {
        private readonly By _downloadButton = By.CssSelector("div.button > div > a[data-gtm-action='click']");

        public AboutPage(IWebDriver driver) : base(driver) { }

        public void DownloadCompanyOverview()
        {
            var downloadButton = WaitForElementToBeClickable(_downloadButton);
            ScrollUsingWheel(downloadButton);
            ClickElement(downloadButton);
            Thread.Sleep(3000);
        }
    }
}
