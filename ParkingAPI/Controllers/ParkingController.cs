using Microsoft.AspNetCore.Mvc;
using ParkingAPI.Models.APIModels;
using ParkingAPI.Models.InternalModels;
using ParkingAPI.Repos;
using System.Text.Json.Serialization;

namespace ParkingAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParkingController : ControllerBase
    {
        private readonly ILogger<ParkingController> _logger;
        private IParkingRepo _repo;

        public ParkingController(ILogger<ParkingController> logger, IParkingRepo repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet]
        public ParkingStatus Get()
        {
            return new ParkingStatus() { AvailableSpaces = _repo.CountFreeSpaces(), OccupiedSpaces = _repo.CountOccupiedSpaces() };
        }

        [HttpPost]
        public ActionResult<EntryResponse> Post(EntryRequest request)
        {
            if( !Enum.IsDefined(typeof(CarSize), request.VehicleType)){
                return BadRequest("Invalid vehicle type - must be 1, 2 or 3");
            }
            if(String.IsNullOrEmpty(request.VehicleReg))
            {
                return BadRequest("Must supply VehicleReg");
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
                CarEntered entry = new CarEntered()
                {
                    TimeIn = DateTime.UtcNow,
                    CarSize = (CarSize)request.VehicleType,
                    RegistrationNumber = request.VehicleReg,
                    SpaceNumber = (int)firstFreeSpace
                };
                _repo.RecordCarEntry(entry);

                EntryResponse response = new EntryResponse()
                {
                    VehicleReg = request.VehicleReg,
                    SpaceNumber = entry.SpaceNumber,
                    TimeIn = entry.TimeIn,
                    
                };
                return new ActionResult<EntryResponse>(response);
            }
            
        }
    }
}
