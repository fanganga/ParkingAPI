using Microsoft.EntityFrameworkCore;
using ParkingAPI.Models.DataModels;
namespace ParkingAPI.DataContexts
{
    public class ParkingDataContext: DbContext
    {
        public ParkingDataContext(DbContextOptions<ParkingDataContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            int numberOfSpaces = 50;
            optionsBuilder.UseSeeding( (context, _) =>
                {
                    if (context.Set<ParkingSpace>().FirstOrDefault() == null)
                    {
                        for (int i = 1; i <= numberOfSpaces; i++)
                        {
                            context.Set<ParkingSpace>().Add(new ParkingSpace { Id = i });
                        }
                        context.SaveChanges();
                    }
                }
            );
        }

        public DbSet<ParkingSpace> Spaces { get; set; }
    }
}
