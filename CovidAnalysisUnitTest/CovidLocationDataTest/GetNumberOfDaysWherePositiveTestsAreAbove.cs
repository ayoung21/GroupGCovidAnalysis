using System;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAnalysisUnitTest
{
    [TestClass]
    public class TestingGetNumberOfDaysWherePositiveTestsAreAbove
    {
        [TestMethod]
        public void TestPositiveTestAboveFive()
        {
            const int testCount = 5;

            CovidLocationData covidLocationData = new CovidLocationData("GA");

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2015, 2, 15))
            {
                PositiveIncrease = 0
            });

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2016, 2, 15))
            {
                PositiveIncrease = 6
            });

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2017, 2, 15))
            {
                PositiveIncrease = 10
            });

            int expected = 2;
            int result   = covidLocationData.GetNumberOfDaysWherePositiveTestsAreAbove(testCount);
            Assert.AreEqual(expected, result);
        }
    }
}
