using ParkingAPI.Models.InternalModels;

namespace ParkingAPI.Services
{
    public enum ExitStatus
    {
        Success,
        RegNotFound
    }
    public interface ICarExitService
    {
        public Result<OccupancyBill?, ExitStatus> CheckOutCar(string registrationNumber);
    }
}
