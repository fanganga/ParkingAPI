using Microsoft.EntityFrameworkCore;
using ParkingAPI.DataContexts;
using ParkingAPI.DbInteractionLayer;
using ParkingAPI.Models.DataModels;
using ParkingAPI.Repos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ParkingDataContext>(opt => opt
    .UseInMemoryDatabase("Parking")
);

builder.Services.AddScoped<IParkingRepo, ParkingRepo>();
builder.Services.AddControllers();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialise(services);
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Parking}/{action=Index}");


app.Run();
