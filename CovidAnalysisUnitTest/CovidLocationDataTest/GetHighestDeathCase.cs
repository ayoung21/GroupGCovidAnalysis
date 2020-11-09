using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CovidAnalysisUnitTest.CovidLocationDataTest
{
    [TestClass]
    public class TestingGetHighestDeathCase
    {
        [TestMethod]
        public void TestGetHighestDeathCaseReturnNull()
        {
            var covidLocationData = new CovidLocationData("GA");

            Assert.IsNull(covidLocationData.GetHighestDeathsEvent());
        }

        [TestMethod]
        public void TestGetHighestDeathCase()
        {
            var covidLocationData = new CovidLocationData("GA");

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2015, 2, 15))
            {
                DeathIncrease = 50
            });

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2016, 2, 15))
            {
                DeathIncrease = 125
            });

            const int expected = 125;
            var result = covidLocationData.GetHighestDeathsEvent().DeathIncrease;

            Assert.AreEqual(expected, result);
        }
    }
}
