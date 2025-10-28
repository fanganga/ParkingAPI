using ParkingAPI.Models.InternalModels;

namespace ParkingAPI.Services
{
    public interface IFeeCalculator
    {
        public int CalculateFee(SpaceOccupancy occupancyPeriod);
    }
}
