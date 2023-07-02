using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using MeasurTest.Model;


namespace MeasurTest.Tests
{
    public class MeasurementTests
    {
        [Fact]
        public void SetProperty_Should_Set_Property_Value()
        {
            var measurement = new Measurement();
            var expectedValue = "TestValue";

            measurement.FullName = expectedValue;


            Assert.Equal(expectedValue, measurement.FullName);
        }

        [Fact]
        public void GenerateRandomMeasurements_Should_Return_Correct_Count()
        {          

            var measurementModel = new MeasurementModel();
            var expectedCount = 10;

            var measurements = measurementModel.GenerateRandomMeasurements(expectedCount);
            var actualCount = measurements.Count;

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void GenerateRandomMeasurements_Should_Contain_Non_Null_Items()
        {

            var measurementModel = new MeasurementModel();
            var count = 10;

            var measurements = measurementModel.GenerateRandomMeasurements(count);

            Assert.All(measurements, measurement => Assert.NotNull(measurement));
        }

        [Fact]
        public void GenerateRandomMeasurements_Should_Have_Unique_IdNumbers()
        {
            var measurementModel = new MeasurementModel();
            var count = 10;

            var measurements = measurementModel.GenerateRandomMeasurements(count);
            var idNumbers = measurements.Select(m => m.idNumber).ToList();

            Assert.Equal(count, idNumbers.Distinct().Count());
        }
    }
}