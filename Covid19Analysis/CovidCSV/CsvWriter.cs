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
        public void SaveDataAsCsv(CovidLocationDataCollection dataToSave)
        {
            var locationCollection = dataToSave.CollectionOfCovidLocationData;
            string data = $"{CsvConstants.HeaderInformation} {Environment.NewLine}";

            foreach (KeyValuePair<string, CovidLocationData> currentLocation in locationCollection)
            {
                var locationData = currentLocation.Value.CovidCases;
                data += this.extractDataFromLocation(locationData);
            }

            this.saveData(data);
        }

        private string extractDataFromLocation(IList<CovidCase> covidLocation)
        {
            string data = "";

            foreach (var currentData in covidLocation)
            {
                data += $"{currentData.Date.ToString(CsvConstants.DateFormat)},{currentData.Location},{currentData.PositiveIncrease},{currentData.NegativeIncrease},{currentData.DeathIncrease},{currentData.HospitalizedIncrease}{Environment.NewLine}";
            }

            return data;
        }

        private async void saveData(string dataToSave)
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Comma Separated Value", new List<string>() { ".csv" });
            savePicker.SuggestedFileName = "New Document";

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);

                await FileIO.WriteTextAsync(file, dataToSave);

                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                if (status == FileUpdateStatus.Complete)
                {
                    this.displaySaveSuccessfulDialog();
                }
                else
                {
                    this.displaySaveUnsuccessfulDialog();
                }
            }
        }

        private async void displaySaveSuccessfulDialog()
        {
            var saveDataDialog = new ContentDialog()
            {
                Title = "Save Successful",
                Content = "File has been saved successfully",
                PrimaryButtonText = "Ok",
            };
            await saveDataDialog.ShowAsync();
        }

        private async void displaySaveUnsuccessfulDialog()
        {
            var saveDataDialog = new ContentDialog()
            {
                Title = "Save Unsuccessful",
                Content = "File has NOT been saved successfully",
                PrimaryButtonText = "Ok",
            };
            await saveDataDialog.ShowAsync();
        }

    }
}
