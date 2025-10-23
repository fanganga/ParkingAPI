using Microsoft.AspNetCore.Mvc;
using ParkingAPI.Models.APIModels;
using ParkingAPI.Repos;

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
    }
}
