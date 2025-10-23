using System.ComponentModel.DataAnnotations;

namespace ParkingAPI.Models.InternalModels
{
    public class CarEntered
    {

        public DateTime TimeIn {  get; set; }  
        public int SpaceNumber { get; set; }

        public required string RegistrationNumber { get; set; }

        public required CarSize CarSize { get; set; } 
    }
}
