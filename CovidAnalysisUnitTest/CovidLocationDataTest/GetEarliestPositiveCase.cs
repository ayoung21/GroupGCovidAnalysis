using System;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAnalysisUnitTest
{
    [TestClass]
    public class TestingGetEarliestPositiveCase
    {
        [TestMethod]
        public void TestGetEarliestPositiveCaseReturnNull()
        {
            CovidLocationData covidLocationData = new CovidLocationData("GA");

            Assert.IsNull(covidLocationData.GetEarliestPositiveCase());
        }

        [TestMethod]
        public void TestGetEarliestPositiveCase()
        {
            CovidLocationData covidLocationData = new CovidLocationData("GA");

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2016, 2, 15))
            {
                PositiveIncrease = 125
            });
            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2016, 2, 10))
            {
                PositiveIncrease = 125
            });
            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2016, 2, 12))
            {
                PositiveIncrease = 125
            });

            var expected = "2/10/2016";

            var covidCase = covidLocationData.GetEarliestPositiveCase();
            var result = covidCase.Date.ToShortDateString();

            Assert.AreEqual(expected, result, true);
        }
    }
}
