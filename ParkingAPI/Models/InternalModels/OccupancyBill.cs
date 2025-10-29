namespace ParkingAPI.Models.InternalModels
{
    public record OccupancyBill(
        string RegistrationNumber,
        DateTime TimeIn,
        DateTime TimeOut,
        int FeePence
    );
}
