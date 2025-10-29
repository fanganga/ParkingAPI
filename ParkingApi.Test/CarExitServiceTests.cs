using Moq;
using ParkingAPI.Models.APIModels;
using ParkingAPI.Models.InternalModels;
using ParkingAPI.Repos;
using ParkingAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingApi.Test
{
    public class CarExitServiceTests
    {
        private DateTime testTime;
        private int testFeePence = 350;
        private int testSpace = 2;
        private string testReg = "ABC 123";
        private DateTime entryTime;
        private SpaceOccupancy testOccupancy;
        private CarSize testSize = CarSize.Medium;

        [SetUp]
        public void SetUp()
        {
            testTime = DateTime.Now;
            entryTime = testTime.AddMinutes(-10);
            testOccupancy = new()
            {
                OccupierReg = testReg,
                OccupierSize = testSize,
                SpaceNumber = testSpace,
                TimeIn = entryTime
            };
        }

        [Test]
        public void ShouldCallTheRepoToFreeTheSpace()
        {
            var mockTimeProvider = new Mock<ITimeProvider>();
            mockTimeProvider.Setup(t => t.CurrentTime()).Returns(testTime);
            var mockRepo = new Mock<IParkingRepo>();
            var mockCalculator = new Mock<IFeeCalculator>();
            mockCalculator.Setup(c => c.CalculateFee(It.IsAny<SpaceOccupancy>())).Returns(testFeePence);
            mockRepo.Setup(r => r.GetCurrentOccupancyForReg(testReg)).Returns(testOccupancy);

            CarExitService service = new CarExitService(mockTimeProvider.Object, mockCalculator.Object, mockRepo.Object);

            service.CheckOutCar(testReg);

            mockRepo.Verify(r => r.GetCurrentOccupancyForReg(testReg));
            mockRepo.Verify(r => r.FreeSpace(testSpace));
        }

        [Test]
        public void ShouldCallTheCalculatorWithTheAppropriateParameters()
        {
            var mockTimeProvider = new Mock<ITimeProvider>();
            mockTimeProvider.Setup(t => t.CurrentTime()).Returns(testTime);
            var mockRepo = new Mock<IParkingRepo>();
            var mockCalculator = new Mock<IFeeCalculator>();
            mockCalculator.Setup(c => c.CalculateFee(It.IsAny<SpaceOccupancy>())).Returns(testFeePence);
            mockRepo.Setup(r => r.GetCurrentOccupancyForReg(testReg)).Returns(testOccupancy);

            CarExitService service = new CarExitService(mockTimeProvider.Object, mockCalculator.Object, mockRepo.Object);

            service.CheckOutCar(testReg);

            mockCalculator.Verify(c => c.CalculateFee(It.Is<SpaceOccupancy>(o =>
             o.OccupierSize == testSize && o.TimeIn == entryTime && o.TimeOut == testTime)));
        }

        [Test]
        public void ShouldBuildTheAppropriateReturnValue()
        {
            var mockTimeProvider = new Mock<ITimeProvider>();
            mockTimeProvider.Setup(t => t.CurrentTime()).Returns(testTime);
            var mockRepo = new Mock<IParkingRepo>();
            var mockCalculator = new Mock<IFeeCalculator>();
            mockCalculator.Setup(c => c.CalculateFee(It.IsAny<SpaceOccupancy>())).Returns(testFeePence);
            mockRepo.Setup(r => r.GetCurrentOccupancyForReg(testReg)).Returns(testOccupancy);

            CarExitService service = new CarExitService(mockTimeProvider.Object, mockCalculator.Object, mockRepo.Object);

            Result<OccupancyBill, ExitStatus> returnValue = service.CheckOutCar(testReg);

            Assert.Multiple(() =>
            {
                Assert.That(returnValue.Status, Is.EqualTo(ExitStatus.Success));
                Assert.That(returnValue.Value.RegistrationNumber, Is.EqualTo(testReg));
                Assert.That(returnValue.Value.FeePence, Is.EqualTo(testFeePence));
                Assert.That(returnValue.Value.TimeIn, Is.EqualTo(entryTime));
                Assert.That(returnValue.Value.TimeOut, Is.EqualTo(testTime));
            });

        }

    }
}
