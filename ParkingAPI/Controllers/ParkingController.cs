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
        private readonly IFeeCalculator _calculator;

        public ParkingController(ILogger<ParkingController> logger, IParkingRepo repo, IFeeCalculator calculator)
        {
            _logger = logger;
            _repo = repo;
            _calculator = calculator;
        }

        [HttpGet]
        [Route("parking")]
        public ParkingStatus Index()
        {
            return new ParkingStatus() { AvailableSpaces = _repo.CountFreeSpaces(), OccupiedSpaces = _repo.CountOccupiedSpaces() };
        }

        [HttpPost]
        [Route("parking")]
        public ActionResult<EntryResponse> Index(EntryRequest request)
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
                CarEntered entry = new ()
                {
                    TimeIn = DateTime.UtcNow,
                    CarSize = (CarSize)request.VehicleType,
                    RegistrationNumber = request.VehicleReg,
                    SpaceNumber = (int)firstFreeSpace
                };
                _repo.RecordCarEntry(entry);

                EntryResponse response = new ()
                {
                    VehicleReg = request.VehicleReg,
                    SpaceNumber = entry.SpaceNumber,
                    TimeIn = entry.TimeIn,
                    
                };
                return new ActionResult<EntryResponse>(response);
            }            
        }

        [HttpPost]
        [Route("parking/exit")]
        public ActionResult<ExitResponse> Exit(ExitRequest request)
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

            currentOccupancy.TimeOut = DateTime.UtcNow;
            _repo.FreeSpace(currentOccupancy.SpaceNumber);

            ExitResponse response = new ()
            {
                VehicleReg = currentOccupancy.OccupierReg,
                VehicleCharge = _calculator.CalculateFee(currentOccupancy),
                TimeIn = currentOccupancy.TimeIn,
                TimeOut = currentOccupancy.TimeOut
            };
            return new ActionResult<ExitResponse>(response);
        }
    }
}
