using NUnit.Framework.Constraints;
using OpenQA.Selenium;
using SeleniumFrameworkCSharp.BaseTest;
using SeleniumFrameworkCSharp.PageObjects.CarBrandPages;
using SeleniumFrameworkCSharp.PageObjects.CarPages;
using SeleniumFrameworkCSharp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFrameworkCSharp.PageObjects
{
    internal class CarBase

    {
        IWebDriver driver;
        public CarBase(IWebDriver driver) 
        {
            this.driver = driver;
        }
        
        public string ValidatePageTitle()
        {
            return BasePage.keyword.GetText("CarBase", "cartitle", "XPATH");
            
        }

        public string GetCarNameAndPrice()
        {
            StringBuilder PriceNameData = new StringBuilder();
            //string PriceNameData = "";
            for (int i = 0; i < BasePage.keyword.GetWebElements("CarBase", "carprice", "XPATH").Count; i++)
            {
                //string ActualPriceNameData = BasePage.keyword.GetWebElements("CarBase", "carname", "XPATH")[i].Text + "----Car price is : " + BasePage.keyword.GetWebElements("CarBase", "carprice", "XPATH")[i].Text;
                PriceNameData.Append(BasePage.keyword.GetWebElements("CarBase", "carname", "XPATH")[i].Text)
                .Append("----Car price is : ")
                .Append(BasePage.keyword.GetWebElements("CarBase", "carprice", "XPATH")[i].Text)
                .Append("<br>");
               // PriceNameData += ActualPriceNameData + "\t" + ;
               
                
            }
            return PriceNameData.ToString();
        }

        public void GlobalSearch(string carname, string carbrand,string cartitle, string searchtype)
        {
            if (searchtype == "car")
            {
                BasePage.keyword.Type("HomePage", "search", "XPATH", carname);
                
                baseTest.log.Info("Carname "+ carname+ " is entered for Search ");
            }
            else if (searchtype == "brand")
            {
                BasePage.keyword.Type("HomePage", "search", "XPATH", carbrand);
                baseTest.log.Info("Car Brand Name "+ carbrand +" is entered for Search ");
            }

            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> menu = BasePage.keyword.GetWebElements("HomePage", "dropdownmenu", "XPATH");
            SearchDropDownSelection(carname, carbrand, cartitle, menu);
            Thread.Sleep(5000);
        }

        public void SearchDropDownSelection(string carname, string carbrand, string cartitle, System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> menu)
        {

            foreach (IWebElement item in menu)
            {
                if (item.GetAttribute("data-label") == carname)
                {
                    item.Click();
                    baseTest.log.Info("Car name " + carname + " is selected for search");
                    Assert.That(carname.Equals(BasePage.carBase.ValidatePageTitle()), "car titles not matching for : " + carname);
                    break;
                }
                else if (item.GetAttribute("data-label") == "All " + carbrand + " Cars")
                {
                    item.Click();
                    baseTest.log.Info("Car Brand Name " + "All " + carbrand + " Cars" + " is selected for Search ");
                    Assert.That(cartitle.Equals(BasePage.carBase.ValidatePageTitle()), "car titles not matching for : " + cartitle);
                    break;
                }
                else if (item.GetAttribute("data-label") == "Used " + carbrand + "/in Mumbai$/")
                {
                    item.Click();
                    baseTest.log.Info("Car Brand Name " + "Used " + carbrand + " in Mumbai" + " is selected for Search ");
                    Assert.That(cartitle.Equals(BasePage.carBase.ValidatePageTitle()), "car titles not matching for : " + cartitle);
                    break;
                }

            }
        }

        public List<string> Getdata_DB(string query)
        {
            // Step 1: Fetch expected data from database
            DBManager.SetMySQLDBConnection();
            List<string> expectedDBData = DBManager.GetQuery(query);
            baseTest.GetExtentTest()?.Info("Fetched expected data from database");
            baseTest.log.Info("Fetched expected data from database");
            return expectedDBData;
        }
    }
}
