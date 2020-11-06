using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Covid19Analysis.Model;

namespace Covid19Analysis.IO
{
    public class SummaryStatisticsFormatter
    {

        private CovidLocationData location;

        public SummaryStatisticsFormatter(CovidLocationData stateData)
        {
            this.location = stateData ?? throw new NullReferenceException(nameof(stateData));
        }

        /// <summary>
        ///     Gets the LocationData summary.
        /// </summary>
        /// <returns>A summary of the LocationData</returns>
        public string GetLocationSummary(int lowerThreshold, int upperThreshold, int binSize)
        {
            var output = "";

            output += $"{this.location.State}{Environment.NewLine}";
            output += $"{this.getEarliestKnownPositiveTestStatement()} {Environment.NewLine}";
            output +=
                $"{this.getHighestNumberOfPositiveTestsStatement(this.location.CovidCases)} {Environment.NewLine}";
            output += $"{this.getHighestNumberOfNegativeTestsStatement()} {Environment.NewLine}";
            output +=
                $"{this.getHighestNumberOfTestsOfAGivenDayStatement(this.location.CovidCases)} {Environment.NewLine}";
            output += $"{this.getHighestNumberOfDeathsStatement()} {Environment.NewLine}";
            output += $"{this.getHighestNumberOfHospitalizationsStatement()} {Environment.NewLine}";
            output += $"{this.getAverageOfCurrentHospitalizationsStatement():N2} {Environment.NewLine}";
            output += $"{this.getHighestPercentageOfPositiveTestsStatement()} {Environment.NewLine}";
            output += $"{this.getAverageOfPositiveTestsSinceFirstPositiveCaseStatement()} {Environment.NewLine}";
            output += $"{this.getOverallPositivityRatesStatement()} {Environment.NewLine}";
            output +=
                $"{this.getNumberOfDaysWherePositiveTestsAreAboveStatement(lowerThreshold)} {Environment.NewLine}";
            output +=
                $"{this.getNumberOfDaysWherePositiveTestsAreBelowStatement(upperThreshold)} {Environment.NewLine}";

            output += $"{Environment.NewLine}{this.buildHistogramOfPositiveCases(binSize)} {Environment.NewLine}";

            return output;
        }

        private object getAverageOfCurrentHospitalizationsStatement()
        {
            var averageCurrentHospitalizations =
                //this.LocationData.GetAverageCurrentHospitalizations(this.LocationData.CovidCases);
                this.location.GetAverageCurrentHospitalizations(this.location.CovidCases);
            return $"Average Current Hospitalizations: {averageCurrentHospitalizations}";
        }

        private string getEarliestKnownPositiveTestStatement()
        {
            var earliestCovidCase = this.location.GetEarliestPositiveCase();
            var date = earliestCovidCase.Date;
            var numberOfPositiveTests = earliestCovidCase.PositiveIncrease;
            return
                $"Earliest known positive case occurred on [{date:MMMM dd yyyy}] with {numberOfPositiveTests:N0} positive tests.";
        }

        private string getHighestNumberOfPositiveTestsStatement(IList<CovidCase> covidCases)
        {
            var highestPositiveTests = this.location.GetHighestNumberOfPositiveTests(covidCases);
            var date = highestPositiveTests.Date;
            var numberOfPositiveTests = highestPositiveTests.PositiveIncrease;
            return
                $"Highest number of positive tests occurred on [{date:MMMM dd yyyy}] with {numberOfPositiveTests:N0} positive tests.";
        }

        private string getHighestNumberOfNegativeTestsStatement()
        {
            var highestNegativeTests = this.location.GetHighestNumberOfNegativeIncreases();
            var date = highestNegativeTests.Date;
            var numberOfPositiveTests = highestNegativeTests.NegativeIncrease;
            return
                $"Highest number of negative tests occurred on [{date:MMMM dd yyyy}] with {numberOfPositiveTests:N0} negative tests.";
        }

        private string getHighestNumberOfTestsOfAGivenDayStatement(IList<CovidCase> covidCases)
        {
            var highestNumberOfTests = this.location.GetHighestTotalTestsData(covidCases);
            var date = highestNumberOfTests.Date;
            var totalTests = highestNumberOfTests.PositiveIncrease + highestNumberOfTests.NegativeIncrease;
            return $"Highest number of total tests occurred on [{date:MMMM dd yyyy}] with {totalTests:N0} tests.";
        }

        private string getHighestNumberOfDeathsStatement()
        {
            var highestNumberOfDeaths = this.location.GetHighestDeathsEvent();
            var date = highestNumberOfDeaths.Date;
            var deaths = highestNumberOfDeaths.DeathIncrease;
            return $"Highest number of deaths occurred on [{date:MMMM dd yyyy}] with {deaths:N0} deaths.";
        }

        private string getHighestNumberOfHospitalizationsStatement()
        {
            var highestNumberOfHospitalizations = this.location.GetHighestHospitalization();
            var date = highestNumberOfHospitalizations.Date;
            var hospitalizations = highestNumberOfHospitalizations.HospitalizedIncrease;
            return
                $"Highest number of hospitalizations occurred on [{date:MMMM dd yyyy}] with {hospitalizations:N0} hospitalizations.";
        }

        private string getHighestPercentageOfPositiveTestsStatement()
        {
            var highestPositivePercentageData = this.location.GetHighestPercentageOfPositiveTests();
            var percent = this.location.PositiveTestPercent(highestPositivePercentageData);
            var date = highestPositivePercentageData.Date;
            return $"Highest percentage of positive tests occurred on [{date:MMMM dd yyyy}] at {percent:N2}%";
        }

        private string getAverageOfPositiveTestsSinceFirstPositiveCaseStatement()
        {
            var averageOfPositiveTests = this.location.GetAverageNumberOfPositiveTests(this.location.CovidCases);
            return $"Average number of positive tests: {averageOfPositiveTests:N2}";
        }

        private string getOverallPositivityRatesStatement()
        {
            var overallPositivityRate = this.location.GetOverallPositivityRate(this.location.CovidCases);
            return $"Overall positivity rate of all tests: {overallPositivityRate:N2}%";
        }

        private string getNumberOfDaysWherePositiveTestsAreAboveStatement(int numberOfPositiveTests)
        {
            return
                $"Number of days with Positive Tests > {numberOfPositiveTests:N0}: {this.location.GetNumberOfDaysWherePositiveTestsAreAbove(numberOfPositiveTests)}";
        }

        private string getNumberOfDaysWherePositiveTestsAreBelowStatement(int numberOfPositiveTests)
        {
            return
                $"Number of days with positive tests < {numberOfPositiveTests:N0}: {this.location.GetNumberOfDaysWherePositiveTestsAreBelow(numberOfPositiveTests)}";
        }

        private string buildHistogramOfPositiveCases(int binSize)
        {
            var startingPoint = 0;
            var output = $"HISTOGRAM of Positive Tests{Environment.NewLine}";
            //var highestNumberOfCases = this.LocationData.GetHighestNumberOfPositiveTests(this.location.CovidCases);
            var highestNumberOfCases = this.location.GetHighestNumberOfPositiveTests(this.location.CovidCases);
            var highestPositiveTests = highestNumberOfCases.PositiveIncrease;

            for (var i = startingPoint; i < highestPositiveTests; i += binSize)
            {
                var numberOfCases = this.location.NumberOfPositiveCasesBetween(i, i + binSize);
                output += $"{i} - {i + binSize}: {numberOfCases} {Environment.NewLine}";
            }

            return output;
        }
    }
}
