
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CovidAnalysisUnitTest
{
    [TestClass]
    public class TestingGetEventsFromMonth
    {
        [TestMethod]
        public void TestInvalidLowerBound()
        {
            CovidLocationData covidLocationData = new CovidLocationData("GA");

            Assert.ThrowsException<ArgumentNullException>(() =>
                covidLocationData.GetEventsFromMonth(0));
        }

        [TestMethod]
        public void TestInvalidUpperBound()
        {
            CovidLocationData covidLocationData = new CovidLocationData("GA");

            Assert.ThrowsException<ArgumentNullException>(() =>
                covidLocationData.GetEventsFromMonth(13));
        }

        [TestMethod]
        public void TestZeroEventsFromMonth()
        {
            CovidLocationData covidLocationData = new CovidLocationData("GA");

            int expected = 0;
            int result = covidLocationData.GetEventsFromMonth(1).Count;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestOneEventsFromMonth()
        {
            CovidLocationData covidLocationData = new CovidLocationData("GA");

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2015, 1, 15))
            {
                HospitalizedCurrently = 0
            });

            covidLocationData.AddCovidCase(new CovidCase("GA", new DateTime(2016, 2, 15))
            {
                HospitalizedCurrently = 5
            });

            int expected = 1;
            int result = covidLocationData.GetEventsFromMonth(1).Count;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestManyEventsFromMonth()
        {
            CovidLocationData covidLocationData = new CovidLocationData("GA");

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

            int expected = 3;
            int result = covidLocationData.GetEventsFromMonth(1).Count;
            Assert.AreEqual(expected, result);
        }
    }
}
