using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Drawing;
using WebDriverCore.Core.Logging;

namespace WebDriverCore.Business
{
    public class InsightsPage : BasePage
    {
        private readonly By _carouselArticleTitle = By.CssSelector(".owl-item.active .single-slide__content-container p span span:nth-child(1), .owl-item.active .single-slide__content-container p span span.rte-text-gradient span");
        private readonly By _currentSlideReadMoreButton = By.CssSelector(".owl-item.active .single-slide__cta-container a");
        private readonly By _nextButton = By.CssSelector("button.slider__right-arrow.slider-navigation-arrow");
        private readonly By _articleTitle = By.CssSelector(".colctrl__col--width-70 p span span");
        private string _rememberedTitle = string.Empty;

        public InsightsPage(IWebDriver driver) : base(driver) { }

        public void SwipeCarouselTwice()
        {
            try
            {
                for (int i = 0; i < 2; i++)
                {
                    var nextButton = WaitForElementToBeClickable(_nextButton);

                    ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", nextButton);
                    Thread.Sleep(1000);
                    try
                    {
                        nextButton.Click();
                    }
                    catch
                    {
                        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", nextButton);
                    }

                    Thread.Sleep(1000);
                }
                _rememberedTitle = GetCarouselArticleTitle();
            }
            catch (Exception)
            {
                LoggerManager.LogError("Error during carousel swipe");
                ScreenshotMaker.TakeScreenshot("CarouselSwipeFailed");
                throw;
            }
        }


        public string GetCarouselArticleTitle()
        {
            var titleElements = Driver.FindElements(_carouselArticleTitle);
            string fullTitle = string.Join(" ", titleElements.Select(e => e.Text.Trim()));
            int index = fullTitle.IndexOf("01 01");
            string title = index > 0 ? fullTitle.Substring(0, index).Trim() : fullTitle.Trim();

            Console.WriteLine($"Carousel title: {title}");
            return title;
        }

        public void ClickReadMore()
        {
            try
            {
                var readMoreButton = WaitForElementToBeClickable(_currentSlideReadMoreButton);
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", readMoreButton);
                Thread.Sleep(1000);
                try
                {
                    var hamburgerMenu = Driver.FindElement(By.CssSelector(".hamburger-menu-ui"));
                    if (hamburgerMenu.Displayed)
                    {
                        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].style.display='none'", hamburgerMenu);
                    }
                }
                catch {/* ignore if the menu is not found */}

                try
                {
                    readMoreButton.Click();
                }
                catch
                {
                    ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", readMoreButton);
                }

                WaitForPageLoad();
            }
            catch (Exception)
            {
                LoggerManager.LogError("Failed to click Read More button");
                ScreenshotMaker.TakeScreenshot("ReadMoreClickFailed");
                throw;
            }
        }

        public string GetArticleTitle()
        {
            var titleElement = WaitForElement(_articleTitle);
            return titleElement.Text.Trim();
        }

        public string GetRememberedTitle()
        {
            return _rememberedTitle;
        }
    }
}