using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ParkingAPI.Models.APIModels;
using ParkingAPI.Models.InternalModels;
using ParkingAPI.Services;

namespace ParkingAPI.Controllers
{
    [ApiController]
    public class ParkingController : ControllerBase
    {
        private readonly ILogger<ParkingController> _logger;
        private readonly ICarExitService _carExitService;
        private readonly ICarEntryService _carEntryService;
        private readonly ICarParkStatisticsService _carParkStatisticsService;

        public ParkingController(ILogger<ParkingController> logger, ICarParkStatisticsService carParkStatisticsService, ICarExitService carExitService, ICarEntryService carEntryService)
        {
            _logger = logger;
            _carParkStatisticsService = carParkStatisticsService;
            _carExitService = carExitService;
            _carEntryService = carEntryService;
        }

        [HttpGet]
        [Route("parking")]
        public ParkingStatus Index()
        {
            CarParkOccupancy occupancyCounts = _carParkStatisticsService.CountSpaces();
            return new ParkingStatus() { AvailableSpaces = occupancyCounts.FreeSpaces, OccupiedSpaces = occupancyCounts.OccupiedSpaces };
        }

        [HttpPost]
        [Route("parking")]
        public ActionResult<EntryResponse> Enter(EntryRequest request)
        {
            try
            { 
                ModelStateDictionary validationResult = request.Validate();
                if(validationResult.ErrorCount > 0)
                {
                    return BadRequest(validationResult);
                }

                Result<CarEntered, EntryStatus> entryResult = _carEntryService.ParkCar(request.VehicleReg, (CarSize) request.VehicleType);

                switch (entryResult.Status) {
                    case EntryStatus.Success :
                        {
                            return new EntryResponse()
                            {
                                SpaceNumber = entryResult.Value.SpaceNumber,
                                VehicleReg = entryResult.Value.RegistrationNumber,
                                TimeIn = entryResult.Value.TimeIn
                            };
                        }
                    case EntryStatus.RegAlreadyParked : return BadRequest("Attempting to record arrival of vehicle registration already in car park");
                    case EntryStatus.NoSpace: return Problem("No free spaces");
                    default: return Problem("Internal error. Unexpected return status");
                }

            } catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                return Problem("Internal error");
            }
        }

        [HttpPost]
        [Route("parking/exit")]
        public ActionResult<ExitResponse> Exit(ExitRequest request)
        {

            try
            {
                if (String.IsNullOrEmpty(request.VehicleReg))
                {
                    return BadRequest("Must supply VehicleReg");
                }

                Result<OccupancyBill, ExitStatus> exitResult = _carExitService.CheckOutCar(request.VehicleReg);
                switch (exitResult.Status)
                {
                    case ExitStatus.Success:
                        {
                            double apiFeePence = exitResult.Value.FeePence;
                            return new ExitResponse()
                            {
                                VehicleReg = exitResult.Value.RegistrationNumber,
                                TimeIn = exitResult.Value.TimeIn,
                                TimeOut = exitResult.Value.TimeOut,
                                VehicleCharge = apiFeePence / 100
                            };
                        }
                    case ExitStatus.RegNotFound:
                        {
                            return BadRequest("No vehicle with the supplied registration is recorded in the car park");
                        }
                    default:
                        return Problem("Internal error. Unexpected return status");
                }
            } catch (Exception e) {
                _logger.LogError(e, e.Message);
                return Problem("Internal error");
            }
        }
    }
}
