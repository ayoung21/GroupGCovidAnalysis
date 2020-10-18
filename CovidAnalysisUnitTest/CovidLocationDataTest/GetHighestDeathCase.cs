
using System;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAnalysisUnitTest
{
    [TestClass]
    public class TestingGetHighestDeathCase
    {
        [TestMethod]
        public void TestGetHighestDeathCaseReturnNull()
        {
            CovidLocationData covidLocationData = new CovidLocationData("GA");

            Assert.IsNull(covidLocationData.GetHighestDeathsEvent());
        }

        [TestMethod]
        public void TestGetHighestDeathCase()
        {
            CovidLocationData covidLocationData = new CovidLocationData("GA");

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2015, 2, 15))
            {
                DeathIncrease = 50
            });

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2016, 2, 15))
            {
                DeathIncrease = 125
            });

            int expected = 125;
            int result = covidLocationData.GetHighestDeathsEvent().DeathIncrease;

            Assert.AreEqual(expected, result);
        }
    }
}
