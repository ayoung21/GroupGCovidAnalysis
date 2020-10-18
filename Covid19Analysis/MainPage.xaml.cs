using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Covid19Analysis.IO;
using Covid19Analysis.Model;
using Covid19Analysis.View;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Covid19Analysis
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Constants
        private const string LocationOfInterest = "GA";
        private const int LowerThresholdDefault = 0;
        private const int UpperThresholdDefault = 2500;
        #endregion

        #region Data members
        public int lowerThreshold = 666;
        public int upperThreshold = 2500;
        private readonly CsvReader csvReader;
        private readonly CovidLocationDataCollection covidCollection;
        private CovidLocationData covidLocationData;

        /// <summary>
        ///     The application height
        /// </summary>
        public const int ApplicationHeight = 750;

        /// <summary>
        ///     The application width
        /// </summary>
        public const int ApplicationWidth = 625;

        #endregion

        #region Properties
        /// <summary>
        ///     Gets or sets the current data file to analyze.
        /// </summary>
        /// <value>The current data file to analyze.</value>
        public StorageFile CurrentFile { get; set; }
        #endregion 

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size {Width = ApplicationWidth, Height = ApplicationHeight};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(ApplicationWidth, ApplicationHeight));

            this.csvReader = new CsvReader();
            this.covidCollection = new CovidLocationDataCollection();
            this.lowerThresholdTextBox.Text = LowerThresholdDefault.ToString();
            this.upperThresholdTextBox.Text = UpperThresholdDefault.ToString();

            this.comboboxState.ItemsSource = Enum.GetValues(typeof(UnitedStatesLocations)).Cast<UnitedStatesLocations>();
        }

        #endregion
        private async void displayErrors_Click(object sender, RoutedEventArgs e)
        {
            string defaultOutput = "No Known Errors";
            var errorDialog = new ContentDialog()
            {
                Title = "CSV Errors",
                Content = new ScrollViewer()
                {
                    Content = new TextBlock()
                    {
                        Text = (this.csvReader.Errors.Count > 0) ? this.csvReader.GetErrorsAsString() : defaultOutput
                    },
                },
                CloseButtonText = "ok"
            };

            await errorDialog.ShowAsync();
        }

        private async void loadFile_Click(object sender, RoutedEventArgs e)
        {
            this.lowerThreshold = int.Parse(this.lowerThresholdTextBox.Text);
            this.upperThreshold = int.Parse(this.upperThresholdTextBox.Text);

            var validThreshold = (this.lowerThreshold < this.upperThreshold);
            if (validThreshold)
            {
                if (this.CurrentFile == null)
                {
                    this.CurrentFile = await this.chooseFile();
                }
                else
                {
                    await this.promptDisplayOrMerge();
                }

                await this.extractData();
                await this.displayInformation();
            }
            else
            {
                this.displayDialogInvalidThreshold();
            }
        }

        private async Task promptDisplayOrMerge()
        {
            ContentDialogResult dialogResult = await this.displayDialogReplaceOrMergeCase();
            if (dialogResult == ContentDialogResult.Primary)
            {
                await this.loadFile();
            }
            else if (dialogResult == ContentDialogResult.Secondary)
            {
                this.covidCollection.ClearData();
                await this.loadFile();
            }
        }

        private async Task loadFile()
        {
            var fileToLoad = await this.chooseFile();
            if (fileToLoad != null)
            {
                this.CurrentFile = fileToLoad;
            }
        }

        private async Task<StorageFile> chooseFile()
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            picker.FileTypeFilter.Add(".csv");
            picker.FileTypeFilter.Add(".txt");

            return await picker.PickSingleFileAsync();
        }

        private async Task extractData()
        {
            if (this.CurrentFile != null)
            {
                csvReader.CsvFile = this.CurrentFile;
                IList<CovidCase> covidCases = await csvReader.Parse();
                this.covidCollection.AddAllCovidCases(covidCases);

                this.covidLocationData = this.covidCollection.GetLocationData(LocationOfInterest);
            }

            if (this.covidLocationData.DuplicateCases.Count > 0)
            {
                await this.displayDuplicateCases();
            }
        }

        private async Task displayDuplicateCases()
        {
            IList<CovidCase> tempList = new List<CovidCase>();

            if (this.covidLocationData == null || this.covidLocationData.DuplicateCases.Count == 0)
            {
                this.displayDialogNoDuplicateKeysFound();
            }

            if (this.covidLocationData != null && this.covidLocationData.DuplicateCases.Count > 0)
            {

                var skipOrReplaceDialog = new DuplicateEntryContentDialog();

                for (var i = 0; i < this.covidLocationData.DuplicateCases.Count; i++)
                {
                    skipOrReplaceDialog.Subtitle = $"There are {this.covidLocationData.DuplicateCases.Count - i} items with the same date";
                    skipOrReplaceDialog.Message = this.covidLocationData.DuplicateCases[i].ToString();
                    skipOrReplaceDialog.UpdateContent();

                    if (!skipOrReplaceDialog.IsChecked)
                    {
                        var result = await skipOrReplaceDialog.ShowAsync();

                        if (result == ContentDialogResult.Primary)
                        {
                            this.covidLocationData.FindAndReplace(this.covidLocationData.DuplicateCases[i]);
                            tempList.Add(this.covidLocationData.DuplicateCases[i]);
                        }
                    }
                    else if (skipOrReplaceDialog.IsChecked && skipOrReplaceDialog.LastKnownButtonPress == "Primary")
                    {
                        this.covidLocationData.FindAndReplace(this.covidLocationData.DuplicateCases[i]);
                        tempList.Add(this.covidLocationData.DuplicateCases[i]);
                    }
                }

                var itemsRemoved = this.removeDuplicateItems(tempList);
                tempList.Clear();
                if (itemsRemoved > 0)
                {
                    this.promptItemsHaveBeenReplaced(itemsRemoved);
                }
            }
        }

        private async Task displayInformation()
        {
            // this.covidLocationData = this.covidCollection.GetLocationData(LocationOfInterest);

            if (this.CurrentFile != null || this.covidLocationData != null)
            {
                this.buildAndSetSummaryReport();
            }
            else
            {
                this.summaryTextBox.Text = "No file loaded...";
            }
        }

        private void buildAndSetSummaryReport()
        {
            try
            {
                this.summaryTextBox.Text = "Loading...";

                if (this.covidLocationData != null)
                {
                    CovidOutputBuilder report = new CovidOutputBuilder(this.covidLocationData) {
                        LowerThreshold = this.lowerThreshold,
                        UpperThreshold = this.upperThreshold
                    };
                    this.summaryTextBox.Text = report.GetLocationSummary() + report.GetYearlySummary();
                }
                else
                {
                    this.summaryTextBox.Text = "No data found for the requested location.";
                }
            }
            catch (Exception)
            {
                var message =
                    "Invalid File. Please make sure you have chosen the correct file or ensure the file is in the proper format.";
                this.summaryTextBox.Text = message;
            }
        }

        private async void promptItemsHaveBeenReplaced(int itemsRemoved)
        {
            var dialogBox = new ContentDialog()
            {
                Title = "Items Replaced",
                Content = $"{itemsRemoved} items have been replaced",
                PrimaryButtonText = "Yes!",
            };
            await dialogBox.ShowAsync();
            await this.displayInformation();
        }

        private int removeDuplicateItems(IList<CovidCase> tempList)
        {

            var count = 0;
            foreach (var currentCase in tempList)
            {
                
                var item = this.covidLocationData.DuplicateCases.First(i => i.Date.Equals(currentCase.Date));
                var index = this.covidLocationData.DuplicateCases.IndexOf(item);

                if (index == -1) continue;
                this.covidLocationData.DuplicateCases.RemoveAt(index);

                count++;
            }

            return count;
        }

        private void TextBox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private async void displayDialogInvalidThreshold()
        {
            var invalidThresholdDialog = new ContentDialog()
            {
                Title = "Invalid Thresholds",
                Content = "Lower threshold CAN'T be higher than the upper threshold.",
                PrimaryButtonText = "Okay, fine...",
            };
            await invalidThresholdDialog.ShowAsync();
        }

        private async void displayDialogNoDuplicateKeysFound()
        {
            var noDuplicateKeysDialog = new ContentDialog()
            {
                Title = "No Duplicate Keys Found",
                Content = "No duplicate keys have been found",
                PrimaryButtonText = "Okay!",
            };
            await noDuplicateKeysDialog.ShowAsync();
        }

        private async Task<ContentDialogResult> displayDialogReplaceOrMergeCase()
        {
            var mergeOrReplaceDialog = new ContentDialog()
            {
                Title = "Replace or Merge Existing File?",
                Content = "Do you want to replace or merge to the current file?",
                PrimaryButtonText = "Merge!",
                SecondaryButtonText = "Replace!",
                CloseButtonText = "Cancel"
            };
            var mergeOrReplaceResult = await mergeOrReplaceDialog.ShowAsync();
            return mergeOrReplaceResult;
        }

        private void LowerThreshold_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                this.updateDisplayAsync();
            }
        }

        private void UpperThreshold_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                this.updateDisplayAsync();
            }
        }
        
        private async Task updateDisplayAsync()
        {
            this.lowerThreshold = int.Parse(this.lowerThresholdTextBox.Text);
            this.upperThreshold = int.Parse(this.upperThresholdTextBox.Text);
            if (this.lowerThreshold < this.upperThreshold)
            {
                await this.displayInformation();
            }
            else
            {
                this.displayDialogInvalidThreshold();
            }
        }

        private async void clearData_Click(object sender, RoutedEventArgs e)
        {
            ContentDialogResult dialogResult = await this.promptClearDataDialog();
            if (dialogResult == ContentDialogResult.Primary)
            {
                this.clearData();
            }
        }

        private async Task<ContentDialogResult> promptClearDataDialog()
        {
            var clearDataDialog = new ContentDialog()
            {
                Title = "Are You Sure?",
                Content = "Do you want to delete all current data?",
                PrimaryButtonText = "Yes!",
                SecondaryButtonText = "No!!"
            };
            var clearDataResult = await clearDataDialog.ShowAsync();

            return clearDataResult;
        }

        private void clearData()
        {
            this.covidCollection.ClearData();
            this.CurrentFile = null;
            this.summaryTextBox.Text = "Data Cleared...";
        }


        private async void saveData_Click(object sender, RoutedEventArgs e)
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Comma Separated Value", new List<string>() { ".csv" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "New Document";

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until we finish making changes and call CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);

                // write to file
                // write the current State file (as testing purposes)
                var data = csvReader.GetDataAsCSV(this.covidLocationData.CovidCases);

                await FileIO.WriteTextAsync(file, data);

                // Let Windows know that we're finished changing the file so the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                if (status == FileUpdateStatus.Complete)
                {
                    // OutputTextBlock.Text = "File " + file.Name + " was saved.";
                }
                else
                {
                    // OutputTextBlock.Text = "File " + file.Name + " couldn't be saved.";
                }
            }
            else
            {
                // OutputTextBlock.Text = "Operation cancelled.";
            }
        }

        private async void buttonAddNewEntry_Click(object sender, RoutedEventArgs e)
        {
            this.textBlockCovidEntryErrorMessage.Visibility = Visibility.Collapsed;

            var isEntryValid = this.validateNewEntryData();
            if (isEntryValid)
            {
                var location = this.comboboxState.SelectedValue.ToString();
                DateTime dateTime = DateTime.Parse(this.datePickerCovidCase.Date.ToString());

                var covidCase = new CovidCase(location, dateTime)
                {
                    PositiveIncrease = int.Parse(this.textBoxPositiveTests.Text),
                    NegativeIncrease = int.Parse(this.textBoxNegativeTests.Text),
                    DeathIncrease = int.Parse(this.textBoxDeaths.Text),
                    HospitalizedIncrease = int.Parse(this.textBoxHospitalizations.Text)
                };

                
                await this.covidCollection.AddCovidCase(covidCase);

                if (this.covidLocationData == null)
                {
                    this.covidLocationData = this.covidCollection.GetLocationData(LocationOfInterest);
                }

                this.clearNewDataEntryFields();
                await this.updateDisplayAsync();
            }
            else
            {
                this.textBlockCovidEntryErrorMessage.Text = "Please Complete All Fields Correctly";
                this.textBlockCovidEntryErrorMessage.Visibility = Visibility.Visible;
            }


        }

        private bool validateNewEntryData()
        {
            var selectedStateEntry = (this.comboboxState.SelectedValue != null) ? this.comboboxState.SelectedValue.ToString() : "";
            var positiveTestEntry = this.textBoxPositiveTests.Text;
            var negativeTestEntry = this.textBoxNegativeTests.Text;
            var deathsEntry = this.textBoxDeaths.Text;
            var hospitalizationsEntry = this.textBoxHospitalizations.Text;
            var selectedCovidCaseDate = this.datePickerCovidCase.Date.ToString();

            if (String.IsNullOrEmpty(selectedStateEntry))
            {
                return false;
            }

            if (String.IsNullOrEmpty(positiveTestEntry))
            {
                return false;
            }

            if (String.IsNullOrEmpty(negativeTestEntry))
            {
                return false;
            }

            if (String.IsNullOrEmpty(deathsEntry))
            {
                return false;
            }

            if (String.IsNullOrEmpty(hospitalizationsEntry))
            {
                return false;
            }

            if (String.IsNullOrEmpty(selectedCovidCaseDate))
            {
                return false;
            }

            return true;
        }

        private void clearNewDataEntryFields()
        {
            this.comboboxState.SelectedItem = null;
            this.textBoxPositiveTests.Text = "";
            this.textBoxNegativeTests.Text = "";
            this.textBoxDeaths.Text = "";
            this.textBoxHospitalizations.Text = "";
            this.datePickerCovidCase.Date = null;
        }
    }
}
