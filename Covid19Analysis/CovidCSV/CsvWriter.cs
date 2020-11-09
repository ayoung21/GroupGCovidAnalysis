using Covid19Analysis.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;

namespace Covid19Analysis.CovidCSV
{
    /// <summary>
    ///     Class to save Covid-19 data to a CSV file
    /// </summary>
    public class CsvWriter
    {
        /// <summary>
        ///     Saves the data as CSV.
        /// </summary>
        /// <param name="file">The file</param>
        /// <param name="dataToSave">The data to save.</param>
        public async void SaveDataAsCsv(StorageFile file, CovidLocationDataCollection dataToSave)
        {
            var locationCollection = dataToSave.CollectionOfCovidLocationData;
            var data = $"{CsvConstants.HeaderInformation} {Environment.NewLine}";

            data = locationCollection.Select(currentLocation => currentLocation.Value.CovidCases).Aggregate(data, (current, locationData) => current + extractDataFromLocation(locationData));

            await FileIO.WriteTextAsync(file, data);
        }

        private static string extractDataFromLocation(IEnumerable<CovidCase> covidLocation)
        {
            var data = "";

            foreach (var currentData in covidLocation)
            {
                data += $"{currentData.Date.ToString(CsvConstants.DateFormat)},{currentData.Location},";
                data += $"{currentData.PositiveIncrease},{currentData.NegativeIncrease},";
                data += $"{currentData.HospitalizedCurrently},{currentData.HospitalizedIncrease},{currentData.DeathIncrease}{Environment.NewLine}";
            }

            return data;
        }
    }
}