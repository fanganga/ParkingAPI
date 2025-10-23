using ParkingAPI.Models.APIModels;
using ParkingAPI.Models.InternalModels;
using ParkingAPI.Repos;

namespace ParkingAPI.Services
{
    public class CarExitService : ICarExitService
    {
        private readonly ITimeProvider _timeProvider;
        private readonly IFeeCalculator _feeCalculator;
        private readonly IParkingRepo _repo;

        public CarExitService(ITimeProvider timeProvider, IFeeCalculator feeCalculator, IParkingRepo repo)
        {
            _timeProvider = timeProvider;
            _feeCalculator = feeCalculator;
            _repo = repo;
        }

        public ExitResponse CheckOutCar(SpaceOccupancy occupancy)
        {
            occupancy.TimeOut = _timeProvider.CurrentTime();
            _repo.FreeSpace(occupancy.SpaceNumber);

            return new ExitResponse()
            {
                VehicleReg = occupancy.OccupierReg,
                VehicleCharge = _feeCalculator.CalculateFee(occupancy),
                TimeIn = occupancy.TimeIn,
                TimeOut = occupancy.TimeOut
            };
        }
    }
}
