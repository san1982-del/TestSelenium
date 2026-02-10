using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumFrameworkCSharp.BaseTest;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFrameworkCSharp.PageObjects
{
    internal class UsedCarPage : BasePage
    {
        [FindsBy(How = How.XPath, Using = "//input[@placeholder='Type model name, e.g, Used Alto']")]
        private IWebElement searchsecondhandcar;
        [FindsBy(How = How.XPath, Using = "//div[@class='o-D o-ox o-jJ o-j4 o-O o-om o-T']//*[name()='svg']")]
        private IWebElement searchbutton;
        public UsedCarPage(IWebDriver driver) : base(driver)
        {

        }
            public void SearchUsedcars(string carname, string carbrand, string cartitle, string searchtype)
            {

            try
            {
                if (searchtype == "car")
                {

                    searchsecondhandcar.SendKeys(carname);
                    baseTest.log.Info("Carname " + carname + " is entered for Search ");

                }
                else if (searchtype == "brand")
                {

                    searchsecondhandcar.SendKeys(carbrand);
                    baseTest.log.Info("Car Brand Name " + carbrand + " is entered for Search ");

                }
                waitHelper.WaitForPageLoad(10);
                WaitForElementToBeClickable(searchbutton);
                searchbutton.Click();
                baseTest.log.Info("search is clicked ");
                waitHelper.WaitForPageLoad(10);
            }
            catch (Exception ex)
            {
                baseTest.GetExtentTest()?.Fail($"Failed to enter search criteria: {ex.Message}");
                baseTest.log.Error($"Error entering search criteria: {ex.Message}", ex);
                throw;
            }
            
        }
    }
}
