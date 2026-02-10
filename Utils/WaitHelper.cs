using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFrameworkCSharp.Utils
{
    internal class WaitHelper
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private const int DEFAULT_TIMEOUT = 10;

        public WaitHelper(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(DEFAULT_TIMEOUT));
        }

        // Wait for element to be clickable
        public IWebElement WaitForElementToBeClickable(IWebElement element, int timeoutInSeconds = DEFAULT_TIMEOUT)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(drv =>
            {
                try
                {
                    if (element.Enabled && element.Displayed)
                        return element;
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            });
        }

        // Wait for element to be visible
        public IWebElement WaitForElementToBeVisible(IWebElement element, int timeoutInSeconds = DEFAULT_TIMEOUT)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(drv =>
            {
                try
                {
                    if (element.Displayed)
                        return element;
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            });
        }

        // Wait for element by locator
        public IWebElement WaitForElement(By locator, int timeoutInSeconds = DEFAULT_TIMEOUT)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }

        // Wait for element to be clickable by locator
        public IWebElement WaitForElementClickable(By locator, int timeoutInSeconds = DEFAULT_TIMEOUT)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        // Wait for multiple elements
        public ReadOnlyCollection<IWebElement> WaitForElements(By locator, int timeoutInSeconds = DEFAULT_TIMEOUT)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(locator));
        }

        // Wait for text to be present in element
        public bool WaitForTextInElement(IWebElement element, string text, int timeoutInSeconds = DEFAULT_TIMEOUT)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(drv =>
            {
                try
                {
                    return element.Text.Contains(text);
                }
                catch
                {
                    return false;
                }
            });
        }

        // Wait for page to load completely
        public void WaitForPageLoad(int timeoutInSeconds = DEFAULT_TIMEOUT)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(drv => ((IJavaScriptExecutor)drv).ExecuteScript("return document.readyState").Equals("complete"));
        }
    }

}

