using ParkingAPI.Models.InternalModels;

namespace ParkingAPI.Services
{
    public interface ICarParkStatisticsService
    {
        public CarParkOccupancy CountSpaces();
    }
}
