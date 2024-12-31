using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace WebDriver.Core
{
    public class BasePage
    {
        protected IWebDriver Driver;
        protected WebDriverWait Wait;

        public BasePage(IWebDriver driver)
        {
            Driver = driver;
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        }

        protected IWebElement WaitForElement(By by)
        {
            return Wait.Until(ExpectedConditions.ElementExists(by));
        }

        protected IWebElement WaitForElementToBeClickable(By by)
        {
            return Wait.Until(ExpectedConditions.ElementToBeClickable(by));
        }

        protected void WaitForPageLoad()
        {
            Wait.Until(driver => ((IJavaScriptExecutor)driver)
                .ExecuteScript("return document.readyState").Equals("complete"));
            Thread.Sleep(1000);
        }

        protected void ScrollToElement(IWebElement element)
        {
            try
            {
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
                System.Threading.Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Scroll error: {ex.Message}");
            }
        }

        protected void ClickElement(IWebElement element)
        {
            try
            {
                ScrollToElement(element);
                element.Click();
            }
            catch
            {
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", element);
            }
        }

        protected void SendKeys(IWebElement element, string text)
        {
            ScrollToElement(element);
            element.Clear();
            element.SendKeys(text);
        }

        protected bool IsElementDisplayed(By by, int timeoutInSeconds = 10)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(ExpectedConditions.ElementIsVisible(by)).Displayed;
            }
            catch
            {
                return false;
            }
        }
    }
}