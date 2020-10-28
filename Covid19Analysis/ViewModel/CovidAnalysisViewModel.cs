using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Covid19Analysis.CovidCSV;

namespace Covid19Analysis.ViewModel
{
    public class CovidAnalysisViewModel : INotifyPropertyChanged
    {
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

        public string LocationOfInterest
        {
            get
            {
                return this.locationOfInterest;
            }
            set
            {
                this.locationOfInterest = value;
                this.OnPropertyChanged();
            }
        }
        public int LowerThreshold
        {
            get
            {
                return this.lowerThreshold;
            }
            set
            {
                this.lowerThreshold = value;
                this.OnPropertyChanged();
            }
        }
        public int UpperThreshold
        {
            get
            {
                return this.upperThreshold;
            }
            set
            {
                this.upperThreshold = value;
                this.OnPropertyChanged();
            }
        }
        public int BinSize
        {
            get
            {
                return this.binSize;
            }
            set
            {
                this.binSize = value;
                this.OnPropertyChanged();
            }
        }
        public string StateToSave
        {
            get
            {
                return this.stateToSave;
            }
            set
            {
                this.stateToSave = value;
                this.OnPropertyChanged();
            }
        }
        public int PositiveTestsToSave
        {
            get
            {
                return this.positiveTestsToSave;
            }
            set
            {
                this.positiveTestsToSave = value;
                this.OnPropertyChanged();
            }
        }
        public int NegativeTestsToSave
        {
            get
            {
                return this.negativeTestsToSave;
            }
            set
            {
                this.negativeTestsToSave = value;
                this.OnPropertyChanged();
            }
        }
        public int DeathsToSave
        {
            get
            {
                return this.deathsToSave;
            }
            set
            {
                this.deathsToSave = value;
                this.OnPropertyChanged();
            }
        }

        public int HospitalizationsToSave
        {
            get
            {
                return this.hospitalizationsToSave;
            }
            set
            {
                this.hospitalizationsToSave = value;
                this.OnPropertyChanged();
            }
        }

        public DateTimeOffset DateOfCaseToSave
        {
            get
            {
                return this.dateOfCaseToSave;
            }
            set
            {
                this.dateOfCaseToSave = value;
                this.OnPropertyChanged();
            }
        }

        public CovidAnalysisViewModel()
        {
            this.LowerThreshold = Default_Lower_Threshold;
            this.UpperThreshold = Default_Upper_Threshold;
            this.BinSize = Default_Bin_Size;
            this.DateOfCaseToSave = DateTimeOffset.Now;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
