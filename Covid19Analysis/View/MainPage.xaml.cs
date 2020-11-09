using Covid19Analysis.CovidCSV;
using Covid19Analysis.IO;
using Covid19Analysis.Model;
using Covid19Analysis.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Covid19Analysis.ViewModel;
using XmlReader = Covid19Analysis.CovidXML.XmlReader;
using XmlWriter = Covid19Analysis.CovidXml.XmlWriter;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Covid19Analysis
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        #region Constants
        private string LocationOfInterest = "GA";
        private const int LowerThresholdDefault = 0;
        private const int UpperThresholdDefault = 2500;
        private const int BinSizeDefault = 500;
        #endregion

        #region Data members
        private readonly CsvReader csvReader;
        private readonly CsvWriter csvWriter;
        private CovidAnalysisViewModel viewModel;

        /// <summary>
        ///     The application height
        /// </summary>
        public const int ApplicationHeight = 800;

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

            ApplicationView.PreferredLaunchViewSize = new Size { Width = ApplicationWidth, Height = ApplicationHeight };
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(ApplicationWidth, ApplicationHeight));

            this.csvReader = new CsvReader();
            this.csvWriter = new CsvWriter();
            this.viewModel = new CovidAnalysisViewModel();

            this.lowerThresholdTextBox.Text = LowerThresholdDefault.ToString();
            this.upperThresholdTextBox.Text = UpperThresholdDefault.ToString();
            this.binSizeTextBox.Text = BinSizeDefault.ToString();

            this.comboboxLocationSelection.IsEnabled = false;
            this.comboboxState.ItemsSource = Enum.GetValues(typeof(UnitedStatesLocations)).Cast<UnitedStatesLocations>();
        }

        #endregion
        private async void displayErrors_Click(object sender, RoutedEventArgs e)
        {
            const string defaultOutput = "No Known Errors";
            var errorDialog = new ContentDialog
            {
                Title = "CSV Errors",
                Content = new ScrollViewer
                {
                    Content = new TextBlock
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
            this.viewModel.LowerThreshold = int.Parse(this.lowerThresholdTextBox.Text);
            this.viewModel.UpperThreshold = int.Parse(this.upperThresholdTextBox.Text);

            var validThreshold = (this.viewModel.LowerThreshold < this.viewModel.UpperThreshold);
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

                try
                {
                    await this.extractData();

                    if (this.CurrentFile != null || this.viewModel.CovidLocationData != null)
                    {
                        this.displayInformation();
                        this.updateLocationSelectionCombobox(true);
                    }

                }
                catch (Exception exc)
                {
                    this.CurrentFile = null;
                    var invalidFileDialog = new ContentDialog()
                    {
                        Title = "Invalid File Format",
                        Content = "The file you selected appears to be in the wrong format.",
                        PrimaryButtonText = "Okay!",
                    };
                    await invalidFileDialog.ShowAsync();
                }
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
                this.viewModel.CovidCollection.ClearData();
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
            picker.FileTypeFilter.Add(".xml");

            return await picker.PickSingleFileAsync();
        }

        private async Task extractData()
        {
            const string XmlExtension = ".xml";
            if (this.CurrentFile != null)
            {
                if (this.CurrentFile.FileType == XmlExtension)
                {
                    XmlReader reader = new XmlReader();
                    var covidCollection = await reader.GetCovidData(this.CurrentFile);
                    this.viewModel.CovidCollection.AddAllCovidCases(covidCollection);
                }
                else
                {
                    csvReader.CsvFile = this.CurrentFile;
                    IList<CovidCase> covidCases = await csvReader.Parse();
                    this.viewModel.CovidCollection.AddAllCovidCases(covidCases);
                }


                this.viewModel.CovidLocationData = this.viewModel.CovidCollection.GetLocationData(LocationOfInterest);
                this.updateLocationSelectionCombobox(true);
            }

            if (this.viewModel.CovidLocationData != null && this.viewModel.CovidLocationData.DuplicateCases.Count > 0)
            {
                await this.displayDuplicateCases();
            }
        }

        private async Task displayDuplicateCases()
        {
            IList<CovidCase> tempList = new List<CovidCase>();

            if (this.viewModel.CovidLocationData == null || this.viewModel.CovidLocationData.DuplicateCases.Count == 0)
            {
                this.displayDialogNoDuplicateKeysFound();
            }

            if (this.viewModel.CovidLocationData != null && this.viewModel.CovidLocationData.DuplicateCases.Count > 0)
            {

                var skipOrReplaceDialog = new DuplicateEntryContentDialog();

                for (var i = 0; i < this.viewModel.CovidLocationData.DuplicateCases.Count; i++)
                {
                    skipOrReplaceDialog.Subtitle = $"There are {this.viewModel.CovidLocationData.DuplicateCases.Count - i} items with the same date";
                    skipOrReplaceDialog.Message = this.viewModel.CovidLocationData.DuplicateCases[i].ToString();
                    skipOrReplaceDialog.UpdateContent();

                    if (!skipOrReplaceDialog.IsChecked)
                    {
                        var result = await skipOrReplaceDialog.ShowAsync();

                        if (result == ContentDialogResult.Primary)
                        {
                            this.viewModel.CovidLocationData.FindAndReplace(this.viewModel.CovidLocationData.DuplicateCases[i]);
                            tempList.Add(this.viewModel.CovidLocationData.DuplicateCases[i]);
                        }
                    }
                    else if (skipOrReplaceDialog.IsChecked && skipOrReplaceDialog.LastKnownButtonPress == "Primary")
                    {
                        this.viewModel.CovidLocationData.FindAndReplace(this.viewModel.CovidLocationData.DuplicateCases[i]);
                        tempList.Add(this.viewModel.CovidLocationData.DuplicateCases[i]);
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

        private void displayInformation()
        {
            if (this.CurrentFile != null || this.viewModel.CovidLocationData != null)
            {
                this.buildAndSetSummaryReport();
            }
            else
            {
                this.summaryTextBox.Text = $"No file loaded OR No information for {LocationOfInterest}";
            }
        }

        private void buildAndSetSummaryReport()
        {
            try
            {
                this.summaryTextBox.Text = "Loading...";

                if (this.viewModel.CovidLocationData != null)
                {
                    CovidOutputBuilder report = new CovidOutputBuilder(this.viewModel.CovidLocationData)
                    {
                        LowerThreshold = this.viewModel.LowerThreshold,
                        UpperThreshold = this.viewModel.UpperThreshold,
                        BinSize = this.viewModel.BinSize
                    };
                    //this.summaryTextBox.Text = report.GetLocationSummary() + report.GetYearlySummary();
                    this.summaryTextBox.Text = report.CovidOutput();
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
            this.displayInformation();
        }

        private int removeDuplicateItems(IList<CovidCase> tempList)
        {

            var count = 0;
            foreach (var currentCase in tempList)
            {

                var item = this.viewModel.CovidLocationData.DuplicateCases.First(i => i.Date.Equals(currentCase.Date));
                var index = this.viewModel.CovidLocationData.DuplicateCases.IndexOf(item);

                if (index == -1) continue;
                this.viewModel.CovidLocationData.DuplicateCases.RemoveAt(index);

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
            var mergeOrReplaceDialog = new ContentDialog
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
                this.updateDisplay();
            }
        }

        private void UpperThreshold_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                this.updateDisplay();
            }
        }

        private void updateDisplay()
        {
            this.viewModel.LowerThreshold = int.Parse(this.lowerThresholdTextBox.Text);
            this.viewModel.UpperThreshold = int.Parse(this.upperThresholdTextBox.Text);
            this.viewModel.BinSize = int.Parse(this.binSizeTextBox.Text);
            if (this.viewModel.LowerThreshold < this.viewModel.UpperThreshold && (this.CurrentFile != null || this.viewModel.CovidLocationData != null))
            {
                this.displayInformation();
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
            this.viewModel.CovidCollection.ClearData();
            this.CurrentFile = null;
            this.summaryTextBox.Text = "Data Cleared...";
            this.updateLocationSelectionCombobox(false);
        }

        private async void saveData_Click(object sender, RoutedEventArgs e)
        {
            const string CsvFileExtension = ".csv";
            const string XmlFileExension = ".xml";

            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Comma Separated Value", new List<string>() { CsvFileExtension });
            savePicker.FileTypeChoices.Add("Extensible Markup Language", new List<string>() { XmlFileExension });
            savePicker.SuggestedFileName = "New Document";
            StorageFile file = await savePicker.PickSaveFileAsync();

            if (file != null)
            {
                if (file.FileType == XmlFileExension)
                {
                    XmlWriter writer = new XmlWriter();
                    writer.WriteToXml(file, this.viewModel.CovidCollection);
                }
                else if (file.FileType == CsvFileExtension)
                {
                    this.csvWriter.SaveDataAsCsv(file, this.viewModel.CovidCollection);
                }

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

        private void buttonAddNewEntry_Click(object sender, RoutedEventArgs e)
        {
            this.textBlockCovidEntryErrorMessage.Visibility = Visibility.Collapsed;

            var isEntryValid = this.validateNewEntryData();
            if (isEntryValid)
            {
                var location = this.comboboxState.SelectedValue?.ToString();
                DateTime dateTime = DateTime.Parse(this.datePickerCovidCase.Date.ToString());

                var covidCase = new CovidCase(location, dateTime)
                {
                    PositiveIncrease = int.Parse(this.textBoxPositiveTests.Text),
                    NegativeIncrease = int.Parse(this.textBoxNegativeTests.Text),
                    DeathIncrease = int.Parse(this.textBoxDeaths.Text),
                    HospitalizedIncrease = int.Parse(this.textBoxHospitalizations.Text)
                };


                this.viewModel.CovidCollection.AddCovidCase(covidCase);

                if (this.viewModel.CovidLocationData == null)
                {
                    this.viewModel.CovidLocationData = this.viewModel.CovidCollection.GetLocationData(LocationOfInterest);
                }

                this.clearNewDataEntryFields();
                this.updateDisplay();
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

        private void updateLocationSelectionCombobox(bool isEnabled)
        {
            this.comboboxLocationSelection.IsEnabled = isEnabled;
            if (this.CurrentFile != null || this.viewModel.CovidCollection.CollectionOfCovidLocationData.Count > 0)
            {
                this.comboboxLocationSelection.ItemsSource =
                    this.viewModel.CovidCollection.CollectionOfCovidLocationData.Keys.ToList();
            }
        }

        private void comboboxLocationSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((this.comboboxLocationSelection.SelectedItem != null || this.viewModel.CovidCollection.CollectionOfCovidLocationData.Count > 0) && this.comboboxLocationSelection.SelectedValue != null)
            {
                var selectedValue = this.comboboxLocationSelection.SelectedValue.ToString();
                this.viewModel.CovidLocationData = this.viewModel.CovidCollection.GetLocationData(selectedValue);
                this.updateDisplay();
            }
            else
            {
                this.comboboxLocationSelection.ItemsSource = null;
                this.updateLocationSelectionCombobox(false);
            }

            
        }

        private void onUpdateSummary_Click(object sender, RoutedEventArgs e)
        {
            this.updateDisplay();
        }
    }
}
