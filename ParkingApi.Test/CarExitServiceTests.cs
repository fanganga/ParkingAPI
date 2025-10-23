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
        private double testFee = 3.50;
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
            mockCalculator.Setup(c => c.CalculateFee(It.IsAny<SpaceOccupancy>())).Returns(testFee);

            CarExitService service = new CarExitService(mockTimeProvider.Object, mockCalculator.Object, mockRepo.Object);

            service.CheckOutCar(testOccupancy);

            mockRepo.Verify(r => r.FreeSpace(testSpace));
        }

        [Test]
        public void ShouldCallTheCalculatorWithTheAppropriateParameters()
        {
            var mockTimeProvider = new Mock<ITimeProvider>();
            mockTimeProvider.Setup(t => t.CurrentTime()).Returns(testTime);
            var mockRepo = new Mock<IParkingRepo>();
            var mockCalculator = new Mock<IFeeCalculator>();
            mockCalculator.Setup(c => c.CalculateFee(It.IsAny<SpaceOccupancy>())).Returns(testFee);

            CarExitService service = new CarExitService(mockTimeProvider.Object, mockCalculator.Object, mockRepo.Object);

            service.CheckOutCar(testOccupancy);

            mockCalculator.Verify(c => c.CalculateFee(It.Is<SpaceOccupancy>(o =>
             o.OccupierSize == testSize && o.TimeIn == entryTime && o.TimeOut == testTime)));
        }

        [Test]
        public void ShouldBuildTheAppropriateResponse()
        {
            var mockTimeProvider = new Mock<ITimeProvider>();
            mockTimeProvider.Setup(t => t.CurrentTime()).Returns(testTime);
            var mockRepo = new Mock<IParkingRepo>();
            var mockCalculator = new Mock<IFeeCalculator>();
            mockCalculator.Setup(c => c.CalculateFee(It.IsAny<SpaceOccupancy>())).Returns(testFee);

            CarExitService service = new CarExitService(mockTimeProvider.Object, mockCalculator.Object, mockRepo.Object);

            ExitResponse response = service.CheckOutCar(testOccupancy);

            Assert.Multiple(() =>
            {
                Assert.That(response.VehicleReg, Is.EqualTo(testReg));
                Assert.That(response.VehicleCharge, Is.EqualTo(testFee));
                Assert.That(response.TimeIn, Is.EqualTo(entryTime));
                Assert.That(response.TimeOut, Is.EqualTo(testTime));
            });

        }

    }
}
