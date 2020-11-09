using Covid19Analysis.Extensions;
using Covid19Analysis.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Covid19Analysis.ViewModel
{
    /// <summary>
    /// The view model
    /// </summary>
    public class CovidAnalysisViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The date format
        /// </summary>
        public const string DateFormat = "yyyyMMdd";
        private const int Default_Lower_Threshold = 0;
        private const int Default_Upper_Threshold = 2500;
        private const int Default_Bin_Size = 500;

        private string locationOfInterest;
        private int lowerThreshold;
        private int upperThreshold;
        private int binSize;
        private string stateToSave;
        private int positiveTestsToSave;
        private int negativeTestsToSave;
        private int deathsToSave;
        private int hospitalizationsToSave;
        private DateTimeOffset dateOfCaseToSave;
        private CovidLocationData covidLocationData;
        private CovidLocationDataCollection covidCollection;
        private ObservableCollection<CovidCase> covidCases;

        /// <summary>
        /// Observable collection of covid cases
        /// </summary>
        public ObservableCollection<CovidCase> CovidCases
        {
            get => this.covidCases;
            set
            {
                this.covidCases = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Covid location data collection
        /// </summary>
        public CovidLocationDataCollection CovidCollection
        {
            get => this.covidCollection;
            set
            {
                this.covidCollection = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Covid location data
        /// </summary>
        public CovidLocationData CovidLocationData
        {
            get => this.covidLocationData;
            set
            {
                this.covidLocationData = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Location of interest
        /// </summary>
        public string LocationOfInterest
        {
            get => this.locationOfInterest;
            set
            {
                this.locationOfInterest = value;
                this.OnPropertyChanged();
            }
        }
        /// <summary>
        /// The lower threshold
        /// </summary>
        public int LowerThreshold
        {
            get => this.lowerThreshold;
            set
            {
                this.lowerThreshold = value;
                this.OnPropertyChanged();
            }
        }
        /// <summary>
        /// The upper threshold
        /// </summary>
        public int UpperThreshold
        {
            get => this.upperThreshold;
            set
            {
                this.upperThreshold = value;
                this.OnPropertyChanged();
            }
        }
        /// <summary>
        /// The bin size
        /// </summary>
        public int BinSize
        {
            get => this.binSize;
            set
            {
                this.binSize = value;
                this.OnPropertyChanged();
            }
        }
        /// <summary>
        /// The state with data to save
        /// </summary>
        public string StateToSave
        {
            get => this.stateToSave;
            set
            {
                this.stateToSave = value;
                this.OnPropertyChanged();
            }
        }
        /// <summary>
        /// Positive tests to save
        /// </summary>
        public int PositiveTestsToSave
        {
            get => this.positiveTestsToSave;
            set
            {
                this.positiveTestsToSave = value;
                this.OnPropertyChanged();
            }
        }
        /// <summary>
        /// Negative tests to save
        /// </summary>
        public int NegativeTestsToSave
        {
            get => this.negativeTestsToSave;
            set
            {
                this.negativeTestsToSave = value;
                this.OnPropertyChanged();
            }
        }
        /// <summary>
        /// Deaths to save
        /// </summary>
        public int DeathsToSave
        {
            get => this.deathsToSave;
            set
            {
                this.deathsToSave = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Hospitalizations to save
        /// </summary>
        public int HospitalizationsToSave
        {
            get => this.hospitalizationsToSave;
            set
            {
                this.hospitalizationsToSave = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Date of Case to Save
        /// </summary>
        public DateTimeOffset DateOfCaseToSave
        {
            get => this.dateOfCaseToSave;
            set
            {
                this.dateOfCaseToSave = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// The view model constructor
        /// </summary>
        public CovidAnalysisViewModel()
        {
            this.LowerThreshold = Default_Lower_Threshold;
            this.UpperThreshold = Default_Upper_Threshold;
            this.BinSize = Default_Bin_Size;
            this.DateOfCaseToSave = DateTimeOffset.Now;

            this.covidCollection = new CovidLocationDataCollection();
            this.covidLocationData = new CovidLocationData("GA");
            this.covidCases = this.covidLocationData.CovidCases.ToObservableCollection();
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        /// <returns></returns>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
