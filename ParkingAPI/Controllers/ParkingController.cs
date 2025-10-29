using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ParkingAPI.Models.APIModels;
using ParkingAPI.Models.InternalModels;
using ParkingAPI.Repos;
using ParkingAPI.Services;
using System.Text.Json.Serialization;

namespace ParkingAPI.Controllers
{
    [ApiController]
    public class ParkingController : ControllerBase
    {
        private readonly ILogger<ParkingController> _logger;
        private readonly IParkingRepo _repo;
        private readonly ICarExitService _carExitService;
        private readonly ICarEntryService _carEntryService;

        public ParkingController(ILogger<ParkingController> logger, IParkingRepo repo, ICarExitService carExitService, ICarEntryService carEntryService)
        {
            _logger = logger;
            _repo = repo;
            _carExitService = carExitService;
            _carEntryService = carEntryService;
        }

        [HttpGet]
        [Route("parking")]
        public ParkingStatus Index()
        {
            return new ParkingStatus() { AvailableSpaces = _repo.CountFreeSpaces(), OccupiedSpaces = _repo.CountOccupiedSpaces() };
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
                SpaceOccupancy? currentOccupancy = _repo.GetCurrentOccupancyForReg(request.VehicleReg);

                if(currentOccupancy == null)
                {
                    return BadRequest("No vehicle with the supplied registration is recorded in the car park");
                }

                ExitResponse response = _carExitService.CheckOutCar(currentOccupancy);
                return new ActionResult<ExitResponse>(response);
            } catch (Exception e) {
                _logger.LogError(e, e.Message);
                return Problem("Internal error");
            }
        }
    }
}
