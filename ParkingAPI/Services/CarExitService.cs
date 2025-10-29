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

        public Result<OccupancyBill?, ExitStatus> CheckOutCar(string registrationNumber)
        {
            SpaceOccupancy? currentOccupancy = _repo.GetCurrentOccupancyForReg(registrationNumber);

            if (currentOccupancy == null)
            {
                return new Result<OccupancyBill?, ExitStatus>( null, ExitStatus.RegNotFound );
            }

            currentOccupancy.TimeOut = _timeProvider.CurrentTime();
            _repo.FreeSpace(currentOccupancy.SpaceNumber);
            int internalFeePence = _feeCalculator.CalculateFee(currentOccupancy);

            return new Result<OccupancyBill?, ExitStatus>(
                new OccupancyBill(
                    registrationNumber,
                    currentOccupancy.TimeIn.Value,
                    currentOccupancy.TimeOut.Value,
                    internalFeePence
                ),
                ExitStatus.Success
            );
        }
    }
}
