using OpenQA.Selenium;
using SeleniumFrameworkCSharp.BaseTest;
using SeleniumFrameworkCSharp.PageObjects.CarBrandPages;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumExtras.PageObjects;

namespace SeleniumFrameworkCSharp.PageObjects
{
    internal class NewCarPage : BasePage
    {
        // Page Factory Elements
        [FindsBy(How = How.XPath, Using = "//div[contains(text(),'View More Brands')]")]
        private IWebElement viewBrandButton;

        [FindsBy(How = How.LinkText, Using = "BMW")]
        private IWebElement bmwBrandLink;

        [FindsBy(How = How.LinkText, Using = "Kia")]
        private IWebElement kiaBrandLink;

        [FindsBy(How = How.LinkText, Using = "Audi")]
        private IWebElement audiBrandLink;

        [FindsBy(How = How.LinkText, Using = "Toyota")]
        private IWebElement toyotaBrandLink;

        // Constructor
        public NewCarPage(IWebDriver driver) : base(driver)
        {
        }

        // View all brands - Fluent method
        public NewCarPage ViewBrand()
        {
            WaitForElementToBeClickable(viewBrandButton);
            viewBrandButton.Click();

            baseTest.GetExtentTest()?.Info("Clicked on View Brand button");
            baseTest.log.Info("View Brand button clicked");

            return this; // Return this for method chaining
        }

        // Generic method to open any car brand page - Fluent pattern
        public CarBrandPage OpenCarBrandPage(string brandName)
        {
            IWebElement brandLink = brandName.ToUpper() switch
            {
                "BMW" => bmwBrandLink,
                "KIA" => kiaBrandLink,
                "AUDI" => audiBrandLink,
                "TOYOTA" => toyotaBrandLink,
                _ => throw new ArgumentException($"Brand '{brandName}' is not supported")
            };

            WaitForElementToBeClickable(brandLink);
            brandLink.Click();

            baseTest.GetExtentTest()?.Info($"Opened {brandName} brand page");
            baseTest.log.Info($"{brandName} brand page opened");

            // Wait for page to load
            waitHelper.WaitForPageLoad();

            return new CarBrandPage(driver);
        }

        // Specific brand methods (can still keep for backward compatibility)
        public BMWCarBrandPage OpenBMWCarBrandPage()
        {
            WaitForElementToBeClickable(bmwBrandLink);
            bmwBrandLink.Click();

            waitHelper.WaitForPageLoad();

            baseTest.GetExtentTest()?.Info("Opened BMW brand page");
            return new BMWCarBrandPage(driver);
        }

        public ToyotaCarBrandPage OpenToyotaCarBrandPage()
        {
            WaitForElementToBeClickable(toyotaBrandLink);
            toyotaBrandLink.Click();

            waitHelper.WaitForPageLoad();

            baseTest.GetExtentTest()?.Info("Opened Toyota brand page");
            return new ToyotaCarBrandPage(driver);
        }

        public AudiCarBrandPage OpenAudiCarBrandPage()
        {
            WaitForElementToBeClickable(audiBrandLink);
            audiBrandLink.Click();

            waitHelper.WaitForPageLoad();

            baseTest.GetExtentTest()?.Info("Opened Audi brand page");
            return new AudiCarBrandPage(driver);
        }
    }
}
