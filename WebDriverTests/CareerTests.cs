namespace WebDriver.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using WebDriverTests;

    [TestClass]
    public class CareerTests : BaseTest
    {
        [TestMethod]
        [DataRow("Java Developer", true)]
        [DataRow("Python Developer", false)]
        public void SearchJobTest(string position, bool isRemote)
        {
            HomePage.ClickCareers();
            CareersPage.SearchJob(position, isRemote);
            // Add assertions here
        }
    }
}
