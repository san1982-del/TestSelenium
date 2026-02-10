using SeleniumFrameworkCSharp.BaseTest;
using SeleniumFrameworkCSharp.PageObjects;
using SeleniumFrameworkCSharp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFrameworkCSharp.testcases
{
    [TestFixture]//is essential for telling NUnit that your class contains tests to be executed. It’s the entry point for test discovery in NUnit-based projects.
    [Parallelizable]
    internal class FindNewCarsTest : baseTest
    {
        [Parallelizable(ParallelScope.Children)]
        [Test, TestCaseSource(nameof(GetTestData)), Category("smoke"), Retry(2)]
        public void TestFindNewCar(string browser, string runmode, string carbrand, string cartitle, string carname, string pageUrl)
        {
            // Check if test should run
            runmodecheck(runmode);

            // Setup browser
            SetUp(browser, pageUrl);

            try
            {
                // FLUENT PATTERN - Method Chaining
                new HomePage(driver.Value)
               .FindNewCar()                          // Navigate to new cars
               .ViewBrand()                           // View all brands
               .OpenCarBrandPage(carbrand)            // Open specific brand
               .OpenCarNamePage(carname)            // Open specific car model
               .GetCarPrice();                       // Get car price   

                // Assertions
                string actualTitle = new CarBase(driver.Value).ValidatePageTitle();
                Assert.That(actualTitle, Does.Contain(carname),
                    $"Car Name title not matching. Expected: {carname}, Actual: {actualTitle}");

                // Log results
                baseTest.GetExtentTest()?.Info($"Page for {carname} is Opened");
                baseTest.log.Info($"Test completed successfully for {carname}");
            }
            catch (Exception ex)
            {
                baseTest.GetExtentTest()?.Fail($"Test failed with error: {ex.Message}");
                baseTest.log.Error($"Test execution failed: {ex.Message}", ex);
                throw;
            }
        }

        public static IEnumerable<TestCaseData> GetTestData()
        {
            var columns = new List<string> { "browser", "runmode", "carbrand", "cartitle", "carname", "pageUrl" };
            return DataUtil.GetTestDataFromExcel(
                Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\resources\\testdata.xlsx",
                "FindNewCar",
                columns);
        }
    }
}
