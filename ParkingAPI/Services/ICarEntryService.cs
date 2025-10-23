using ParkingAPI.Models.APIModels;

namespace ParkingAPI.Services
{
    public interface ICarEntryService
    {
        public EntryResponse ParkCar(EntryRequest request, int spaceNumber);
    }
}
