using System;
using System.Collections.Generic;
using Moq;
using ParkingAPI.Repos;
using ParkingAPI.Services;
using ParkingAPI.Models.APIModels;
using ParkingAPI.Models.InternalModels;

namespace ParkingApi.Test
{
    public class CarEntryServiceTests
    {
        private DateTime testTime;
        [SetUp]
        public void SetUp()
        {
            testTime = DateTime.Now;
        }

        [Test]
        public void ShouldPassNecessaryValuesToUpdateDatabaseToTheRepo()
        {
            var mockTimeProvider = new Mock<ITimeProvider>();   
            mockTimeProvider.Setup(t => t.CurrentTime()).Returns(testTime);
            var mockRepo = new Mock<IParkingRepo>();
            int assignedSpace = 1;

            var serviceUnderTest = new CarEntryService(mockTimeProvider.Object, mockRepo.Object);
            EntryRequest request = new()
            {
                VehicleReg = "ABC 123",
                VehicleType = (int)CarSize.Medium
            };
            serviceUnderTest.ParkCar(request, assignedSpace);

            mockRepo.Verify(r => r.RecordCarEntry(It.Is<CarEntered>(
                c => c.SpaceNumber == assignedSpace && c.CarSize == CarSize.Medium && c.RegistrationNumber == "ABC 123" && c.TimeIn == testTime)
            ));
        }

        [Test]
        public void ShouldBuildResponseWithAppropriateValues()
        {
            var mockTimeProvider = new Mock<ITimeProvider>();
            mockTimeProvider.Setup(t => t.CurrentTime()).Returns(testTime);
            var mockRepo = new Mock<IParkingRepo>();
            int assignedSpace = 1;

            var serviceUnderTest = new CarEntryService(mockTimeProvider.Object, mockRepo.Object);
            EntryRequest request = new()
            {
                VehicleReg = "ABC 123",
                VehicleType = (int)CarSize.Medium
            };
            EntryResponse response = serviceUnderTest.ParkCar(request, assignedSpace);

            Assert.Multiple(() =>
            {
                Assert.That(response.VehicleReg, Is.EqualTo(request.VehicleReg));
                Assert.That(response.SpaceNumber, Is.EqualTo(assignedSpace));
                Assert.That(response.TimeIn, Is.EqualTo(testTime));
            });
        }
    }
}
