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
        public Result<CarEntered, EntryStatus> ParkCar(string registrationNumber, CarSize size)
        {
            if (_repo.GetCurrentOccupancyForReg(registrationNumber) != null)
            {
                return new Result<CarEntered, EntryStatus>(null, EntryStatus.RegAlreadyParked);
            }

            int? firstFreeSpace = _repo.GetFirstFreeSpace();
            if (firstFreeSpace == null) {
                return new Result<CarEntered, EntryStatus>(null, EntryStatus.NoSpace);
            }

            CarEntered entry = new()
            {
                TimeIn = _timeProvider.CurrentTime(),
                CarSize = size,
                RegistrationNumber = registrationNumber,
                SpaceNumber = firstFreeSpace.Value
            };
            _repo.RecordCarEntry(entry);

            return new Result<CarEntered, EntryStatus>(entry, EntryStatus.Success);
        }
    }
}
