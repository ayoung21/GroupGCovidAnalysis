using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CovidAnalysisUnitTest
{
    [TestClass]
    public class TestingGetHighestHospitalizations
    {
        [TestMethod]
        public void TestGetHighestHospitalizationReturnNull()
        {
            CovidLocationData covidLocationData = new CovidLocationData("GA");

            Assert.IsNull(covidLocationData.GetHighestHospitalization());
        }

        [TestMethod]
        public void TestGetHighestHospitalizationCase()
        {
            CovidLocationData covidLocationData = new CovidLocationData("GA");

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2015, 2, 15))
            {
                HospitalizedIncrease = 50
            });

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2016, 2, 15))
            {
                HospitalizedIncrease = 125
            });

            int expected = 125;
            int result = covidLocationData.GetHighestHospitalization().HospitalizedIncrease;

            Assert.AreEqual(expected, result);
        }
    }
}
