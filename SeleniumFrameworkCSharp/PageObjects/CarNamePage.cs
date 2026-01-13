using OpenQA.Selenium;
using MySql.Data.MySqlClient;
using SeleniumFrameworkCSharp.BaseTest;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SeleniumFrameworkCSharp.Utils;
using System.Data.Common;

namespace SeleniumFrameworkCSharp.PageObjects
{
    internal class CarNamePage : BasePage
    {
        // Page Factory Elements
        [FindsBy(How = How.XPath, Using = "//div[2]/div[1]/div[2]/div[1]/span")]
        private IWebElement carPriceElement;

        [FindsBy(How = How.XPath, Using = "//div[contains(text(),'View More Variants')]")]
        private IWebElement viewVariantsButton;

        [FindsBy(How = How.XPath, Using = "//section/div/div[2]/table")]
        private IWebElement variantTable;

        [FindsBy(How = How.XPath, Using = "//section/div/div[2]/table/tbody/tr")]
        private IList<IWebElement> tableRows;

        [FindsBy(How = How.XPath, Using = "//section/div/div[2]/table/tbody/tr[1]/td")]
        private IList<IWebElement> tableColumns;

        // Constructor
        public CarNamePage(IWebDriver driver) : base(driver)
        {
        }
        
        // Get car price with improved wait
        public string GetCarPrice()
        {
            WaitForElementToBeVisible(carPriceElement);
            string price = carPriceElement.Text;

            baseTest.GetExtentTest()?.Info($"Retrieved car price: {price}");
            baseTest.log.Info($"Car price: {price}");

            return price;
        }

        // Get car variant table data with improved wait
        public List<string> GetCarVariantWebTable()
        {
            List<string> tableRowsData = new List<string>();
            try
            {
                try
                {
                    // Check if variants button exists and click it
                    if (viewVariantsButton.Displayed)
                    {
                        WaitForElementToBeClickable(viewVariantsButton);
                        viewVariantsButton.Click();
                    }
                }
                catch (NoSuchElementException)
                { 
                }
                // Wait for table to load
                WaitForElementToBeVisible(variantTable);
                waitHelper.WaitForElements(By.XPath("//section/div/div[2]/table/tbody/tr"));

                int totalRows = tableRows.Count;
                int totalCols = tableColumns.Count;

                baseTest.GetExtentTest()?.Info($"Variant table has {totalRows} rows and {totalCols} columns");

                // Extract table data
                for (int i = 1; i <= totalRows; i++)
                {
                    StringBuilder rowData = new StringBuilder();
                    for (int j = 1; j <= totalCols; j++)
                    {
                        var cellLocator = By.XPath($"//section/div/div[2]/table/tbody/tr[{i}]/td[{j}]");
                        var cell = waitHelper.WaitForElement(cellLocator);

                        var cleanText = System.Text.RegularExpressions.Regex.Replace(cell.Text, @"\s+", " ").Trim();
                        rowData.Append(cleanText).Append(" | ");
                    }
                    tableRowsData.Add(rowData.ToString().TrimEnd(' ', '|'));
                }
            }
            catch (Exception ex)
            {
                baseTest.GetExtentTest()?.Warning($"Could not retrieve variant table: {ex.Message}");
                baseTest.log.Warn($"Variant table error: {ex.Message}");
            }

            return tableRowsData;
        }

        
            
    }
}
