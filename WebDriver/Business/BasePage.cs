using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using WebDriverCore.Core.Logging;
using WebDriverCore.Core.Utils;

namespace WebDriverCore.Business
{
    public class BasePage
    {
        protected IWebDriver Driver;
        protected WebDriverWait Wait;
        protected ScreenshotMaker ScreenshotMaker;

        protected BasePage(IWebDriver driver)
        {
            Driver = driver;
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            ScreenshotMaker = new ScreenshotMaker(driver);
        }

        protected IWebElement WaitForElement(By by)
        {
            try
            {
                LoggerManager.LogInfo($"Waiting for element: {by}");
                return Wait.Until(ExpectedConditions.ElementExists(by));
            }
            catch (WebDriverTimeoutException)
            {
                LoggerManager.LogError($"Element not found: {by}");
                ScreenshotMaker.TakeScreenshot("ElementNotFound");
                throw;
            }
        }

        protected IWebElement WaitForElementToBeClickable(By by)
        {
            try
            {
                LoggerManager.LogInfo($"Waiting for element to be clickable: {by}");
                return Wait.Until(ExpectedConditions.ElementToBeClickable(by));
            }
            catch (WebDriverTimeoutException)
            {
                LoggerManager.LogError($"Element not clickable: {by}");
                ScreenshotMaker.TakeScreenshot("ElementNotClickable");
                throw;
            }
        }

        protected void WaitForPageLoad()
        {
            try
            {
                LoggerManager.LogInfo("Waiting for page to load");
                Wait.Until(driver => ((IJavaScriptExecutor)driver)
                    .ExecuteScript("return document.readyState").Equals("complete"));
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                LoggerManager.LogError("Page load timeout");
                ScreenshotMaker.TakeScreenshot("PageLoadTimeout");
                throw;
            }
        }

        protected void ClickElement(IWebElement element)
        {
            try
            {
                LoggerManager.LogInfo("Clicking element");
                ScrollToElement(element);
                element.Click();
            }
            catch (Exception)
            {
                LoggerManager.LogError("Failed to click element");
                ScreenshotMaker.TakeScreenshot("ClickFailed");
                throw;
            }
        }

        protected void ScrollToElement(IWebElement element)
        {
            try
            {
                LoggerManager.LogInfo("Scrolling to element");
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
                Thread.Sleep(500);
            }
            catch (Exception)
            {
                LoggerManager.LogError("Failed to scroll to element");
                ScreenshotMaker.TakeScreenshot("ScrollFailed");
                throw;
            }
        }

        protected void SendKeys(IWebElement element, string text)
        {
            try
            {
                LoggerManager.LogInfo($"Sending keys: {text}");
                ScrollToElement(element);
                element.Clear();
                element.SendKeys(text);
            }
            catch (Exception)
            {
                LoggerManager.LogError($"Failed to send keys: {text}");
                ScreenshotMaker.TakeScreenshot("SendKeysFailed");
                throw;
            }
        }
        protected bool IsElementDisplayed(By by, int timeoutInSeconds = 10)
        {
            try
            {
                LoggerManager.LogInfo($"Checking if element is displayed: {by}");
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(ExpectedConditions.ElementIsVisible(by)).Displayed;
            }
            catch
            {
                LoggerManager.LogInfo($"Element is not displayed: {by}");
                return false;
            }
        }

        protected void ScrollUsingWheel(IWebElement element)
        {
            try
            {
                LoggerManager.LogInfo("Scrolling using wheel to element");
                Actions actions = new Actions(Driver);
                actions.MoveToElement(element)
                       .SendKeys(Keys.PageDown)
                       .Build()
                       .Perform();
                Thread.Sleep(1000);
            }
            catch (Exception)
            {
                LoggerManager.LogError("Failed to scroll using wheel to element");
                ScreenshotMaker.TakeScreenshot("ScrollUsingWheelFailed");
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            }
        }
    }
}