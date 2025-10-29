
using ParkingAPI.Models.InternalModels;

namespace ParkingAPI.Services
{
    public enum EntryStatus
    {
        Success,
        RegAlreadyParked,
        NoSpace
    }
    public interface ICarEntryService
    {
        public Result<CarEntered,EntryStatus> ParkCar(string registrationNumber, CarSize size);
    }
}
