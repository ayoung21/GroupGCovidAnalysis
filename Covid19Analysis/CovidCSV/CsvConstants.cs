namespace Covid19Analysis.CovidCSV
{
    /// <summary>
    ///     Commonly used constants within CSVReader and CSVWriter
    /// </summary>
    public class CsvConstants
    {

        /// <summary>The date format</summary>
        public const string DateFormat = "yyyyMMdd";
        /// <summary>The header information for when saving a CSV file</summary>
        public const string HeaderInformation = "date,state,positiveIncrease,negativeIncrease,deathIncrease,hospitalizedIncrease";
    }
}
