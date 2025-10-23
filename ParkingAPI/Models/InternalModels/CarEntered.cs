namespace ParkingAPI.Models.InternalModels
{
    public class CarEntered
    {
        public DateTime TimeIn {  get; set; }  
        public int SpaceNumber { get; set; }

        public string? RegistrationNumber { get; set; }

        public CarSize? CarSize { get; set; } 
    }
}
