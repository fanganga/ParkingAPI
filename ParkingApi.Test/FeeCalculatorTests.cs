using ParkingAPI.Models.InternalModels;
using ParkingAPI.Services;

namespace ParkingApi.Test
{
    public class FeeCalculatorTests
    {
        FeeCalculator _calculator; 
        [SetUp]
        public void Setup()
        {
            _calculator = new FeeCalculator();
        }

        [Test]
        public void ShouldReturn0ForUnderOneMinute()
        {
            DateTime now = DateTime.Now;
            SpaceOccupancy under1Small = new ()
            {
                TimeIn = now.AddSeconds(-59),
                TimeOut = now,
                OccupierSize = CarSize.Small
               
            };

            Assert.That(_calculator.CalculateFee(under1Small), Is.EqualTo(0.0));
        }

        [Test]
        public void ShouldReturnAppropriatelyForJustOverOneMinute()
        {
            DateTime now = DateTime.Now;
            DateTime timeIn = now.AddSeconds(-61);
            SpaceOccupancy small = new()
            {
                TimeIn = timeIn,
                TimeOut = now,
                OccupierSize = CarSize.Small

            };

            Assert.That(_calculator.CalculateFee(small), Is.EqualTo(0.1));

            SpaceOccupancy med = new()
            {
                TimeIn = timeIn,
                TimeOut = now,
                OccupierSize = CarSize.Medium

            };

            Assert.That(_calculator.CalculateFee(med), Is.EqualTo(0.2));

            SpaceOccupancy large = new()
            {
                TimeIn = timeIn,
                TimeOut = now,
                OccupierSize = CarSize.Large

            };

            Assert.That(_calculator.CalculateFee(large), Is.EqualTo(0.4));
        }

        [Test]
        public void ShouldReturnAppropriatelyForJustUnderFiveMinutes()
        {
            DateTime now = DateTime.Now;
            DateTime timeIn = now.AddSeconds(-299);
            SpaceOccupancy small = new()
            {
                TimeIn = timeIn,
                TimeOut = now,
                OccupierSize = CarSize.Small

            };

            Assert.That(_calculator.CalculateFee(small), Is.EqualTo(0.1 * 4));

            SpaceOccupancy med = new()
            {
                TimeIn = timeIn,
                TimeOut = now,
                OccupierSize = CarSize.Medium

            };

            Assert.That(_calculator.CalculateFee(med), Is.EqualTo(0.2 *4));

            SpaceOccupancy large = new()
            {
                TimeIn = timeIn,
                TimeOut = now,
                OccupierSize = CarSize.Large

            };

            Assert.That(_calculator.CalculateFee(large), Is.EqualTo(0.4 * 4));
        }

        [Test]
        public void ShouldReturnAppropriatelyForJustOverFiveMinutes()
        {
            DateTime now = DateTime.Now;
            DateTime timeIn = now.AddSeconds(-301);
            SpaceOccupancy small = new()
            {
                TimeIn = timeIn,
                TimeOut = now,
                OccupierSize = CarSize.Small

            };

            Assert.That(_calculator.CalculateFee(small), Is.EqualTo(0.1 * 5 + 1));

            SpaceOccupancy med = new()
            {
                TimeIn = timeIn,
                TimeOut = now,
                OccupierSize = CarSize.Medium

            };

            Assert.That(_calculator.CalculateFee(med), Is.EqualTo(0.2 * 5 + 1));

            SpaceOccupancy large = new()
            {
                TimeIn = timeIn,
                TimeOut = now,
                OccupierSize = CarSize.Large

            };

            Assert.That(_calculator.CalculateFee(large), Is.EqualTo(0.4 * 5 +1));
        }

        [Test]
        public void ShouldReturnAppropriatelyForJustUnderTenMinutes()
        {
            DateTime now = DateTime.Now;
            DateTime timeIn = now.AddSeconds(-599);
            SpaceOccupancy small = new()
            {
                TimeIn = timeIn,
                TimeOut = now,
                OccupierSize = CarSize.Small

            };

            Assert.That(_calculator.CalculateFee(small), Is.EqualTo(0.1 * 9 + 1));

            SpaceOccupancy med = new()
            {
                TimeIn = timeIn,
                TimeOut = now,
                OccupierSize = CarSize.Medium

            };

            Assert.That(_calculator.CalculateFee(med), Is.EqualTo(0.2 * 9 + 1));

            SpaceOccupancy large = new()
            {
                TimeIn = timeIn,
                TimeOut = now,
                OccupierSize = CarSize.Large

            };

            Assert.That(_calculator.CalculateFee(large), Is.EqualTo(0.4 * 9 + 1));
        }

        [Test]
        public void ShouldReturnAppropriatelyForJustOverTenMinutes()
        {
            DateTime now = DateTime.Now;
            DateTime timeIn = now.AddSeconds(-601);
            SpaceOccupancy small = new()
            {
                TimeIn = timeIn,
                TimeOut = now,
                OccupierSize = CarSize.Small

            };

            Assert.That(_calculator.CalculateFee(small), Is.EqualTo(0.1 * 10 + 2));

            SpaceOccupancy med = new()
            {
                TimeIn = timeIn,
                TimeOut = now,
                OccupierSize = CarSize.Medium

            };

            Assert.That(_calculator.CalculateFee(med), Is.EqualTo(0.2 * 10 + 2));

            SpaceOccupancy large = new()
            {
                TimeIn = timeIn,
                TimeOut = now,
                OccupierSize = CarSize.Large

            };

            Assert.That(_calculator.CalculateFee(large), Is.EqualTo(0.4 * 10 + 2));
        }
    }
}