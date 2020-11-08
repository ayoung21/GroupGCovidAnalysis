using System;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAnalysisUnitTest
{
    [TestClass]
    public class TestingGetCurrentHospitalizationsMinimum
    {
        [TestMethod]
        public void TestMinIsZero()
        {
            CovidLocationData covidLocationData = new CovidLocationData("GA");

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

            int expected = 0;
            int result = covidLocationData.GetCurrentHospitalizationsMinimum(covidLocationData.CovidCases);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestMinIsFive()
        {
            CovidLocationData covidLocationData = new CovidLocationData("GA");

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2015, 2, 15))
            {
                HospitalizedCurrently = 5
            });

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2016, 2, 15))
            {
                HospitalizedCurrently = 10
            });

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2017, 2, 15))
            {
                HospitalizedCurrently = 15
            });

            int expected = 5;
            int result = covidLocationData.GetCurrentHospitalizationsMinimum(covidLocationData.CovidCases);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestMinIsTen()
        {
            CovidLocationData covidLocationData = new CovidLocationData("GA");

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2015, 2, 15))
            {
                HospitalizedCurrently = 10
            });

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2016, 2, 15))
            {
                HospitalizedCurrently = 20
            });

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2017, 2, 15))
            {
                HospitalizedCurrently = 25
            });

            int expected = 10;
            int result = covidLocationData.GetCurrentHospitalizationsMinimum(covidLocationData.CovidCases);
            Assert.AreEqual(expected, result);
        }
    }
}
