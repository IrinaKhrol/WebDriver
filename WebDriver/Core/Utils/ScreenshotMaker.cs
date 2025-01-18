using OpenQA.Selenium;
using WebDriverCore.Core.Logging;

namespace WebDriverCore.Core.Utils
{
    public class ScreenshotMaker
    {
        private readonly string _screenshotPath;
        private readonly IWebDriver _driver;

        public ScreenshotMaker(IWebDriver driver)
        {
            _driver = driver;
            _screenshotPath = CreateScreenshotFolder();
        }

        private string CreateScreenshotFolder()
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }

        public string TakeScreenshot(string testName)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string screenshotName = $"{testName}_{timestamp}.png";
                string fullPath = Path.Combine(_screenshotPath, screenshotName);

                Screenshot screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                screenshot.SaveAsFile(fullPath);

                LoggerManager.LogInfo($"Screenshot captured and saved to: {fullPath}");
                return fullPath;
            }
            catch (Exception ex)
            {
                LoggerManager.LogError($"Failed to capture screenshot for test '{testName}'", ex);
                return string.Empty;
            }
        }
    }
}
