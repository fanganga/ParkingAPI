using Microsoft.EntityFrameworkCore;
using ParkingAPI.DataContexts;
using ParkingAPI.DbInteractionLayer;
using ParkingAPI.Models.DataModels;
using ParkingAPI.Repos;
using ParkingAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ParkingDataContext>(opt => opt
    .UseInMemoryDatabase("Parking")
);

builder.Services.AddScoped<IParkingRepo, ParkingRepo>();
builder.Services.AddSingleton<IFeeCalculator, FeeCalculator>();
builder.Services.AddSingleton<ITimeProvider, ParkingTimeProvider>();
builder.Services.AddScoped<ICarExitService, CarExitService>();
builder.Services.AddScoped<ICarEntryService, CarEntryService>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
