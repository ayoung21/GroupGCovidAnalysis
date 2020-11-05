using Covid19Analysis.Model;
using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.Xaml.Controls;

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
        /// <param name="dataToSave">The data to save.</param>
        public async void SaveDataAsCsv(StorageFile file, CovidLocationDataCollection dataToSave)
        {
            var locationCollection = dataToSave.CollectionOfCovidLocationData;
            string data = $"{CsvConstants.HeaderInformation} {Environment.NewLine}";

            foreach (KeyValuePair<string, CovidLocationData> currentLocation in locationCollection)
            {
                var locationData = currentLocation.Value.CovidCases;
                data += this.extractDataFromLocation(locationData);
            }

            await FileIO.WriteTextAsync(file, data);
        }

        private string extractDataFromLocation(IList<CovidCase> covidLocation)
        {
            string data = "";

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
