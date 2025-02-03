using OpenQA.Selenium;
using WebDriverCore.Core.Logging;

namespace WebDriverCore.Business
{
    public class ServicesPage : BasePage
    {
        private readonly By _expertiseSection = By.CssSelector(".related-expertise");

        public ServicesPage(IWebDriver driver) : base(driver) { }

        public void SelectCategory(string category)
        {
            try
            {
                if (string.IsNullOrEmpty(category))
                    throw new ArgumentException("Category cannot be null or empty");

                WaitForPageLoad();
                ((IJavaScriptExecutor)Driver).ExecuteScript("window.scrollBy(0, 700)");
                Thread.Sleep(2000);

                By buttonSelector = By.CssSelector(".button-in-columns .button__content--desktop");
                var buttons = Driver.FindElements(buttonSelector);

                var targetButton = buttons.FirstOrDefault(b => b.Text.Equals(category, StringComparison.OrdinalIgnoreCase));

                if (targetButton == null)
                    throw new NoSuchElementException($"Button with text '{category}' not found");

                ScrollIntoView(targetButton);
                ClickElement(targetButton);
                WaitForPageLoad();
                LoggerManager.LogInfo($"Selected category: {category}");
            }
            catch (Exception ex)
            {
                LoggerManager.LogError($"Failed to select {category}: {ex.Message}");
                throw;
            }
        }

        private void ScrollIntoView(IWebElement element)
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
            Thread.Sleep(1000);
        }

        public bool IsRelatedExpertiseSectionDisplayed()
        {
            try
            {
                return IsElementDisplayed(_expertiseSection);
            }
            catch (Exception ex)
            {
                LoggerManager.LogError($"Related expertise check failed: {ex.Message}");
                return false;
            }
        }
    }
}

