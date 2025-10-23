using ParkingAPI.Models.InternalModels;

namespace ParkingAPI.Services
{
    public interface IFeeCalculator
    {
        public double CalculateFee(SpaceOccupancy occupancyPeriod);
    }
}
