using Microsoft.AspNetCore.Mvc.ModelBinding;
using ParkingAPI.Models.InternalModels;

namespace ParkingAPI.Models.APIModels
{
    public class EntryRequest
    {
        public string? VehicleReg { get; set; }
        public int VehicleType { get; set; }

        public ModelStateDictionary Validate()
        {
            ModelStateDictionary validationResult = new ();
            if (!Enum.IsDefined(typeof(CarSize), this.VehicleType))
            {
                validationResult.AddModelError("VehicleType", "Invalid vehicle type - must be 1, 2 or 3");
            }
            if (String.IsNullOrEmpty(this.VehicleReg))
            {
                validationResult.AddModelError("VehicleReg", "Must supply VehicleReg");
            }
            return validationResult;
        }

    }
}
