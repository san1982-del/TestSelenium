using AventStack.ExtentReports;
using AventStack.ExtentReports.MarkupUtils;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFrameworkCSharp.BaseTest
{
    [SetUpFixture]
    internal class baseTest
    {
        public static ThreadLocal<IWebDriver> driver = new();
        private static ExtentReports extent;
        public static ThreadLocal<ExtentTest> exTest = new();
        public static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IConfiguration configuration;
        static string fileName;

        public static IWebDriver GetDriver() => driver.Value;

        public static ExtentTest GetExtentTest() => exTest.Value;

        [SetUp]
        public void BeforeEachTest()
        {
            log.Info("Test Execution is Started");
            exTest.Value = extent.CreateTest(
                TestContext.CurrentContext.Test.ClassName +
                " @ Test Case Name : " +
                TestContext.CurrentContext.Test.Name);
        }

        [TearDown]
        public void AfterEachTest()
        {
            log.Info("Test Execution is Completed");
            var testStatus = TestContext.CurrentContext.Result.Outcome.Status;

            if (testStatus == TestStatus.Passed)
            {
                GetExtentTest().Pass("Test case passed");
                GetExtentTest().Pass(MarkupHelper.CreateLabel("PASS", ExtentColor.Green));
            }
            else if (testStatus == TestStatus.Skipped)
            {
                GetExtentTest().Skip("Test Skipped : " + TestContext.CurrentContext.Result.Message);
                GetExtentTest().Skip(MarkupHelper.CreateLabel("SKIP", ExtentColor.Amber));
            }
            else if (testStatus == TestStatus.Failed)
            {
                CaptureScreenshot();
                GetExtentTest().Fail("Test Failed : " + TestContext.CurrentContext.Result.Message);
                GetExtentTest().Fail(
                    "<b><font color=red>Screenshot of failure</font></b><br>",
                    MediaEntityBuilder.CreateScreenCaptureFromPath("../screenshots/" + fileName
                    ).Build()
                );
                GetExtentTest().Fail(MarkupHelper.CreateLabel("FAIL", ExtentColor.Red));
            }

            if (driver.Value != null)
            {
                GetDriver().Quit();
            }
        }

        private void CaptureScreenshot()
        {
            try
            {
                DateTime currentTime = DateTime.Now;
                fileName = currentTime.ToString("yyyy-MM-dd_HH-mm-ss") + ".jpg";

                var screenshot = GetDriver()?.TakeScreenshot();
                if (screenshot != null)
                {
                    screenshot.SaveAsFile(
                        Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName +
                        "\\screenshots\\" +
                        fileName
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Screenshot capture failed: " + ex.Message);
            }
        }

        private dynamic GetBrowserOptions(string browserName)
        {
            switch (browserName.ToLower())
            {
                case "chrome":
                    var chrome = new ChromeOptions();
                    //chrome.AddArgument("--headless=new");
                    chrome.AddArgument("--no-sandbox");
                    chrome.AddArgument("--disable-dev-shm-usage");
                    chrome.AddArgument("--disable-gpu");
                    chrome.AddArgument("--disable-blink-features=AutomationControlled");
                    chrome.AddExcludedArgument("enable-automation");
                    chrome.AddAdditionalOption("useAutomationExtension", false);
                    chrome.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36");
                    // chrome.AddArgument("--window-size=1920,1080");
                    return chrome;

                case "firefox":
                    var ff = new FirefoxOptions();
                    // ff.AddArgument("--headless");
                    // ff.AddArgument("--width=1920");
                    //ff.AddArgument("--height=1080");
                    return ff;

                default:
                    return new ChromeOptions();
            }
        }

        public void SetUp(string browserName, string customUrl = null)
        {
            var commandTimeout = TimeSpan.FromMinutes(3);
            dynamic options = GetBrowserOptions(browserName);

            // IMPORTANT: Do NOT set platformName ("windows" would break Grid)
            // Grid automatically detects Linux container platform.

            driver.Value = new RemoteWebDriver(
                new Uri(configuration["AppSettings:gridurl"]),
                options.ToCapabilities(),
                commandTimeout
            );

            string urlToNavigate = customUrl ?? configuration["AppSettings:testsiteurl"];
            GetDriver().Navigate().GoToUrl(urlToNavigate);
            GetDriver().Manage().Cookies.DeleteAllCookies();
            GetDriver().Manage().Window.Maximize();
            GetDriver().Manage().Timeouts().ImplicitWait =
                TimeSpan.FromSeconds(int.Parse(configuration["AppSettings:implicit.wait"]));
        }

        static baseTest()
        {
            DateTime currentTime = DateTime.Now;
            string reportName = "Extent_" + currentTime.ToString("yyyy-MM-dd_HH-mm-ss") + ".html";
            extent = CreateInstance(reportName);
        }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Loging();
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\resources\\")
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        private void Loging()
        {
            var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo(
                Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName +
                "\\resources\\log4net.config"
            ));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            extent.Flush();
            if (driver != null)
            {
                driver.Dispose();
                exTest.Dispose();
                log.Info("Test Execution Completed");
            }
        }

        public void runmodecheck(string runmode)
        {
            if (runmode.Equals("N"))
            {
                Assert.Ignore("Ignoring the test as the run mode is NO");
            }
        }

        public static ExtentReports CreateInstance(string fileName)
        {
            var htmlReporter = new ExtentSparkReporter(
                Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName +
                "\\reports\\" +
                fileName
            );

            htmlReporter.Config.Theme = Theme.Standard;
            htmlReporter.Config.DocumentTitle = "AUTOMATION Test Suite";
            htmlReporter.Config.ReportName = "Automation Test Results";
            htmlReporter.Config.Encoding = "utf-8";

            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);

            extent.AddSystemInfo("Automation Tester", "XXXX");
            extent.AddSystemInfo("Organization", "YYYY");
            extent.AddSystemInfo("Build No:", "ZZZZ");

            return extent;
        }
    }
}
