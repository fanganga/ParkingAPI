using ParkingAPI.Models.APIModels;
using ParkingAPI.Models.InternalModels;
using ParkingAPI.Repos;

namespace ParkingAPI.Services
{
    public class CarParkStatisticsService : ICarParkStatisticsService
    {
        private IParkingRepo _repo;

        public CarParkStatisticsService(IParkingRepo repo)
        {
            _repo = repo;
        }
        public CarParkOccupancy CountSpaces()
        {
            return new CarParkOccupancy(_repo.CountFreeSpaces(), _repo.CountOccupiedSpaces());
        }
    }
}
