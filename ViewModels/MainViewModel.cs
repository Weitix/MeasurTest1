using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using MeasurTest.Model;

namespace MeasurTest.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            MeasurementModel measurementModel = new MeasurementModel();
            Measurements = measurementModel.GenerateRandomMeasurements(100);
        }

        private Measurement measurement;
        public string idNumber
        {
            get { return measurement.idNumber; }
            set
            {
                if (measurement.idNumber != value)
                {
                    measurement.idNumber = value;
                    OnPropertyChanged();
                }
            }
        }
        public string FullName
        {
            get { return measurement.FullName; }
            set
            {
                if (measurement.FullName != value)
                {
                    measurement.FullName = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Address
        {
            get { return measurement.Address; }
            set
            {
                if (measurement.Address != value)
                {
                    measurement.Address = value;
                    OnPropertyChanged();
                }
            }
        }
        public string PhoneNumber
        {
            get { return measurement.PhoneNumber; }
            set
            {
                if (measurement.PhoneNumber != value)
                {
                    measurement.PhoneNumber = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime? Date
        {
            get { return measurement.Date; }
            set
            {
                if (measurement.Date != value)
                {
                    measurement.Date = value;
                    OnPropertyChanged();
                    UpdateAvailableMeasurementsCount();
                }
            }
        }
        public List<Measurement> Measurements { get; set; }

        private List<Measurement> filteredList;
        public List<Measurement> FilteredList
        {
            get
            {
                if (string.IsNullOrEmpty(SelectedCity) || SelectedCity.Contains("город"))
                    return Measurements;

                return filteredList;
            }
            set
            {
                filteredList = value;
                OnPropertyChanged();
            }
        }

        private DateTime? selectedDate;
        public DateTime? SelectedDate
        {
            get { return selectedDate; }
            set
            {
                if (selectedDate != value)
                {
                    selectedDate = value;
                    OnPropertyChanged();
                    UpdateAvailableMeasurementsCount();
                }
            }
        }

        private string availableMeasurementsCount;
        public string AvailableMeasurementsCount
        {
            get { return availableMeasurementsCount; }
            set
            {
                if (availableMeasurementsCount != value)
                {
                    availableMeasurementsCount = value;
                    OnPropertyChanged();
                }
            }
        }

        private void UpdateAvailableMeasurementsCount()
        {
            if (!string.IsNullOrEmpty(SelectedCity)&& SelectedCity != "Выберите город")
            {
                Random random = new Random();
                AvailableMeasurementsCount = SelectedDate + " в городе " + SelectedCity + " доступно замеров: " + random.Next(1, 10).ToString();
            }
            else
            {
                AvailableMeasurementsCount = string.Empty;
            }
        }

        private string selectedCity;
        public string SelectedCity
        {
            get { return selectedCity; }
            set
            {
                if (selectedCity != value )
                {
                    selectedCity = "Запланированные замеры в городе: " + value;
                    OnPropertyChanged();
                    selectedCity = value;
                    FilteredList = Measurements.Where(m => m.Address.Contains(selectedCity)).ToList();
                    UpdateAvailableMeasurementsCount();
                }
                else
                {
                    selectedCity = "Запланированные замеры во всех городах";
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}