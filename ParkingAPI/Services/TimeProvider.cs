
namespace ParkingAPI.Services
{
    public class ParkingTimeProvider : ITimeProvider
    {
        public DateTime CurrentTime()
        {
            return DateTime.UtcNow;
        }
    }
}
