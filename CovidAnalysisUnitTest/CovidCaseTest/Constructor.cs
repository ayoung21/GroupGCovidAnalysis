
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace CovidAnalysisUnitTest
{
    [TestClass]
    public class CovidCaseTest
    {
        private const string DateFormat = "yyyyMMdd";

        [TestMethod]
        public void TestConstructor()
        {
            var date = DateTime.ParseExact("20200816", DateFormat, CultureInfo.InvariantCulture);
            var covidCase = new CovidCase("GA", date);
            var expected = $"8/16/2020 GA : [+Increase] 0 [-increase] 0 [death increase] 0 [hospitalized increase] 0{ Environment.NewLine}";
            var result = covidCase.ToString();

            Assert.AreEqual(expected, result, true);
        }
    }
}
