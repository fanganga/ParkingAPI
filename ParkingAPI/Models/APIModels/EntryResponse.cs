namespace ParkingAPI.Models.APIModels
{
    public record EntryResponse
    {
        public string VehicleReg { get; set; }
        public int SpaceNumber { get; set; }  
        public DateTime TimeIn { get; set; }
    }
}
