using OpenQA.Selenium;

namespace WebDriverCore.Business
{
    public class CareersPage : BasePage
    {
        public CareersPage(IWebDriver driver) : base(driver) { }

        private readonly By _keywordInput = By.XPath("//*[@id=\"new_form_job_search-keyword\"]");
        private readonly By _findButton = By.XPath("//*[@id=\"jobSearchFilterForm\"]/button");

        public void SearchJob(string keyword)
        {
            try
            {
                WaitForPageLoad();

                var keywordField = WaitForElementToBeClickable(_keywordInput);
                keywordField.Click();
                keywordField.Clear();
                keywordField.SendKeys(keyword);

                var submitButton = WaitForElementToBeClickable(_findButton);
                ClickElement(submitButton);
                WaitForPageLoad();
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException($"Failed to perform job search: {ex.Message}");
            }
        }

        public bool IsJobFound(string keyword)
        {
            try
            {
                return Driver.PageSource.ToLower().Contains(keyword.ToLower());
            }
            catch
            {
                return false;
            }
        }
    }
}
