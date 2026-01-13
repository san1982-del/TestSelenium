using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumFrameworkCSharp.Utils;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFrameworkCSharp.PageObjects
{
    internal class BasePage
    {
        public IWebDriver driver;
        public static KeywordDriven keyword;
        public static CarBase carBase;
        protected WebDriverWait wait;
        protected WaitHelper waitHelper;
        public static DBManager dbManager;

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
            keyword = new KeywordDriven();
            carBase = new CarBase(driver);
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            this.waitHelper = new WaitHelper(driver);
            dbManager = new DBManager();

            // Initialize Page Factory elements
            PageFactory.InitElements(driver, this);
        }
        // Fluent wait for element to be clickable
        protected IWebElement WaitForElementToBeClickable(IWebElement element)
        {
            return waitHelper.WaitForElementToBeClickable(element);
        }

        // Fluent wait for element to be visible
        protected IWebElement WaitForElementToBeVisible(IWebElement element)
        {
            return waitHelper.WaitForElementToBeVisible(element);
        }

        // Get page title
        public string GetPageTitle()
        {
            waitHelper.WaitForPageLoad();
            return driver.Title;
        }

        // Scroll to element
        protected void ScrollToElement(IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

    }
}
