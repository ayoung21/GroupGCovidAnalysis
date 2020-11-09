
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CovidAnalysisUnitTest
{
    [TestClass]
    public class TestingGetHighestNumberOfNegativeIncreases
    {
        [TestMethod]
        public void TestNull()
        {
            CovidLocationData covidLocationData = new CovidLocationData("GA");

            Assert.IsNull(covidLocationData.GetHighestNumberOfNegativeIncreases());
        }

        [TestMethod]
        public void TestHighestWithOnlyOneCase()
        {
            CovidLocationData covidLocationData = new CovidLocationData("GA");

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2015, 2, 15)) {
                NegativeIncrease = 0
            });

            var expected = 0;
            var result = covidLocationData.GetHighestNumberOfNegativeIncreases().NegativeIncrease;

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestHighestWithManyCases()
        {
            CovidLocationData covidLocationData = new CovidLocationData("GA");

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2015, 2, 15))
            {
                NegativeIncrease = 0
            });

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2016, 2, 15))
            {
                NegativeIncrease = 5
            });

            var expected = 5;
            var result = covidLocationData.GetHighestNumberOfNegativeIncreases().NegativeIncrease;

            Assert.AreEqual(expected, result);
        }
    }
}
