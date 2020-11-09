using System;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAnalysisUnitTest.CovidLocationDataTest
{
    [TestClass]
    public class CovidLocationDataTest
    {
        [TestMethod]
        public void TestAddCovidCase()
        {
            var covidLocationData = new CovidLocationData("GA");
            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2015, 1, 15)));
            const int expected = 1;
            var result = covidLocationData.CovidCases.Count;

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestAddCovidCaseThrowsNullException()
        {
            var covidLocationData = new CovidLocationData("GA");

            Assert.ThrowsException<ArgumentNullException>(() =>
                covidLocationData.AddCovidCase(null));
        }
    }
}
