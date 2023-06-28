using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MeasurTest.Model
{
    public class Measurement : INotifyPropertyChanged
    {
        private string number;
        private string fullName;
        private string address;
        private string phoneNumber;
        private DateTime? date;

        public string idNumber
        {
            get { return number; }
            set { SetProperty(ref number, value); }
        }
        public string FullName
        {
            get { return fullName; }
            set { SetProperty(ref fullName, value); }
        }
        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { SetProperty(ref phoneNumber, value); }
        }
        public DateTime? Date
        {
            get { return date; }
            set { SetProperty(ref date, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class MeasurementModel
    {
        public List<Measurement> GenerateRandomMeasurements(int count)
        {
            List<Measurement> measurements = new List<Measurement>();
            Random random = new Random();

            string[] cities = { "Москва", "Ростов", "Краснодар", "Челябинск", "Санкт-Петербург" };

            for (int i = 1; i <= count; i++)
            {
                string number = $"{i}";
                string fullName = $"Client{i}";
                string address = $"Address{i} " + cities[random.Next(cities.Length)];
                string phoneNumber = $"Phone{i}";


                Measurement measurement = new Measurement
                {
                    idNumber = number,
                    FullName = fullName,
                    Address = address,
                    PhoneNumber = phoneNumber
                };

                measurements.Add(measurement);
            }

            return measurements;
        }
    }
}
