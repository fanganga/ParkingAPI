using ParkingAPI.Models.APIModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingApi.Test
{
    public class EntryRequestTests
    {
        [Test]
        public void ShouldPassValidationWhenSuppliedWithValidRegAndType()
        {
            EntryRequest request = new()
            {
                VehicleReg = "ABC 123",
                VehicleType = 1
            };

            var validationResult = request.Validate();

            Assert.Multiple(() =>
            {
                Assert.That(validationResult.IsValid, Is.True);
                Assert.That(validationResult.ErrorCount, Is.EqualTo(0));
            });
        }

        [Test]
        public void ShouldReportErrorWhenRegIsBlank()
        {
            EntryRequest request = new()
            {
                VehicleReg = "",
                VehicleType = 1
            };

            var validationResult = request.Validate();

            Assert.Multiple(() =>
            {
                Assert.That(validationResult.IsValid, Is.False);
                Assert.That(validationResult.ErrorCount, Is.EqualTo(1));
                Assert.That(validationResult.Keys.ElementAt(0), Is.EqualTo("VehicleReg"));
            });
        }

        [Test]
        public void ShouldReportErrorWhenTypeIsOutOfRange()
        {
            EntryRequest request = new()
            {
                VehicleReg = "ABC 123",
                VehicleType = 10
            };

            var validationResult = request.Validate();

            Assert.Multiple(() =>
            {
                Assert.That(validationResult.IsValid, Is.False);
                Assert.That(validationResult.ErrorCount, Is.EqualTo(1));
                Assert.That(validationResult.Keys.ElementAt(0), Is.EqualTo("VehicleType"));
            });
        }

        [Test]
        public void ShouldReportErrorsWhenBothAreWrong()
        {
            EntryRequest request = new()
            {
                VehicleReg = "",
                VehicleType = 0
            };

            var validationResult = request.Validate();

            Assert.Multiple(() =>
            {
                Assert.That(validationResult.IsValid, Is.False);
                Assert.That(validationResult.ErrorCount, Is.EqualTo(2));
            });
        }
    }
}
