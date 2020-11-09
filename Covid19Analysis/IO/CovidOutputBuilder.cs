using Covid19Analysis.Model;
using System;
using System.Collections.Generic;

namespace Covid19Analysis.IO
{
    /// <summary>
    ///     Report class to showcase data at a given location.
    /// </summary>
    public class CovidOutputBuilder
    {
        #region Data members

        private CovidLocationData location;

        private readonly SummaryStatisticsFormatter totalStatisticsFormatter;
        private readonly MonthlyStatisticsFormatter monthlyStatisticsFormatter;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the LocationData for the report.
        /// </summary>
        /// <value>
        ///     The LocationData.
        /// </value>
        /// <exception cref="NullReferenceException">value cannot be null</exception>
        public CovidLocationData LocationData
        {
            get => this.location;
            set => this.location = value ?? throw new NullReferenceException(nameof(value));
        }

        /// <summary>
        ///     Gets or sets the upper threshold for positive cases.
        /// </summary>
        /// <value>The upper threshold.</value>
        public int UpperThreshold { get; set; }

        /// <summary>
        ///     Gets or sets the lower threshold for positive cases.
        /// </summary>
        /// <value>The lower threshold.</value>
        public int LowerThreshold { get; set; }

        /// <summary>
        ///     Gets or sets the bin size for the histogram.
        /// </summary>
        /// <value>The lower threshold.</value>
        public int BinSize { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CovidOutputBuilder" /> class.
        /// </summary>
        /// <param name="stateData">The location data.</param>
        /// <exception cref="NullReferenceException">stateData cannot be null</exception>
        public CovidOutputBuilder(CovidLocationData stateData)
        {
            this.location = stateData ?? throw new NullReferenceException(nameof(stateData));
            this.totalStatisticsFormatter = new SummaryStatisticsFormatter(stateData);
            this.monthlyStatisticsFormatter = new MonthlyStatisticsFormatter(stateData);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the Covid the output.
        /// </summary>
        /// <returns>
        /// The output to be displayed
        /// </returns>
        public string CovidOutput()
        {
            return this.totalStatisticsFormatter.GetLocationSummary(this.LowerThreshold, this.UpperThreshold, this.BinSize) +
                   this.monthlyStatisticsFormatter.GetYearlySummary();
        }

        /// <summary>
        ///     Gets the LocationData summary.
        /// </summary>
        /// <returns>A summary of the LocationData</returns>
        public string GetLocationSummary()
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
                $"{this.getNumberOfDaysWherePositiveTestsAreAboveStatement(this.LowerThreshold)} {Environment.NewLine}";
            output +=
                $"{this.getNumberOfDaysWherePositiveTestsAreBelowStatement(this.UpperThreshold)} {Environment.NewLine}";

            output += $"{Environment.NewLine}{this.buildHistogramOfPositiveCases()} {Environment.NewLine}";

            return output;
        }

        ///// <summary>
        /////     Gets the monthly summary of a given month.
        ///// </summary>
        ///// <param name="month">The month to generate a report for.</param>
        ///// <returns>The monthly summary of a given month.</returns>
        //public string GetMonthlySummary(int month)
        //{
        //    if (month < 1 || month > 12)
        //    {
        //        return "Invalid Month" + Environment.NewLine;
        //    }

        //    var covidEvents = this.location.GetEventsFromMonth(month);
        //    var output = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month) +
        //                 $" [{covidEvents.Count} days of data]" + Environment.NewLine;

        //    if (covidEvents.Count == 0)
        //    {
        //        return output;
        //    }

        //    var caseWithHighestPositiveTests = this.location.GetHighestNumberOfPositiveTests(covidEvents);
        //    var caseWithLowestPositiveTests = this.location.GetLowestNumberOfPositiveTests(covidEvents);
        //    var caseWithHighestTestCount = this.location.GetHighestTotalTestsData(covidEvents);
        //    var caseWithLowestTestCount = this.location.GetLowestNumberOfTotalTests(covidEvents);
        //    var averageOfPositiveTests = Math.Round(this.location.GetAverageNumberOfPositiveTests(covidEvents), 2);
        //    var averageOfTotalTests = Math.Round(this.location.GetAverageNumberOfAllTests(covidEvents), 2);
        //    var minOfCurrentHospitalizations = this.location.GetCurrentHospitalizationsMinimum(covidEvents);
        //    var maxOfCurrentHospitalizations = this.location.GetCurrentHospitalizationsMaximum(covidEvents);

        //    output +=
        //        $"Highest # of positive tests: {caseWithHighestPositiveTests.PositiveIncrease:N0} occurred on the {this.getDayWithSuffix(caseWithHighestPositiveTests.Date.Day)} {Environment.NewLine}";
        //    output +=
        //        $"Lowest # of positive tests: {caseWithLowestPositiveTests.PositiveIncrease:N0} occurred on the {this.getDayWithSuffix(caseWithLowestPositiveTests.Date.Day)} {Environment.NewLine}";
        //    output +=
        //        $"Highest # of total tests: {caseWithHighestTestCount.TotalTestCount:N0} occurred on the {this.getDayWithSuffix(caseWithHighestTestCount.Date.Day)} {Environment.NewLine}";
        //    output +=
        //        $"Lowest # of total tests: {caseWithLowestTestCount.TotalTestCount:N0} occurred on the {this.getDayWithSuffix(caseWithLowestTestCount.Date.Day)} {Environment.NewLine}";
        //    output += $"Average # of positive tests: {averageOfPositiveTests:N2} {Environment.NewLine}";
        //    output += $"Average # of total tests: {averageOfTotalTests:N2} {Environment.NewLine}";
        //    output += $"Min of Current Hospitalizations: {minOfCurrentHospitalizations} {Environment.NewLine}";
        //    output += $"Max of Current Hospitalizations: {maxOfCurrentHospitalizations} {Environment.NewLine}";

        //    return output;
        //}

        ///// <summary>
        /////     Gets the yearly summary of all months with Covid data.
        ///// </summary>
        ///// <returns>A yearly summary of the covid data for the current LocationData.</returns>
        //public string GetYearlySummary()
        //{
        //    var output = "";
        //    var startingMonth = this.LocationData.GetEarliestPositiveCase().Date.Month;
        //    for (var month = startingMonth; month <= 12; month++)
        //    {
        //        output += this.GetMonthlySummary(month) + Environment.NewLine;
        //    }

        //    return output;
        //}

        private object getAverageOfCurrentHospitalizationsStatement()
        {
            var averageCurrentHospitalizations =
                this.LocationData.GetAverageCurrentHospitalizations(this.LocationData.CovidCases);
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

        private string buildHistogramOfPositiveCases()
        {
            const int startingPoint = 0;
            var output = $"HISTOGRAM of Positive Tests{Environment.NewLine}";
            var highestNumberOfCases = this.LocationData.GetHighestNumberOfPositiveTests(this.location.CovidCases);
            var highestPositiveTests = highestNumberOfCases.PositiveIncrease;

            for (var i = startingPoint; i < highestPositiveTests; i += this.BinSize)
            {
                var numberOfCases = this.location.NumberOfPositiveCasesBetween(i, i + this.BinSize);
                output += $"{i} - {i + this.BinSize}: {numberOfCases} {Environment.NewLine}";
            }

            return output;
        }

        //private string getDayWithSuffix(int day)
        //{
        //    if (day < 1 || day > 31)
        //    {
        //        return "";
        //    }

        //    switch (day)
        //    {
        //        case 1:
        //        case 21:
        //        case 31:
        //            return $"{day}st";
        //        case 2:
        //        case 22:
        //            return $"{day}nd";
        //        case 3:
        //        case 23:
        //            return $"{day}rd";
        //        default:
        //            return $"{day}th";
        //    }
        //}

        #endregion
    }
}