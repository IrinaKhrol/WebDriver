using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace WebDriver
{
    public class BasePage
    {
        protected IWebDriver Driver;
        protected WebDriverWait Wait;

        public BasePage(IWebDriver driver)
        {
            Driver = driver;
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        // Переход на главную страницу
        public void NavigateToHomePage()
        {
            Driver.Navigate().GoToUrl("https://www.epam.com/");

            // Ожидание и клик по кнопке согласия с cookies
            WaitForElementToBeClickable(By.CssSelector("button.accept-cookies")).Click();
        }

        // Метод для поиска элемента с ожиданием
        protected IWebElement FindElement(By locator)
        {
            return Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
        }

        // Метод для кликабельности элемента
        protected IWebElement WaitForElementToBeClickable(By locator)
        {
            return Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
        }
    }
}


