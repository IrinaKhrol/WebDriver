using OpenQA.Selenium;
using WebDriverCore.Core.Logging;

namespace WebDriverCore.Business
{
    public class AboutPage : BasePage
    {
        private readonly By _downloadButton = By.CssSelector("div.button > div > a[data-gtm-action='click']");

        public AboutPage(IWebDriver driver) : base(driver) { }

        public void DownloadCompanyOverview()
        {
            try
            {
                var downloadButton = WaitForElementToBeClickable(_downloadButton);

                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", downloadButton);
                Thread.Sleep(1000);

                try
                {
                    downloadButton.Click();
                }
                catch
                {
                    ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", downloadButton);
                }

                Thread.Sleep(3000);
            }
            catch (Exception)
            {
                LoggerManager.LogError("Failed to download company overview");
                ScreenshotMaker.TakeScreenshot("DownloadFailed");
                throw;
            }
        }
    }
}
