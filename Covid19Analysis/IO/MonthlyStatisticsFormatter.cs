using Covid19Analysis.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Covid19Analysis.IO
{
    /// <summary>
    /// Formats the monthly statistics to be displayed
    /// </summary>
    public class MonthlyStatisticsFormatter
    {

        private readonly CovidLocationData location;

        /// <summary>Initializes a new instance of the <see cref="MonthlyStatisticsFormatter" /> class.</summary>
        /// <param name="stateData">The state data.</param>
        /// <exception cref="NullReferenceException">stateData</exception>
        public MonthlyStatisticsFormatter(CovidLocationData stateData)
        {
            this.location = stateData ?? throw new NullReferenceException(nameof(stateData));
        }

        /// <summary>
        ///     Gets the monthly summary of a given month.
        /// </summary>
        /// <param name="month">The month to generate a report for.</param>
        /// <returns>The monthly summary of a given month.</returns>
        public string GetMonthlySummary(int month)
        {
            if (month < 1 || month > 12)
            {
                return "Invalid Month" + Environment.NewLine;
            }

            var covidEvents = this.location.GetEventsFromMonth(month);
            var output = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month) +
                         $" [{covidEvents.Count} days of data]" + Environment.NewLine;

            if (covidEvents.Count == 0)
            {
                return output;
            }

            output += this.formatHighestPositiveTests(covidEvents);
            output += this.formatLowestPositiveTests(covidEvents);
            output += this.formatHighestTotalTests(covidEvents);
            output += this.formatLowestTotalTests(covidEvents);
            output += this.formatAveragePositiveTests(covidEvents);
            output += this.formatAverageTotalTests(covidEvents);
            output += this.formatMinCurrentHospitalizations(covidEvents);
            output += this.formatMaxCurrentHospitalizations(covidEvents);

            return output;
        }

        /// <summary>
        ///     Gets the yearly summary of all months with Covid data.
        /// </summary>
        /// <returns>A yearly summary of the covid data for the current LocationData.</returns>
        public string GetYearlySummary()
        {
            var output = "";
            //var startingMonth = this.LocationData.GetEarliestPositiveCase().Date.Month;
            var startingMonth = this.location.GetEarliestPositiveCase().Date.Month;
            var endMonth = this.getLastDayWithData().Date.Month;

            for (var month = startingMonth; month <= endMonth; month++)
            {
                output += this.GetMonthlySummary(month) + Environment.NewLine;
            }

            return output;
        }

        private CovidCase getLastDayWithData()
        {
            return this.location.CovidCases.OrderBy(covidCase => covidCase.Date).Last();
        }

        private static string getDayWithSuffix(int day)
        {
            if (day < 1 || day > 31)
            {
                return "";
            }

            switch (day)
            {
                case 1:
                case 21:
                case 31:
                    return $"{day}st";
                case 2:
                case 22:
                    return $"{day}nd";
                case 3:
                case 23:
                    return $"{day}rd";
                default:
                    return $"{day}th";
            }
        }

        private static string datesOfSpecificPositiveIncrease(IEnumerable<CovidCase> dataCollection, int positiveIncrease)
        {
            var dates = dataCollection.Where(data => data.PositiveIncrease == positiveIncrease).Reverse();
            var days = string.Empty;
            foreach (var data in dates)
            {
                days += $"{getDayWithSuffix(data.Date.Day)}, ";
            }

            return days;
        }

        private static string datesOfSpecificTotalIncrease(IEnumerable<CovidCase> dataCollection, int totalIncrease)
        {
            var dates = dataCollection
                        .Where(data => data.TotalTestCount == totalIncrease).Reverse();
            var days = string.Empty;
            foreach (var data in dates)
            {
                days += $"{getDayWithSuffix(data.Date.Day)}, ";
            }

            return days;
        }

        private string formatHighestPositiveTests(IList<CovidCase> covidEvents)
        {
            var highestTestCase = this.location.GetHighestNumberOfPositiveTests(covidEvents).PositiveIncrease;
            var dateOfHighestTest = datesOfSpecificPositiveIncrease(covidEvents, highestTestCase);

            return
                $"Highest # of positive tests: {highestTestCase:N0} occurred on the {dateOfHighestTest} {Environment.NewLine}";
        }

        private string formatLowestPositiveTests(IList<CovidCase> covidEvents)
        {
            var lowestTestCase = this.location.GetLowestNumberOfPositiveTests(covidEvents).PositiveIncrease;
            var dateOfLowestTest = datesOfSpecificPositiveIncrease(covidEvents, lowestTestCase);

            return $"Lowest # of positive tests: {lowestTestCase:N0} occurred on the {dateOfLowestTest} {Environment.NewLine}";
        }

        private string formatHighestTotalTests(IList<CovidCase> covidEvents)
        {
            var highestTotal = this.location.GetHighestTotalTestsData(covidEvents).TotalTestCount;
            var dateOfHighestTotal = datesOfSpecificTotalIncrease(covidEvents, highestTotal);

            return $"Highest # of total tests: {highestTotal:N0} occurred on the {dateOfHighestTotal} {Environment.NewLine}";
        }

        private string formatLowestTotalTests(IList<CovidCase> covidEvents)
        {
            var lowestTotal = this.location.GetLowestNumberOfTotalTests(covidEvents).TotalTestCount;
            var dateOfLowestTotal = datesOfSpecificTotalIncrease(covidEvents, lowestTotal);

            return $"Lowest # of total tests: {lowestTotal:N0} occurred on the {dateOfLowestTotal} {Environment.NewLine}";
        }

        private string formatAveragePositiveTests(IList<CovidCase> covidEvents)
        {
            var averageOfPositiveTests = Math.Round(this.location.GetAverageNumberOfPositiveTests(covidEvents), 2);

            return $"Average # of positive tests: {averageOfPositiveTests:N2} {Environment.NewLine}";
        }

        private string formatAverageTotalTests(IList<CovidCase> covidEvents)
        {
            var averageOfTotalTests = Math.Round(this.location.GetAverageNumberOfAllTests(covidEvents), 2);

            return $"Average # of total tests: {averageOfTotalTests:N2} {Environment.NewLine}";
        }

        private string formatMinCurrentHospitalizations(IList<CovidCase> covidEvents)
        {
            var minOfCurrentHospitalizations = this.location.GetCurrentHospitalizationsMinimum(covidEvents);

            return $"Min of Current Hospitalizations: {minOfCurrentHospitalizations} {Environment.NewLine}";
        }

        private string formatMaxCurrentHospitalizations(IList<CovidCase> covidEvents)
        {
            var maxOfCurrentHospitalizations = this.location.GetCurrentHospitalizationsMaximum(covidEvents);

            return $"Max of Current Hospitalizations: {maxOfCurrentHospitalizations} {Environment.NewLine}";
        }
    }
}
