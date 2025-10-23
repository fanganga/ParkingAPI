using ParkingAPI.Models.APIModels;
using ParkingAPI.Models.InternalModels;

namespace ParkingAPI.Services
{
    public interface ICarExitService
    {
        public ExitResponse CheckOutCar(SpaceOccupancy occupancy);
    }
}
