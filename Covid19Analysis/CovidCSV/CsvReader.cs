using Covid19Analysis.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Covid19Analysis.CovidCSV
{
    /// <summary>
    ///     Reads CSV file and extracts information
    /// </summary>
    public class CsvReader
    {
        #region Constants
        private const int DateColumn = 0;
        private const int LocationColumn = 1;
        private const int PositiveColumn = 2;
        private const int NegativeColumn = 3;
        private const int HospitalizedCurrentlyColumn = 4;
        private const int HospitalizedIncreaseColumn = 5;
        private const int DeathColumn = 6;

        private const char DefaultDelimiter = ',';
        #endregion

        #region Data members
        private StorageFile csvFile;
        #endregion

        #region Properties

        /// <summary>Gets or sets the CSV file.</summary>
        /// <value>The CSV file.</value>
        /// <exception cref="ArgumentNullException">csvFile cannot be null</exception>
        public StorageFile CsvFile
        {
            get => this.csvFile;
            set
            {
                this.Errors.Clear();
                this.csvFile = value ?? throw new ArgumentNullException(nameof(this.csvFile));
            }
        }

        /// <summary>Get all lines or errors if any.</summary>
        /// <value>The errors.</value>
        public IList<string> Errors { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CsvReader" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">csvFile cannot be null</exception>
        public CsvReader()
        {
            this.Errors = new List<string>();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Parses the current CSV that has been loaded in.
        /// </summary>
        /// <returns>Collection of CovidCases</returns>
        public async Task<List<CovidCase>> Parse()
        {
            var covidCollection = new List<CovidCase>();
            var buffer = await FileIO.ReadBufferAsync(this.CsvFile);
            using (var dataReader = DataReader.FromBuffer(buffer))
            {
                var content = dataReader.ReadString(buffer.Length);
                var data = content.Split(Environment.NewLine);

                var count = 0;
                foreach (var record in data)
                {
                    if (count != 0 && count < data.Length - 1)
                    {
                        var stateData = record.Split(DefaultDelimiter);
                        var covidData = this.processCovidData(count, stateData);
                        if (covidData != null)
                        {
                            covidCollection.Add(covidData);
                        }
                    }

                    count++;
                }
            }

            return covidCollection;
        }

        /// <summary>Gets the errors as string.</summary>
        /// <returns>
        ///   A string of all errors.
        /// </returns>
        public string GetErrorsAsString()
        {
            var errors = "";
            foreach (var currentError in this.Errors)
            {
                errors += currentError + Environment.NewLine;
            }

            return errors;
        }

        private CovidCase processCovidData(int row, IReadOnlyList<string> data)
        {
            if (this.isValid(data))
            {
                return createCovidCase(data);
            }

            this.createErrorMessage(row, data);
            return null;

        }

        private void createErrorMessage(int row, IEnumerable<string> data)
        {
            var line = "";
            foreach (var item in data)
            {
                if (string.IsNullOrEmpty(item))
                {
                    line += " __ ";
                }
                else
                {
                    line += item + " ";
                }
            }
            this.Errors.Add($"ERROR: Invalid Row [{row}] - {line}");
        }

        private static CovidCase createCovidCase(IReadOnlyList<string> data)
        {
            try
            {
                var dateTime = DateTime.ParseExact(data[DateColumn], CsvConstants.DateFormat, CultureInfo.InvariantCulture);
                var state = data[LocationColumn];
                var positiveIncrease = getNumericValue(data[PositiveColumn]);
                var negativeIncrease = getNumericValue(data[NegativeColumn]);
                var deathIncrease = getNumericValue(data[DeathColumn]);
                var hospitalizedIncrease = getNumericValue(data[HospitalizedIncreaseColumn]);
                var hospitalizedCurrently = getNumericValue(data[HospitalizedCurrentlyColumn]);

                var covidCase = new CovidCase(state, dateTime)
                {
                    PositiveIncrease = positiveIncrease,
                    NegativeIncrease = negativeIncrease,
                    DeathIncrease = deathIncrease,
                    HospitalizedIncrease = hospitalizedIncrease,
                    HospitalizedCurrently = hospitalizedCurrently
                };

                return covidCase;
            }
            catch (Exception e)
            {
                throw new Exception($"Error parsing data: {e}");
            }
        }

        private bool isValid(IReadOnlyList<string> data)
        {
            // var validFields = containsValidFields(data);
            var validDate = this.containsValidDate(data[DateColumn]);
            var validPositive = this.containsValidNumber(data[PositiveColumn]);
            var validNegative = this.containsValidNumber(data[NegativeColumn]);
            var validHospitalizedCurrently = this.containsValidNumber(data[HospitalizedCurrentlyColumn]);
            var validHospitalizedIncrease = this.containsValidNumber(data[HospitalizedIncreaseColumn]);
            var validDeath = this.containsValidNumber(data[DeathColumn]);

            var result = validDate && validPositive && validNegative && validDeath && validHospitalizedIncrease && validHospitalizedCurrently;

            return result;
        }

        private bool containsValidFields(IEnumerable<string> data)
        {
            var validFields = true;
            foreach (var item in data)
            {
                if (string.IsNullOrEmpty(item))
                {
                    validFields = false;
                }
            }

            return validFields;
        }

        private bool containsValidDate(string data)
        {
            return DateTime.TryParseExact(data, CsvConstants.DateFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out _);
        }

        private bool containsValidNumber(string data)
        {
            return string.IsNullOrEmpty(data) || int.TryParse(data, out _);
        }

        private static int getNumericValue(string data)
        {
            return string.IsNullOrEmpty(data) || int.Parse(data) < 0 ? 0 : int.Parse(data);
        }

        #endregion
    }
}