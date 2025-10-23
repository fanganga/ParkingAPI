using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ParkingAPI.DataContexts;
using ParkingAPI.Models.DataModels;

namespace ParkingAPI.DbInteractionLayer
{
    public static class SeedData
    {

        public static void Initialise (IServiceProvider serviceProvider)
        {
            using (
                var context = new ParkingDataContext(serviceProvider.GetRequiredService<DbContextOptions<ParkingDataContext>>())
            )
            {
                if (Int32.TryParse(serviceProvider.GetRequiredService<IConfiguration>()["carParkSize"], out int numberOfSpaces) == false)
                {
                    numberOfSpaces = 50;
                }
                
                if (context.Spaces.FirstOrDefault() == null)
                {
                    for (int i = 1; i <= numberOfSpaces; i++)
                    {
                        context.Spaces.Add(new ParkingSpace { Id = i });
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
