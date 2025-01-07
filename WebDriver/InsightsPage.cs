using OpenQA.Selenium;
using WebDriver.Core;

namespace WebDriverCore
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
                    ClickElement(nextButton);
                    Thread.Sleep(1000);
                }
                _rememberedTitle = GetCarouselArticleTitle();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during carousel swipe: {ex.Message}");
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
            var readMoreButton = Wait.Until(driver =>
            {
                var button = driver.FindElement(_currentSlideReadMoreButton);
                return button.Displayed && button.Enabled ? button : null;
            });
            ClickElement(readMoreButton);
            WaitForPageLoad();
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