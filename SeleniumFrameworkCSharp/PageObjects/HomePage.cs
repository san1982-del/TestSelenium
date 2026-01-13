using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumFrameworkCSharp.BaseTest;
using SeleniumFrameworkCSharp.Utils;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFrameworkCSharp.PageObjects
{
    internal class HomePage:BasePage
    {
        // Page Factory Elements
        [FindsBy(How = How.XPath, Using = "//div[normalize-space()='NEW CARS']")]
        private IWebElement newCarMenu;

        [FindsBy(How = How.XPath, Using = "//div[text()='Find New Cars']")]
        private IWebElement findNewCarLink;

        [FindsBy(How = How.XPath, Using = "//div[text()='USED CARS']")]
        private IWebElement usedCarsMenu;

        [FindsBy(How = How.XPath, Using = "//div[contains(text(),'Explore Used Cars')]")]
        private IWebElement exploreUsedCarsLink;

        [FindsBy(How = How.XPath, Using = "//input[contains(@class,'OMyJgF ayFmbh R67Xgy b36DQo CXPRav MdoJAO jCabaD Cxdcp3')]")]
        private IWebElement searchBox;

        [FindsBy(How = How.XPath, Using = "//div/div/ul/li[1]/ul/li")]
        private IList<IWebElement> searchDropdownMenuItems;

        [FindsBy(How = How.XPath, Using = "//div[@class='o-ex o-fJ o-bN o-cV o-f']//*[name()='svg']")]
        private IWebElement locationIcon;

        [FindsBy(How = How.XPath, Using = "//img[@title='Mumbai']")]
        private IWebElement mumbaiCityOption;

        [FindsBy(How = How.XPath, Using = "//div/div[1]/div/div/span[3]")]
        private IWebElement usedSearchTab;

        // Constructor
        public HomePage(IWebDriver driver) : base(driver)
        {
        }

        // Fluent method - Returns NewCarPage
        public NewCarPage FindNewCar()
        {
            var actions = new Actions(driver);

            WaitForElementToBeVisible(newCarMenu);
            actions.MoveToElement(newCarMenu).Perform();

            WaitForElementToBeClickable(findNewCarLink);
            findNewCarLink.Click();

            baseTest.GetExtentTest()?.Info("Navigated to New Cars page");
            baseTest.log.Info("New Cars page navigation completed");

            return new NewCarPage(driver);
        }

        // Fluent method - Returns UsedCarPage
        public UsedCarPage ExploreUsedCar()
        {
            var actions = new Actions(driver);

            WaitForElementToBeVisible(usedCarsMenu);
            actions.MoveToElement(usedCarsMenu).Perform();

            WaitForElementToBeClickable(exploreUsedCarsLink);
            exploreUsedCarsLink.Click();

            baseTest.GetExtentTest()?.Info("Navigated to Used Cars page");
            baseTest.log.Info("Used Cars page is launched");

            return new UsedCarPage(driver);
        }

        // Search functionality with improved wait strategy
        public HomePage SearchForCar(string searchTerm)
        {
            WaitForElementToBeVisible(searchBox);
            searchBox.Clear();
            searchBox.SendKeys(searchTerm);

            baseTest.GetExtentTest()?.Info($"Entered search term: {searchTerm}");

            return this; // Return this for method chaining
        }

        // Select from dropdown with intelligent wait
        public HomePage SelectFromDropdown(string itemText)
        {
            // Wait for dropdown items to be visible
            waitHelper.WaitForElements(By.XPath("//div/div/ul/li[1]/ul/li"));

            foreach (var item in searchDropdownMenuItems)
            {
                if (item.GetAttribute("data-label").Contains(itemText))
                {
                    WaitForElementToBeClickable(item);
                    item.Click();

                    baseTest.GetExtentTest()?.Info($"Selected dropdown item: {itemText}");
                    break;
                }
            }

            return this; // Return this for method chaining
        }

        // Select city location
        public HomePage SelectCity(string cityName)
        {
            WaitForElementToBeClickable(locationIcon);
            locationIcon.Click();

            // Wait for Mumbai option to appear
            WaitForElementToBeVisible(mumbaiCityOption);
            mumbaiCityOption.Click();

            baseTest.GetExtentTest()?.Info($"Selected city: {cityName}");

            return this; // Return this for method chaining
        }

        // Switch to used car search
        public HomePage SwitchToUsedCarSearch()
        {
            WaitForElementToBeClickable(usedSearchTab);
            usedSearchTab.Click();

            baseTest.GetExtentTest()?.Info("Switched to used car search");

            return this; // Return this for method chaining
        }

        // Data provider method
        public static IEnumerable<TestCaseData> GetTestData()
        {
            var columns = new List<string> { "browser", "runmode", "carbrand", "cartitle", "carname" };
            return DataUtil.GetTestDataFromExcel(
                Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\resources\\testdata.xlsx",
                "FindNewCar",
                columns);
        }
    }
}
