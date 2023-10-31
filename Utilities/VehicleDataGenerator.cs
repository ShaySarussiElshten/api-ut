using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingGarageManagement.Dtos.Vehicle;
using ParkingGarageManagement.Models;

namespace ParkingGarageManagement.Utilities
{
  public static class VehicleDataGenerator
  {
    private static readonly Random _random = new Random();

    public static VehicleDto GenerateRandomVehicle()
    {
        var vehicleTypes = Enum.GetValues(typeof(VehicleType));
        var ticketTypes = Enum.GetValues(typeof(TicketType));

        return new VehicleDto
        {
            Name = "Vehicle" + _random.Next(1, 100),
            LicensePlateId = "Plate" + _random.Next(1000, 9999),
            Phone = "555-01" + _random.Next(10, 99),
            VehicleType = (VehicleType)vehicleTypes.GetValue(_random.Next(vehicleTypes.Length)),
            Height = _random.Next(100, 500),
            Width = _random.Next(100, 500),
            Length = _random.Next(100, 500),
            TicketType = (TicketType)ticketTypes.GetValue(_random.Next(ticketTypes.Length)),
        };
    }
  }
}