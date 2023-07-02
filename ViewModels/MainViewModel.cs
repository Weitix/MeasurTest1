using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MeasurTest.Model;

namespace MeasurTest.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        private string _buttonContent;
        private Measurement _selectedMeasurement;
        private DateTime? _selectedDate;
        private string _selectedCity;
        private List<Measurement> _measurements;
        private List<Measurement> _filteredList;
        private List<Measurement> _filteredMeasur;
        private int _availableMeasurementsCount;
        private string _countMassage;
        private Dictionary<string, Dictionary<DateTime, int>> _generatedMeasurementsCounts = new Dictionary<string, Dictionary<DateTime, int>>();
        public MainViewModel()
        {
            MeasurementModel measurementModel = new MeasurementModel();
            Measurements = measurementModel.GenerateRandomMeasurements(100);
            FilteredMeasur = Measurements.Where(m => !string.IsNullOrEmpty(m.ButtonDate)).ToList();
            IsButtonVisible = false;
            
        }

        private bool isButtonEnabled;
        public bool IsButtonEnabled
        {
            get { return isButtonEnabled; }
            set
            {
                if (isButtonEnabled != value)
                {
                    isButtonEnabled = value;
                    OnPropertyChanged(nameof(IsButtonEnabled));
                    UpdateButtonVisibility();
                }
            }
        }

        private bool isButtonVisible;
        public bool IsButtonVisible
        {
            get { return isButtonVisible; }
            set
            {
                if (isButtonVisible != value)
                {
                    isButtonVisible = value;
                    OnPropertyChanged(nameof(IsButtonVisible));
                }
            }
        }

        public string ButtonContent
        {
            get => _buttonContent;
            set
            {
                _buttonContent = value;
                OnPropertyChanged();
            }
        }

        public Measurement SelectedMeasurement
        {
            get => _selectedMeasurement;
            set
            {
                _selectedMeasurement = value;
                OnPropertyChanged();
            }
        }

        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
                UpdateAvailableMeasurementsCount();
            }
        }

        public string SelectedCity
        {
            get => _selectedCity;
            set
            {
                if (_selectedCity != value && !value.Contains("город"))
                {
                    _selectedCity = "Запланированные замеры в городе: " + value;
                    OnPropertyChanged();
                    _selectedCity = value;
                    FilteredMeasur = Measurements.Where(m => m.Address.Contains(_selectedCity) && !string.IsNullOrEmpty(m.ButtonDate)).ToList();
                    FilteredList = Measurements.Where(m => m.Address.Contains(_selectedCity)).ToList();

                    UpdateAvailableMeasurementsCount();
                }
                else
                {
                    FilteredList = Measurements;
                    CountMassage = string.Empty;
                    _selectedCity = "Запланированные замеры во всех городах";
                    OnPropertyChanged();
                }
            }
        }

        public List<Measurement> Measurements
        {
            get => _measurements;
            set
            {
                _measurements = value;
                OnPropertyChanged();
            }
        }

        public List<Measurement> FilteredMeasur
        {
            get => _filteredMeasur;
            set
            {
                _filteredMeasur = value;
                OnPropertyChanged();
            }
        }

        public List<Measurement> FilteredList
        {
            get => _filteredList ?? Measurements;
            set
            {
                _filteredList = value;
                OnPropertyChanged();
            }
        }

        public int AvailableMeasurementsCount
        {
            get => _availableMeasurementsCount;
            set
            {
                _availableMeasurementsCount = value;
                OnPropertyChanged();
            }
        }

        public string CountMassage
        {
            get => _countMassage;
            set
            {
                _countMassage = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand<object> _measureCommand;
        public RelayCommand<object> MeasureCommand
        {
            get
            {
                if (_measureCommand == null)
                    _measureCommand = new RelayCommand<object>(Button_Measure);
                return _measureCommand;
            }
        }

        private string _dateButtom;
        public string DateButtom
        {
            get { return _dateButtom; }
            set
            {
                _dateButtom = value;
                OnPropertyChanged(nameof(DateButtom));
            }
        }

        private string buttonDate;
        public string ButtonDate
        {
            get { return buttonDate; }
            set
            {
                if (buttonDate != value)
                {
                    buttonDate = value;
                    OnPropertyChanged(nameof(ButtonDate));
                }
            }
        }
        private void Button_Measure(object parameter)
        {
            SelectedMeasurement = parameter as Measurement;
            DateTime selectedDate = SelectedDate.Value;
            string selectedCity = SelectedCity;
            AvailableMeasurementsCount = _generatedMeasurementsCounts[selectedCity][selectedDate];

            if(AvailableMeasurementsCount > 0)
            {
                AvailableMeasurementsCount = --_generatedMeasurementsCounts[selectedCity][selectedDate];
                if (selectedDate != null && SelectedMeasurement != null)
                {
                    FilteredMeasur = Measurements.Where(m => m.Address.Contains(selectedCity) &&  !string.IsNullOrEmpty(m.ButtonDate)).ToList();
                    CountMassage = selectedDate.ToString("dd.MM.yyyy") + " в городе " + selectedCity + " доступно замеров: " + AvailableMeasurementsCount;
                    SelectedMeasurement.IsButtonVisible = false;
                    SelectedMeasurement.ButtonDate = selectedDate.ToString("dd.MM.yyyy");

                   
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите дату замера.");
                }
            }
            else
            {
                
                MessageBox.Show(" В городе " + selectedCity+ " " + selectedDate.ToString("dd.MM.yyyy") + " замеров больше недоступно");

            }
            UpdateAvailableMeasurementsCount();
            OnPropertyChanged();

        }



        private void UpdateButtonVisibility()
        {
            IsButtonVisible = IsButtonEnabled; 
        }


        private void UpdateAvailableMeasurementsCount()
        {
            if (!string.IsNullOrEmpty(SelectedCity) && !SelectedCity.Contains("город") && SelectedDate != null)
            {

                string selectedCity = SelectedCity;
                DateTime selectedDate = SelectedDate.Value;
                int measurementsCount;

                if (!_generatedMeasurementsCounts.ContainsKey(selectedCity))
                {
                    _generatedMeasurementsCounts[selectedCity] = new Dictionary<DateTime, int>();
                }

                Dictionary<DateTime, int> cityMeasurementsCounts = _generatedMeasurementsCounts[selectedCity];

                if (cityMeasurementsCounts.ContainsKey(selectedDate))
                {
                    measurementsCount = cityMeasurementsCounts[selectedDate];
                }
                else
                {
                    Random random = new Random();
                    measurementsCount = random.Next(1, 10);
                    cityMeasurementsCounts[selectedDate] = measurementsCount;
                }

                AvailableMeasurementsCount = measurementsCount;
                FilteredMeasur = Measurements.Where(m => m.Address.Contains(selectedCity) && !string.IsNullOrEmpty(m.ButtonDate)).ToList();
                CountMassage = selectedDate.ToString("dd.MM.yyyy") + " в городе " + selectedCity + " доступно замеров: " + AvailableMeasurementsCount;
                foreach (var measurement in Measurements)
                {
                    if (measurement != SelectedMeasurement && string.IsNullOrEmpty(measurement.ButtonDate))
                    {
                        measurement.IsButtonVisible = true;
                    }
                    else
                    {
                        measurement.IsButtonVisible = false;
                    }
                }
            }
            else
            {
                CountMassage = string.Empty;
                IsButtonVisible = false;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}