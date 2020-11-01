using System;
using System.Collections.Generic;
using System.Linq;

namespace Covid19Analysis.Model
{
    /// <summary>
    ///     Stores the data for a given LocationData / territory.
    /// </summary>
    public class CovidLocationData
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the LocationData.
        /// </summary>
        /// <value>
        ///     The state to be set.
        /// </value>
        public string State { get; set; }

        /// <summary>Gets the duplicate cases.</summary>
        /// <value>The duplicate cases.</value>
        public IList<CovidCase> DuplicateCases => this.duplicateCases;

        /// <summary>
        ///     Gets all cases for this state / territory.
        /// </summary>
        /// <returns>A collection of covid cases for this state / territory</returns>
        public IList<CovidCase> CovidCases { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CovidLocationData" /> class.
        /// </summary>
        /// <param name="state">The state / territory of the data</param>
        /// <exception cref="ArgumentNullException">state cannot be null</exception>
        public CovidLocationData(string state)
        {
            this.State = state ?? throw new ArgumentNullException(nameof(state));
            this.CovidCases = new List<CovidCase>();
            this.duplicateCases = new List<CovidCase>();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds a single covid case to the collection
        /// </summary>
        /// <param name="covidCase">The covid case to be added</param>
        /// <exception cref="ArgumentNullException">covidCase cannot be null</exception>
        public void AddCovidCase(CovidCase covidCase)
        {
            if (covidCase == null)
            {
                throw new ArgumentNullException(nameof(covidCase));
            }

            if (this.CovidCases.Any(covid => covid.Date.Equals(covidCase.Date)))
            {
                this.duplicateCases.Add(covidCase);
            }
            else
            {
                this.CovidCases.Add(covidCase);
                this.CovidCases = this.SortData();
            }
        }

        /// <summary>
        ///     Gets the earliest positive case for this LocationData / territory
        /// </summary>
        /// <returns>A single covid case of the earliest positive case</returns>
        public CovidCase GetEarliestPositiveCase()
        {
            if (this.CovidCases.Count == 0)
            {
                return null;
            }

            return this.CovidCases.OrderBy(covidCase => covidCase.Date)
                       .First(covidCase => covidCase.PositiveIncrease > 0);
        }

        /// <summary>
        ///     Gets the highest number of negative increases.
        /// </summary>
        /// <returns>A CovidCase with the highest number of negative tests</returns>
        public CovidCase GetHighestNumberOfNegativeIncreases()
        {
            if (this.CovidCases.Count == 0)
            {
                return null;
            }

            return this.CovidCases.OrderByDescending(covidCase => covidCase.NegativeIncrease).First();
        }

        /// <summary>
        ///     Sorts the data by date (earliest first).
        /// </summary>
        /// <returns>the original data in order by date (earliest first)</returns>
        public IList<CovidCase> SortData()
        {
            return this.CovidCases.OrderBy(x => x.Date).ToList();
        }

        /// <summary>
        ///     Gets the number of days where positive tests are above a specified amount.
        /// </summary>
        /// <param name="numberOfPositiveTests">The number of positive tests.</param>
        /// <returns>
        ///     <br />
        /// </returns>
        public int GetNumberOfDaysWherePositiveTestsAreAbove(int numberOfPositiveTests)
        {
            var positiveTests = this.CovidCases.Count(data => data.PositiveIncrease > numberOfPositiveTests);

            return positiveTests;
        }

        /// <summary>
        ///     Gets the number of days where positive tests are below a specified amount.
        /// </summary>
        /// <param name="numberOfPositiveTests">The number of positive tests.</param>
        /// <returns>
        ///     <br />
        /// </returns>
        public int GetNumberOfDaysWherePositiveTestsAreBelow(int numberOfPositiveTests)
        {
            var earliestCovidCase = this.GetEarliestPositiveCase();

            var positiveTests = this.CovidCases.Where(data => data.Date >= earliestCovidCase.Date).Count(data => data.PositiveIncrease < numberOfPositiveTests);


            return positiveTests;
        }

        /// <summary>
        ///     Gets the overall positivity rate.
        /// </summary>
        /// <param name="covidCases">Collection of covid cases</param>
        /// <returns>The overall positivity rate</returns>
        public double GetOverallPositivityRate(IList<CovidCase> covidCases)
        {
            if (covidCases.Count == 0)
            {
                return 0.00;
            }

            double totalPositiveTests = this.totalPositiveTests(covidCases);
            double totalNegativeTests = this.totalNegativeTests(covidCases);

            var totalCount = totalPositiveTests + totalNegativeTests;

            return totalCount != 0 ? totalPositiveTests / (totalNegativeTests + totalPositiveTests) * 100 : 0;
        }

        private int totalPositiveTests(IList<CovidCase> covidCases)
        {
            if (covidCases == null)
            {
                throw new ArgumentOutOfRangeException(nameof(covidCases));
            }

            if (covidCases.Count == 0)
            {
                throw new IndexOutOfRangeException(nameof(covidCases));
            }

            var totalPositiveTests = covidCases.Sum(data => data.PositiveIncrease);

            return totalPositiveTests;
        }

        private int totalNegativeTests(IList<CovidCase> covidCases)
        {
            if (covidCases == null)
            {
                throw new ArgumentOutOfRangeException(nameof(covidCases));
            }

            if (covidCases.Count == 0)
            {
                throw new IndexOutOfRangeException(nameof(covidCases));
            }

            var totalNegativeTests = covidCases.Sum(data => data.NegativeIncrease);

            return totalNegativeTests;
        }

        /// <summary>
        ///     Gets the average number of positive tests.
        /// </summary>
        /// <param name="covidCases">The collection of covid cases.</param>
        /// <returns></returns>
        public double GetAverageNumberOfPositiveTests(IList<CovidCase> covidCases)
        {
            double positiveTestCount = 0.0;

            if (this.totalPositiveTests(covidCases) != 0)
            {
                positiveTestCount = covidCases.Where(data => data.Date >= this.GetEarliestPositiveCase().Date)
                                              .Average(data => data.PositiveIncrease);
            }

            return positiveTestCount;
        }

        public double GetAverageCurrentHospitalizations(IList<CovidCase> covidCases)
        {
            return covidCases.Average(data => data.HospitalizedCurrently);
        }

        /// <summary>
        ///     Gets the average number of all tests.
        /// </summary>
        /// <param name="covidCases">The covid cases.</param>
        /// <returns>The average number of all tests.</returns>
        public double GetAverageNumberOfAllTests(IList<CovidCase> covidCases)
        {
            var average = covidCases.Average(data => this.totalDailyTests(data));

            return average;
        }

        private int totalDailyTests(CovidCase theData)
        {
            if (theData == null)
            {
                throw new ArgumentOutOfRangeException(nameof(theData));
            }

            return theData.PositiveIncrease + theData.NegativeIncrease;
        }

        /// <summary>
        ///     Gets the highest number of tests on a given day.
        /// </summary>
        /// <param name="covidCases">The covid cases.</param>
        /// <returns>CovidCase with the highest test on a given day</returns>
        public CovidCase GetHighestTotalTestsData(IList<CovidCase> covidCases)
        {
            if (covidCases == null)
            {
                throw new ArgumentOutOfRangeException(nameof(covidCases));
            }
            var orderedCollection = covidCases.OrderBy(this.totalDailyTests);
            var highestNumberOfTests = orderedCollection.Last();

            return highestNumberOfTests;
        }

        /// <summary>
        ///     Gets the highest deaths event.
        /// </summary>
        /// <returns>CovidCase with the highest death on a single day.</returns>
        public CovidCase GetHighestDeathsEvent()
        {
            if (this.CovidCases.Count == 0)
            {
                return null;
            }

            return this.CovidCases.OrderByDescending(covidCase => covidCase.DeathIncrease).First();
        }

        /// <summary>
        ///     Gets the CovidCase with the highest hospitalization.
        /// </summary>
        /// <returns>CovidCase with the highest hospitalizations</returns>
        public CovidCase GetHighestHospitalization()
        {
            if (this.CovidCases.Count == 0)
            {
                return null;
            }

            return this.CovidCases.OrderByDescending(covidCase => covidCase.HospitalizedIncrease).First();
        }

        /// <summary>
        ///     Gets the highest percentage of positive tests event.
        /// </summary>
        /// <returns>CovidCase with the highest percentage of positive tests.</returns>
        public CovidCase GetHighestPercentageOfPositiveTests()
        {
            if (this.CovidCases.Count == 0)
            {
                return null;
            }

            var orderCollection = this.CovidCases.OrderBy(this.PositiveTestPercent);
            var highestPercentPositive = orderCollection.Last();

            return highestPercentPositive;
        }

        /// <summary>
        ///     Calculates the percent of positive covid tests for a covid object.
        /// </summary>
        /// <param name="theData">The data.</param>
        /// <returns>
        ///     The percent of positive covid test for a covidData object
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">theData</exception>
        public double PositiveTestPercent(CovidCase theData)
        {
            if (theData == null)
            {
                throw new ArgumentOutOfRangeException(nameof(theData));
            }

            double totalTests = this.totalDailyTests(theData);
            var percent = 0.0;
            if (totalTests != 0)
            {
                percent = theData.PositiveIncrease / totalTests;
            }

            return percent * 100;
        }



        /// <summary>
        ///     Gets the events from a given month.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <returns>CovidCases of a given month.</returns>
        public IList<CovidCase> GetEventsFromMonth(int month)
        {
            if (month < 1 || month > 12)
            {
                throw new Exception("Invalid Month");
            }

            var covidEvents = new List<CovidCase>();
            foreach (var covidCase in this.CovidCases)
            {
                if (covidCase.Date.Month == month)
                {
                    covidEvents.Add(covidCase);
                }
            }

            return covidEvents;
        }

        /// <summary>
        ///     Gets the lowest number of total tests.
        /// </summary>
        /// <param name="covidCases">The covid cases.</param>
        /// <returns>CovidCase with the lowest number of total tests</returns>
        public CovidCase GetLowestNumberOfTotalTests(IList<CovidCase> covidCases)
        {
            if (covidCases.Count == 0)
            {
                return null;
            }

            var lowestNumberOfTests = covidCases[0];
            foreach (var covidCase in covidCases)
            {
                if (covidCase.PositiveIncrease < lowestNumberOfTests.PositiveIncrease)
                {
                    lowestNumberOfTests = covidCase;
                }
            }

            return lowestNumberOfTests;
        }

        /// <summary>
        ///     Gets the highest number of positive tests.
        /// </summary>
        /// <param name="covidCases">The covid cases.</param>
        /// <returns>CovidCase with the highest number of positive tests.</returns>
        public CovidCase GetHighestNumberOfPositiveTests(IList<CovidCase> covidCases)
        {
            if (covidCases.Count == 0)
            {
                return null;
            }

            return covidCases.OrderByDescending(covidCase => covidCase.PositiveIncrease).First();
        }

        /// <summary>
        ///     Gets the lowest number of positive tests.
        /// </summary>
        /// <param name="covidCases">The covid cases.</param>
        /// <returns>CovidCase with the lowest number of positive.</returns>
        public CovidCase GetLowestNumberOfPositiveTests(IList<CovidCase> covidCases)
        {
            if (covidCases.Count == 0)
            {
                return null;
            }

            return this.CovidCases.OrderBy(covidCase => covidCase.PositiveIncrease).First();
        }

        /// <summary>
        ///     Finds and Replaces a covid case with the exact date.
        /// </summary>
        public void FindAndReplace(CovidCase covidCase)
        {
            var item = this.CovidCases.First(i => i.Date.Equals(covidCase.Date));
            var index = this.CovidCases.IndexOf(item);
            if (index != -1)
            {
                this.CovidCases.RemoveAt(index);
                this.CovidCases.Add(covidCase);
            }
        }

        /// <summary>
        ///     Clears all data in the collection.
        /// </summary>
        public void ClearData()
        {
            this.CovidCases.Clear();
        }

        /// <summary>The number of positive cases between the given parameters.</summary>
        /// <param name="minTestCount">The minimum test count.</param>
        /// <param name="maxTestCount">The maximum test count.</param>
        /// <returns>
        ///     The number of positive cases between the given parameters.
        /// </returns>
        public int NumberOfPositiveCasesBetween(int minTestCount, int maxTestCount)
        {
            return this.CovidCases
                       .Where(covidCase => covidCase.PositiveIncrease >= minTestCount)
                       .Where(covidCase => covidCase.PositiveIncrease <= maxTestCount).Count(covidCase =>
                           covidCase.Date >= this.GetEarliestPositiveCase().Date);
        }
        #endregion

        #region Member Variables

        // private readonly IList<CovidCase> locationsCovidCases;
        private readonly List<CovidCase> duplicateCases;

        #endregion
    }
}