using SeleniumFrameworkCSharp.BaseTest;
using SeleniumFrameworkCSharp.PageObjects;
using SeleniumFrameworkCSharp.Utils;


namespace SeleniumFrameworkCSharp.Testcases
{
    [TestFixture]
    [Parallelizable]
    internal class CarVariantWebTablevValidationTest : baseTest
    {
        [Parallelizable(ParallelScope.Children)]
        [Test, TestCaseSource(nameof(GetTestData)), Category("smoke"), Retry(1)]
        public void TestCarVariantWebTableValidation(string browser, string runmode, string carbrand, string carname, string query, string pageUrl)
        {
            runmodecheck(runmode); // Check if test should run
            List<string> expectedDBData = new CarBase(driver.Value)
                .Getdata_DB(query); // Get data from DB

            SetUp(browser, pageUrl);   // Setup browser

            List<string> actualWebData = new CarNamePage(driver.Value)
                .GetCarVariantWebTable(); // Get data from Web Table

            // Sort both lists before comparison
            expectedDBData.Sort();
            actualWebData.Sort();

            Assert.That(actualWebData, Is.EqualTo(expectedDBData),
                "Car Variant Web Table data does not match with Database data.");

        }
        public static IEnumerable<TestCaseData> GetTestData()
        {
            var columns = new List<string> { "browser", "runmode", "carbrand", "carname","query", "pageUrl" };
            return DataUtil.GetTestDataFromExcel(
                Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\resources\\testdata.xlsx",
                "FindNewCar",
                columns);
        }
    }

}
