
using System;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAnalysisUnitTest
{
    [TestClass]
    public class CovidLocationDataTest
    {
        [TestMethod]
        public void TestAddCovidCase()
        {
            CovidLocationData covidLocationData = new CovidLocationData("GA");
            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2015, 1, 15)));
            int expected = 1;
            int result = covidLocationData.CovidCases.Count;

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestAddCovidCaseThrowsNullException()
        {
            CovidLocationData covidLocationData = new CovidLocationData("GA");

            Assert.ThrowsException<ArgumentNullException>(() =>
                covidLocationData.AddCovidCase(null));
        }
    }
}
