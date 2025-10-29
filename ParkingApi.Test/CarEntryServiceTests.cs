using Moq;
using ParkingAPI.Models.APIModels;
using ParkingAPI.Models.InternalModels;
using ParkingAPI.Repos;
using ParkingAPI.Services;

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
        public void ShouldNotWriteToDbWhenRegAlreadyPresent()
        {
            var mockTimeProvider = new Mock<ITimeProvider>();
            mockTimeProvider.Setup(t => t.CurrentTime()).Returns(testTime);
            var mockRepo = new Mock<IParkingRepo>();
            string testRegistration = "GHI 789";

            mockRepo.Setup(r => r.GetCurrentOccupancyForReg(testRegistration)).Returns(new SpaceOccupancy()
            {
                OccupierReg = testRegistration
            } );

            var serviceUnderTest = new CarEntryService(mockTimeProvider.Object, mockRepo.Object);
            EntryRequest request = new()
            {
                VehicleReg = testRegistration,
                VehicleType = (int)CarSize.Medium
            };
            Result<CarEntered, EntryStatus> response = serviceUnderTest.ParkCar(request.VehicleReg, (CarSize)request.VehicleType);

            mockRepo.Verify(r => r.RecordCarEntry(It.IsAny<CarEntered>()), Times.Never);
        }

        [Test]
        public void ShouldReturnAppropriateErrorWhenRegAlreadyPresent()
        {
            var mockTimeProvider = new Mock<ITimeProvider>();
            mockTimeProvider.Setup(t => t.CurrentTime()).Returns(testTime);
            var mockRepo = new Mock<IParkingRepo>();
            string testRegistration = "GHI 789";

            mockRepo.Setup(r => r.GetCurrentOccupancyForReg(testRegistration)).Returns(new SpaceOccupancy()
            {
                OccupierReg = testRegistration
            });

            var serviceUnderTest = new CarEntryService(mockTimeProvider.Object, mockRepo.Object);
            EntryRequest request = new()
            {
                VehicleReg = testRegistration,
                VehicleType = (int)CarSize.Medium
            };

            Result<CarEntered, EntryStatus> response = serviceUnderTest.ParkCar(request.VehicleReg, (CarSize)request.VehicleType);

            Assert.Multiple(() =>
            {
                Assert.That(response.Status, Is.EqualTo(EntryStatus.RegAlreadyParked));
                Assert.That(response.Value, Is.Null);
            });
        }

        [Test]
        public void ShouldNotWriteToDbWhenNoFreeSpace()
        {
            var mockTimeProvider = new Mock<ITimeProvider>();
            mockTimeProvider.Setup(t => t.CurrentTime()).Returns(testTime);
            var mockRepo = new Mock<IParkingRepo>();
            string testRegistration = "DEF 456";

            mockRepo.Setup(r => r.GetCurrentOccupancyForReg(testRegistration)).Returns((SpaceOccupancy?)null);
            mockRepo.Setup(r => r.GetFirstFreeSpace()).Returns((int?)null);

            var serviceUnderTest = new CarEntryService(mockTimeProvider.Object, mockRepo.Object);
            EntryRequest request = new()
            {
                VehicleReg = testRegistration,
                VehicleType = (int)CarSize.Medium
            };
            Result<CarEntered, EntryStatus> response = serviceUnderTest.ParkCar(request.VehicleReg, (CarSize)request.VehicleType);

            mockRepo.Verify(r => r.RecordCarEntry(It.IsAny<CarEntered>()), Times.Never);
        }

        [Test]
        public void ShouldReturnAppropriateErrorWhenNoFreeSpace()
        {
            var mockTimeProvider = new Mock<ITimeProvider>();
            mockTimeProvider.Setup(t => t.CurrentTime()).Returns(testTime);
            var mockRepo = new Mock<IParkingRepo>();
            string testRegistration = "DEF 456";

            mockRepo.Setup(r => r.GetCurrentOccupancyForReg(testRegistration)).Returns((SpaceOccupancy?)null);
            mockRepo.Setup(r => r.GetFirstFreeSpace()).Returns((int?)null);

            var serviceUnderTest = new CarEntryService(mockTimeProvider.Object, mockRepo.Object);
            EntryRequest request = new()
            {
                VehicleReg = testRegistration,
                VehicleType = (int)CarSize.Medium
            };
            Result<CarEntered, EntryStatus> response = serviceUnderTest.ParkCar(request.VehicleReg, (CarSize)request.VehicleType);

            Assert.Multiple(() =>
            {
                Assert.That(response.Status, Is.EqualTo(EntryStatus.NoSpace));
                Assert.That(response.Value, Is.Null);
            });
        }

        [Test]
        public void ShouldPassNecessaryValuesToUpdateDatabaseToTheRepo()
        {
            var mockTimeProvider = new Mock<ITimeProvider>();
            mockTimeProvider.Setup(t => t.CurrentTime()).Returns(testTime);
            var mockRepo = new Mock<IParkingRepo>();
            int assignedSpace = 1;
            string testRegistration = "ABC 123";

            mockRepo.Setup(r => r.GetCurrentOccupancyForReg(testRegistration)).Returns((SpaceOccupancy?)null);
            mockRepo.Setup(r => r.GetFirstFreeSpace()).Returns(assignedSpace);

            var serviceUnderTest = new CarEntryService(mockTimeProvider.Object, mockRepo.Object);
            EntryRequest request = new()
            {
                VehicleReg = testRegistration,
                VehicleType = (int)CarSize.Medium
            };
            serviceUnderTest.ParkCar(request.VehicleReg, (CarSize) request.VehicleType);

            mockRepo.Verify(r => r.GetFirstFreeSpace(), Times.Once);
            mockRepo.Verify(r => r.RecordCarEntry(It.Is<CarEntered>(
                c => c.SpaceNumber == assignedSpace && c.CarSize == CarSize.Medium && c.RegistrationNumber == testRegistration && c.TimeIn == testTime)
            ));
        }

        [Test]
        public void ShouldBuildResponseWithAppropriateValues()
        {
            var mockTimeProvider = new Mock<ITimeProvider>();
            mockTimeProvider.Setup(t => t.CurrentTime()).Returns(testTime);
            var mockRepo = new Mock<IParkingRepo>();
            int assignedSpace = 2;
            string testRegistration = "DEF 456";

            mockRepo.Setup(r => r.GetCurrentOccupancyForReg(testRegistration)).Returns((SpaceOccupancy?)null);
            mockRepo.Setup(r => r.GetFirstFreeSpace()).Returns(assignedSpace);

            var serviceUnderTest = new CarEntryService(mockTimeProvider.Object, mockRepo.Object);
            EntryRequest request = new()
            {
                VehicleReg = testRegistration,
                VehicleType = (int)CarSize.Medium
            };
            Result<CarEntered, EntryStatus> response = serviceUnderTest.ParkCar(request.VehicleReg, (CarSize)request.VehicleType);

            Assert.Multiple(() =>
            {
                Assert.That(response.Status, Is.EqualTo(EntryStatus.Success));
                Assert.That(response.Value.RegistrationNumber, Is.EqualTo(request.VehicleReg));
                Assert.That(response.Value.SpaceNumber, Is.EqualTo(assignedSpace));
                Assert.That(response.Value.TimeIn, Is.EqualTo(testTime));
            });
        }

    }
}
