using SeleniumFrameworkCSharp.BaseTest;
using SeleniumFrameworkCSharp.PageObjects;
using SeleniumFrameworkCSharp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFrameworkCSharp.Testcases
{
    [TestFixture]
    [Parallelizable]
    internal class FindUsedCarTest : baseTest   
    {
        [Parallelizable(ParallelScope.Children)]
        [Test, TestCaseSource(nameof(GetTestData)), Category("smoke"), Retry(2)]
        public void TestFindUsedCar(string browser, string runmode, string carbrand, string cartitle, string carname, string searchtype)
        {
            // Check if test should run
            runmodecheck(runmode);

            // Setup browser
            SetUp(browser);

            try
            {
                // FLUENT PATTERN - Method Chaining!
                 new HomePage(driver.Value)
                .ExploreUsedCar() //Open Explore Used Car Page
                .SearchUsedcars(carname, carbrand, cartitle, searchtype); // search used car
                                          
                // Assertions
                string actualTitle = driver.Value.Title;
                Assert.That(actualTitle, Does.Contain(carbrand),
                    $"Car Name title not matching. Expected: {carbrand}, Actual: {actualTitle}");
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
            var columns = new List<string> { "browser", "runmode", "carbrand", "cartitle", "carname", "searchtype" };
            return DataUtil.GetTestDataFromExcel(
                Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\resources\\testdata.xlsx",
                "FindNewCar",
                columns);
        }
    }

}
