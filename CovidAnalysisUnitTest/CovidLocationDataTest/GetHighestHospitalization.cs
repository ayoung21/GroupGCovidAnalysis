using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CovidAnalysisUnitTest.CovidLocationDataTest
{
    [TestClass]
    public class TestingGetHighestHospitalizations
    {
        [TestMethod]
        public void TestGetHighestHospitalizationReturnNull()
        {
            var covidLocationData = new CovidLocationData("GA");

            Assert.IsNull(covidLocationData.GetHighestHospitalization());
        }

        [TestMethod]
        public void TestGetHighestHospitalizationCase()
        {
            var covidLocationData = new CovidLocationData("GA");

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2015, 2, 15))
            {
                HospitalizedIncrease = 50
            });

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2016, 2, 15))
            {
                HospitalizedIncrease = 125
            });

            const int expected = 125;
            var result = covidLocationData.GetHighestHospitalization().HospitalizedIncrease;

            Assert.AreEqual(expected, result);
        }
    }
}
