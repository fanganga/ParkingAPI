namespace ParkingAPI.Models.DataModels
{
    public class ParkingSpace
    {
        public int Id { get; set; }
        public string? OccupierReg { get; set; }
        public int? OccupierType { get; set; }

        public DateTime? OccupierTimeIn { get; set; }
    }
}
