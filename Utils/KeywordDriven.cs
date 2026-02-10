using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumFrameworkCSharp.BaseTest;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace SeleniumFrameworkCSharp.Utils
    
{
    internal class KeywordDriven
    {
        private string text;
        private System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> elements;
        private IWebElement webelement;

        public void Click(string pageName, string locatorName, string locatorType)
        {
            if (locatorType.Contains("ID"))
            {
                baseTest.GetDriver().FindElement(By.Id(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType))).Click();

            }
            else if (locatorType.Contains("XPATH"))
            {
                baseTest.GetDriver().FindElement(By.XPath(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType))).Click();

            }
            else if (locatorType.Contains("CSS"))
            {
                baseTest.GetDriver().FindElement(By.CssSelector(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType))).Click();

            }
            else if (locatorType.Contains("LINK"))
            {
                baseTest.GetDriver().FindElement(By.LinkText(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType))).Click();

            }

            baseTest.GetExtentTest().Info("Clicking on an Element : "+locatorName);
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> GetWebElements(string pageName, string locatorName, string locatorType)
        {
            if (locatorType.Contains("ID"))
            {
               elements = baseTest.GetDriver().FindElements(By.Id(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType)));

            }
            else if (locatorType.Contains("XPATH"))
            {
                elements = baseTest.GetDriver().FindElements(By.XPath(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType)));

            }
            else if (locatorType.Contains("CSS"))
            {
                elements = baseTest.GetDriver().FindElements(By.CssSelector(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType)));

            }
            else if (locatorType.Contains("LINK"))
            {
                elements = baseTest.GetDriver().FindElements(By.LinkText(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType)));

            }
            
            baseTest.GetExtentTest().Info("Get Collection of WebElements : " + locatorName);
            return elements;
        }

        public void MoveToElemet(string pageName, string locatorName, string locatorType)
        {
            IWebElement element;
            
            if (locatorType.Contains("ID"))
            {
               element = baseTest.GetDriver().FindElement(By.Id(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType)));
               new Actions(baseTest.GetDriver()).MoveToElement(element).Perform();

            }
            else if (locatorType.Contains("XPATH"))
            {
                element = baseTest.GetDriver().FindElement(By.XPath(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType)));
                new Actions(baseTest.GetDriver()).MoveToElement(element).Perform();
            }
            else if (locatorType.Contains("CSS"))
            {
                element = baseTest.GetDriver().FindElement(By.CssSelector(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType)));
                new Actions(baseTest.GetDriver()).MoveToElement(element).Perform();
            }

            baseTest.GetExtentTest().Info("Clicking on an Element : " + locatorName);
        }

        public void Type(string pageName, string locatorName, string locatorType, string value)
        {

            if (locatorType.Contains("ID"))
            {
                baseTest.GetDriver().FindElement(By.Id(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType))).SendKeys(value);

            }else if (locatorType.Contains("XPATH"))
            {
                baseTest.GetDriver().FindElement(By.XPath(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType))).SendKeys(value);

            }else if (locatorType.Contains("CSS"))
            {
                baseTest.GetDriver().FindElement(By.CssSelector(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType))).SendKeys(value);

            }


            baseTest.GetExtentTest().Info("Typing in an Element : " + locatorName+ " value entered as : "+value);

        }

        public void Select(string pageName, string locatorName, string locatorType, string value)
        {

            IWebElement element = null;

            if (locatorType.Contains("ID"))
            {
                element = baseTest.GetDriver().FindElement(By.Id(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType)));

            }
            else if (locatorType.Contains("XPATH"))
            {
                element = baseTest.GetDriver().FindElement(By.XPath(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType)));

            }
            else if (locatorType.Contains("CSS"))
            {
                element = baseTest.GetDriver().FindElement(By.CssSelector(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType)));

            }

            SelectElement selectElement = new SelectElement(element);
            selectElement.SelectByValue(value);

            baseTest.GetExtentTest().Info("Seleting an Element : " + locatorName + " selected the value as : " + value);

        }

        public bool isElementPresent(string pageName, string locatorName, string locatorType)
        {
            try
            {
                if (locatorType.Contains("ID"))
                {
                    baseTest.GetDriver().FindElement(By.Id(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType)));

                }
                else if (locatorType.Contains("XPATH"))
                {
                    baseTest.GetDriver().FindElement(By.XPath(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType)));

                }
                else if (locatorType.Contains("CSS"))
                {
                    baseTest.GetDriver().FindElement(By.CssSelector(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType)));

                }
                baseTest.GetExtentTest().Info("Finding an Element : " + locatorName);

                return true;
            }
            catch (Exception ex)
            {
                baseTest.GetExtentTest().Info("Error while finding an Element : " + locatorName);

                return false;
            }
        }

        public string GetText(string pageName, string locatorName, string locatorType)
        {
            if (locatorType.Contains("ID"))
            {
                text = baseTest.GetDriver().FindElement(By.Id(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType))).Text;

            }
            else if (locatorType.Contains("XPATH"))
            {
                text = baseTest.GetDriver().FindElement(By.XPath(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType))).Text;

            }
            else if (locatorType.Contains("CSS"))
            {
                text =  baseTest.GetDriver().FindElement(By.CssSelector(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType))).Text;

            }
            else if (locatorType.Contains("LINK"))
            {
                text = baseTest.GetDriver().FindElement(By.LinkText(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType))).Text;

            }

            baseTest.GetExtentTest().Info("Getting the text of an Element : " + locatorName);
            return text;
        }

        public IWebElement FindWebElement(string pageName, string locatorName, string locatorType)
        {

            if (locatorType.Contains("ID"))
            {
               webelement = baseTest.GetDriver().FindElement(By.Id(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType)));

            }
            else if (locatorType.Contains("XPATH"))
            {
                webelement = baseTest.GetDriver().FindElement(By.XPath(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType)));

            }
            else if (locatorType.Contains("CSS"))
            {
                webelement = baseTest.GetDriver().FindElement(By.CssSelector(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType)));

            }
            else if (locatorType.Contains("LINK"))
            {
                webelement = baseTest.GetDriver().FindElement(By.LinkText(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType)));

            }

            baseTest.GetExtentTest().Info("Finding the WebElement : " + locatorName);
            return webelement;
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> GetWebElementsFromVariable(string pageName, string locatorName, string locatorType, IWebElement webelement)
        {
            
            if (locatorType.Contains("ID"))
            {
                elements = webelement.FindElements(By.Id(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType)));

            }
            else if (locatorType.Contains("XPATH"))
            {
                elements = webelement.FindElements(By.XPath(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType)));

            }
            else if (locatorType.Contains("CSS"))
            {
                elements = webelement.FindElements(By.CssSelector(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType)));

            }
            else if (locatorType.Contains("LINK"))
            {
                elements = webelement.FindElements(By.LinkText(XMLLocatorReader.GetLocatorValue(pageName, locatorName, locatorType)));

            }

            baseTest.GetExtentTest().Info("Get Collection of WebElements : " + locatorName);
            return elements;
        }

    }
}
