
using Microsoft.EntityFrameworkCore;
using ParkingAPI.DataContexts;
using ParkingAPI.Models.InternalModels;

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

        public int? GetFirstFreeSpace()
        {
            return _context.Spaces.Where(space => space.OccupierReg == null).OrderBy(s => s.Id).FirstOrDefault()?.Id;
        }

        public void RecordCarEntry(CarEntered entry)
        {
            var space = _context.Spaces.Where(s => s.Id == entry.SpaceNumber).First();
            space.OccupierReg = entry.RegistrationNumber;
            space.OccupierTimeIn = entry.TimeIn;
            space.OccupierType = (int)entry.CarSize;

            _context.SaveChanges();
        }
    }
}
