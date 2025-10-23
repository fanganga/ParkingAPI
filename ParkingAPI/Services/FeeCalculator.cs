using ParkingAPI.Models.InternalModels;

namespace ParkingAPI.Services
{
    public class FeeCalculator : IFeeCalculator
    {
        private readonly Dictionary<CarSize, double> _sizeRates;
        private readonly int _longStayIntervalMin;
        private readonly double _longStayIntervalFee;

        public FeeCalculator()
        {
            _sizeRates = new Dictionary<CarSize, double>
            {
                { CarSize.Small, 0.1 },
                { CarSize.Medium, 0.2 },
                { CarSize.Large, 0.4 }
            };

            _longStayIntervalFee = 1.00;
            _longStayIntervalMin = 5;
        }
        public double CalculateFee(SpaceOccupancy occupancyPeriod)
        {
            TimeSpan timeInSpace = occupancyPeriod.TimeOut.Value - occupancyPeriod.TimeIn.Value;
            int minutes = (int) Math.Floor(timeInSpace.TotalMinutes);
            double fee = minutes * _sizeRates[occupancyPeriod.OccupierSize];

            int longStayIntervals = minutes / _longStayIntervalMin;
            fee += longStayIntervals * _longStayIntervalFee;
            return fee;
        }
    }
}
