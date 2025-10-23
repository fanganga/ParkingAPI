namespace ParkingAPI.Models.InternalModels
{
    public class SpaceOccupancy
    {
        public int SpaceNumber { get; set; }
        public String? OccupierReg { get; set; }
        public DateTime? TimeIn {  get; set; }
        public DateTime? TimeOut { get; set;}
        public CarSize OccupierSize { get; set; }

    }
}
