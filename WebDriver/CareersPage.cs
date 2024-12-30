using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace WebDriver
{
    public class CareersPage : BasePage
    {
        public CareersPage(IWebDriver driver) : base(driver) { }

        // Локаторы
        private IWebElement KeywordInput => FindElement(By.CssSelector("input[name='new_form_job_search-keyword']"));
        private IWebElement LocationDropdown => FindElement(By.Id("select2-location-container"));
        private IWebElement AllLocationsOption => FindElement(By.XPath("//ul[@id='select2-location-results']/li[text()='All Locations']"));
        private IWebElement RemoteCheckbox => FindElement(By.XPath("//label[@class='remote-checkbox']"));
        private IWebElement FindButton => FindElement(By.CssSelector("button[type='submit']"));

        // Метод для прокрутки до элемента
        private void ScrollToElement(IWebElement element)
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
                // Сначала прокручиваем немного вниз
                js.ExecuteScript("window.scrollBy(0, 300);");
                System.Threading.Thread.Sleep(500);
                // Затем прокручиваем к элементу
                js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
                System.Threading.Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to scroll to element: {ex.Message}");
            }
        }

        public void SearchJob(string keyword, bool isRemote = false)
        {
            try
            {
                // Ждем загрузки страницы
                Wait.Until(driver => ((IJavaScriptExecutor)driver)
                    .ExecuteScript("return document.readyState").Equals("complete"));

                // Прокручиваем к форме поиска
                ScrollToElement(KeywordInput);

                // Вводим ключевое слово
                KeywordInput.Clear();
                KeywordInput.SendKeys(keyword);

                // Выбираем локацию
                LocationDropdown.Click();
                Wait.Until(ExpectedConditions.ElementToBeClickable(AllLocationsOption));
                AllLocationsOption.Click();

                if (isRemote)
                {
                    ScrollToElement(RemoteCheckbox);
                    RemoteCheckbox.Click();
                }

                FindButton.Click();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to perform job search: {ex.Message}");
            }
        }

        // Метод для проверки результатов поиска
        public bool IsJobFound(string keyword)
        {
            try
            {
                return FindElement(By.XPath($"//div[contains(text(), '{keyword}')]"))
                    .Displayed;
            }
            catch
            {
                return false;
            }
        }
    }
}


