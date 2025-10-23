using Microsoft.AspNetCore.Mvc;
using ParkingAPI.Models.APIModels;

namespace ParkingAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParkingController : ControllerBase
    {
        private readonly ILogger<ParkingController> _logger;

        public ParkingController(ILogger<ParkingController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ParkingStatus Get()
        {
            return new ParkingStatus() { AvailableSpaces = 10, OccupiedSpaces = 5 };
        }
    }
}
