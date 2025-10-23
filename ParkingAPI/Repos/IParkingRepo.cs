using ParkingAPI.Models.InternalModels;

namespace ParkingAPI.Repos
{
    public interface IParkingRepo
    {
        public int CountFreeSpaces();

        public int CountOccupiedSpaces();

        public int? GetFirstFreeSpace();

        public void RecordCarEntry(CarEntered entry);

        public SpaceOccupancy? GetCurrentOccupancyForReg(string reg);
    }
}
