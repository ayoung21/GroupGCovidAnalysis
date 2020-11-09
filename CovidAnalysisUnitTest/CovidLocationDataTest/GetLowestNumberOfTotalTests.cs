using System;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAnalysisUnitTest.CovidLocationDataTest
{
    [TestClass]
    public class TestingGetLowestNumberOfTotalTests
    {
        [TestMethod]
        public void TestGetLowestNumberOfTotalTestsReturnNull()
        {
            CovidLocationData covidLocationData = new CovidLocationData("GA");

            Assert.IsNull(covidLocationData.GetLowestNumberOfTotalTests(covidLocationData.CovidCases));
        }

        [TestMethod]
        public void TestGetLowestNumberOfTotalTests()
        {
            CovidLocationData covidLocationData = new CovidLocationData("GA");

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2015, 1, 15))
            {
                PositiveIncrease = 50,
                NegativeIncrease = 50
            });

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2016, 2, 15))
            {
                PositiveIncrease = 125,
                NegativeIncrease = 75
            });

            const int expected = 100;
            var result = covidLocationData.GetLowestNumberOfTotalTests(covidLocationData.CovidCases).TotalTestCount;

            Assert.AreEqual(expected, result);
        }
    }
}
