using OpenQA.Selenium;
using WebDriverCore.Core.Logging;

namespace WebDriverCore.Business
{
    public class ArtificialIntelligencePage : BasePage
    {
        private readonly By _generativeAiButton = By.CssSelector(".button-in-columns .button__content--desktop");

        public ArtificialIntelligencePage(IWebDriver driver) : base(driver) { }

        public void SelectButton(string buttonName)
        {
            try
            {
                if (string.IsNullOrEmpty(buttonName))
                    throw new ArgumentException("buttonName cannot be null or empty");

                WaitForPageLoad();
                ((IJavaScriptExecutor)Driver).ExecuteScript("window.scrollBy(0, 700)");
                Thread.Sleep(2000);

                var buttons = Driver.FindElements(_generativeAiButton);
                var targetButton = buttons.FirstOrDefault(b =>
                    b.Text.Equals(buttonName, StringComparison.OrdinalIgnoreCase));

                if (targetButton == null)
                    throw new NoSuchElementException($"Button '{buttonName}' not found");

                ScrollIntoView(targetButton);
                ClickElement(targetButton);
                WaitForPageLoad();
                LoggerManager.LogInfo($"Selected button: {buttonName}");
            }
            catch (Exception ex)
            {
                LoggerManager.LogError($"Failed to select button {buttonName}: {ex.Message}");
                throw;
            }
        }

        private void ScrollIntoView(IWebElement element)
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
            Thread.Sleep(1000);
        }

        public bool ValidateTitle(string expectedTitle)
        {
            try
            {
                WaitForPageLoad();
                var title = Driver.Title;
                LoggerManager.LogInfo($"Page title: {title}");
                return title.Contains(expectedTitle, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                LoggerManager.LogError($"Title validation failed: {ex.Message}");
                return false;
            }
        }
    }
}

