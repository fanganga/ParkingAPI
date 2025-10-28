using ParkingAPI.Models.InternalModels;

namespace ParkingAPI.Services
{
    public class FeeCalculator : IFeeCalculator
    {
        private readonly Dictionary<CarSize, int> _sizeRatesPence;
        private readonly int _longStayIntervalMin;
        private readonly int _longStayIntervalFeePence;

        public FeeCalculator()
        {
            _sizeRatesPence = new Dictionary<CarSize, int>
            {
                { CarSize.Small, 10 },
                { CarSize.Medium, 20 },
                { CarSize.Large, 40 }
            };

            _longStayIntervalFeePence = 100;
            _longStayIntervalMin = 5;
        }
        public int CalculateFee(SpaceOccupancy occupancyPeriod)
        {
            TimeSpan timeInSpace = occupancyPeriod.TimeOut.Value - occupancyPeriod.TimeIn.Value;
            int minutes = (int) Math.Floor(timeInSpace.TotalMinutes);
            int fee = minutes * _sizeRatesPence[occupancyPeriod.OccupierSize];

            int longStayIntervals = minutes / _longStayIntervalMin;
            fee += longStayIntervals * _longStayIntervalFeePence;
            return fee;
        }
    }
}
