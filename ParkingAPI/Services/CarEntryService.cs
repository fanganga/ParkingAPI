using ParkingAPI.Models.APIModels;
using ParkingAPI.Models.InternalModels;
using ParkingAPI.Repos;

namespace ParkingAPI.Services
{
    public class CarEntryService : ICarEntryService
    {
        private readonly ITimeProvider _timeProvider;
        private readonly IParkingRepo _repo;

        public CarEntryService(ITimeProvider timeProvider,  IParkingRepo repo)
        {
            _timeProvider = timeProvider;
            _repo = repo;
        }
        public EntryResponse ParkCar(EntryRequest request, int spaceNumber)
        {
            CarEntered entry = new()
            {
                TimeIn = _timeProvider.CurrentTime(),
                CarSize = (CarSize)request.VehicleType,
                RegistrationNumber = request.VehicleReg,
                SpaceNumber = spaceNumber
            };
            _repo.RecordCarEntry(entry);

            return new()
            {
                VehicleReg = request.VehicleReg,
                SpaceNumber = entry.SpaceNumber,
                TimeIn = entry.TimeIn
            };
        }
    }
}
