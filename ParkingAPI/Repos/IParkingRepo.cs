namespace ParkingAPI.Repos
{
    public interface IParkingRepo
    {
        public int CountFreeSpaces();

        public int CountOccupiedSpaces();
    }
}
