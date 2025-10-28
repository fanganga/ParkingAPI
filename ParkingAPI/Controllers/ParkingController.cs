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

                if(_repo.GetCurrentOccupancyForReg(request.VehicleReg) != null)
                {
                    return BadRequest("Attempting to record arrival of vehicle registration already in car park");
                }

                int? firstFreeSpace = _repo.GetFirstFreeSpace();

                if (firstFreeSpace == null)
                {
                    return Problem("No free spaces");
                } else {
                    EntryResponse response = _carEntryService.ParkCar(request, firstFreeSpace.Value);
                    return new ActionResult<EntryResponse>(response);
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
