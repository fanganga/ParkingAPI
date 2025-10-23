
using ParkingAPI.DataContexts;

namespace ParkingAPI.Repos
{
    public class ParkingRepo : IParkingRepo
    {
        ParkingDataContext _context;
        public ParkingRepo(ParkingDataContext context) {
            _context = context;
        }
        public int CountOccupiedSpaces()
        {
            return _context.Spaces.Count(space => space.OccupierReg != null);
        }

        public int CountFreeSpaces()
        {
            return _context.Spaces.Count(space => space.OccupierReg == null);
        }
    }
}
