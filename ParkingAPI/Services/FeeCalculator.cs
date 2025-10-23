using ParkingAPI.Models.InternalModels;

namespace ParkingAPI.Services
{
    public class FeeCalculator : IFeeCalculator
    {
        private Dictionary<CarSize, double> _sizeRates;
        private int _longStayIntervalMin;
        private double _longStayIntervalFee;

        public FeeCalculator()
        {
            _sizeRates = new Dictionary<CarSize, double>();
            _sizeRates.Add(CarSize.Small, 0.1);
            _sizeRates.Add(CarSize.Medium, 0.2);
            _sizeRates.Add(CarSize.Large, 0.4);

            _longStayIntervalFee = 1.00;
            _longStayIntervalMin = 5;
        }
        public double CalculateFee(SpaceOccupancy occupancyPeriod)
        {
            TimeSpan? timeInSpace = occupancyPeriod.TimeOut - occupancyPeriod.TimeIn;
            int minutes = (int) Math.Floor(timeInSpace.Value.TotalMinutes);
            double fee = minutes * _sizeRates[occupancyPeriod.OccupierSize];

            int longStayIntervals = minutes / _longStayIntervalMin;
            fee += longStayIntervals * _longStayIntervalFee;
            return fee;
        }
    }
}
