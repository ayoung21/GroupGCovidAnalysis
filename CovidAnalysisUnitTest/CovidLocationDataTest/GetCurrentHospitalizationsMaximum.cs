using System;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAnalysisUnitTest.CovidLocationDataTest
{
    [TestClass]
    public class TestingGetCurrentHospitalizationsMaximum
    {
        [TestMethod]
        public void TestMaxIsZero()
        {
            var covidLocationData = new CovidLocationData("GA");

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2015, 2, 15))
            {
                HospitalizedCurrently = 0
            });

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2016, 2, 15))
            {
                HospitalizedCurrently = 0
            });

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2017, 2, 15))
            {
                HospitalizedCurrently = 0
            });

            const int expected = 0;
            var result = covidLocationData.GetCurrentHospitalizationsMaximum(covidLocationData.CovidCases);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestMaxIsFive()
        {
            var covidLocationData = new CovidLocationData("GA");

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2015, 2, 15))
            {
                HospitalizedCurrently = 0
            });

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2016, 2, 15))
            {
                HospitalizedCurrently = 5
            });

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2017, 2, 15))
            {
                HospitalizedCurrently = 0
            });

            const int expected = 5;
            var result = covidLocationData.GetCurrentHospitalizationsMaximum(covidLocationData.CovidCases);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestMaxIsTen()
        {
            var covidLocationData = new CovidLocationData("GA");

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2015, 2, 15))
            {
                HospitalizedCurrently = 0
            });

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2016, 2, 15))
            {
                HospitalizedCurrently = 5
            });

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2017, 2, 15))
            {
                HospitalizedCurrently = 10
            });

            const int expected = 10;
            var result = covidLocationData.GetCurrentHospitalizationsMaximum(covidLocationData.CovidCases);
            Assert.AreEqual(expected, result);
        }
    }
}
