using System;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAnalysisUnitTest.CovidLocationDataTest
{
    [TestClass]
    public class TestingGetEventsFromMonth
    {
        [TestMethod]
        public void TestInvalidLowerBound()
        {
            var covidLocationData = new CovidLocationData("GA");

            Assert.ThrowsException<ArgumentNullException>(() =>
                covidLocationData.GetEventsFromMonth(0));
        }

        [TestMethod]
        public void TestInvalidUpperBound()
        {
            var covidLocationData = new CovidLocationData("GA");

            Assert.ThrowsException<ArgumentNullException>(() =>
                covidLocationData.GetEventsFromMonth(13));
        }

        [TestMethod]
        public void TestZeroEventsFromMonth()
        {
            var covidLocationData = new CovidLocationData("GA");

            const int expected = 0;
            var result = covidLocationData.GetEventsFromMonth(1).Count;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestOneEventsFromMonth()
        {
            var covidLocationData = new CovidLocationData("GA");

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2015, 1, 15))
            {
                HospitalizedCurrently = 0
            });

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2016, 2, 15))
            {
                HospitalizedCurrently = 5
            });

            const int expected = 1;
            var result = covidLocationData.GetEventsFromMonth(1).Count;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestManyEventsFromMonth()
        {
            var covidLocationData = new CovidLocationData("GA");

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2015, 1, 15))
            {
                HospitalizedCurrently = 0
            });

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2016, 1, 15))
            {
                HospitalizedCurrently = 5
            });

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2017, 1, 15))
            {
                HospitalizedCurrently = 5
            });

            const int expected = 3;
            var result = covidLocationData.GetEventsFromMonth(1).Count;
            Assert.AreEqual(expected, result);
        }
    }
}
