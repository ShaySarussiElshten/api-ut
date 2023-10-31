using Microsoft.EntityFrameworkCore;
using ParkingGarageManagement.Data;
using ParkingGarageManagement.Models;
using ParkingGarageManagement.Services.ParkingLotService;
using AutoMapper;
using ParkingGarageManagement.Services.VehicleService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// Add DbContext configuration for SQLite
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register ParkingLotService and its interface with dependency injection
builder.Services.AddScoped<IParkingLotService, ParkingLotService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

// Register AutoMapper and add the profile
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(); // Enable CORS

app.UseAuthorization();

app.MapControllers();

// Initialize database and seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DataContext>();
    context.Database.Migrate();

    if (!context.ParkingLots.Any())
    {
        for (int i = 1; i <= 60; i++)
        {
            context.ParkingLots.Add(new ParkingLot { Id = i });
        }
        context.SaveChanges();
    }
}

app.Run();
