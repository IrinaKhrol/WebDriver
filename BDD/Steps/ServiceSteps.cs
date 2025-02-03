using TechTalk.SpecFlow;
using WebDriverCore.Business;
using WebDriver;
using OpenQA.Selenium;
using NUnit.Framework;
using WebDriverCore.Core.Logging;

namespace WebDriverTests.Steps
{
    [Binding]
    public class ServiceSteps
    {
        private IWebDriver? _driver;
        private HomePage? _homePage;
        private ServicesPage? _servicesPage;
        private ArtificialIntelligencePage? _aiPage;

        [Given(@"I am on EPAM homepage")]
        public void GivenIAmOnEPAMHomepage()
        {
            _driver = Browser.GetDriver();
            _homePage = new HomePage(_driver);
            _servicesPage = new ServicesPage(_driver);
            _aiPage = new ArtificialIntelligencePage(_driver);
            _homePage.NavigateToHomePage();
        }

        [When(@"I click on Services in the main navigation menu")]
        public void WhenIClickOnServicesInTheMainNavigationMenu()
        {
            _homePage.ClickServices();
        }

        [When(@"I select service category '(.*)'")]
        public void WhenISelectServiceCategory(string category)
        {
            _servicesPage.SelectCategory(category);
        }


        [When(@"I click on the '(.*)' button on the '(.*)' page")]
        public void WhenIClickOnTheButtonOnThePage(string buttonName, string category)
        {
            LoggerManager.LogInfo($"Clicking {buttonName} button on {category} page");
            _aiPage.SelectButton(buttonName);
        }

        [Then(@"I should see the correct title for '(.*)'")]
        public void ThenIShouldSeeTheCorrectTitleFor(string buttonName)
        {
            Assert.That(_aiPage.ValidateTitle(buttonName), Is.True);
        }

        [Then(@"the section Our Related Expertise should be displayed on the page")]
        public void ThenTheSectionOurRelatedExpertiseShouldBeDisplayed()
        {
            Assert.That(_servicesPage.IsRelatedExpertiseSectionDisplayed(), Is.True);
        }

        [AfterScenario]
        public void CleanUp()
        {
            Browser.QuitDriver();
        }
    }
}
