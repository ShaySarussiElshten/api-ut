using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingGarageManagement.Models
{
    public class Vehicle
{
    public int Id { get; set; }
    public string ?Name { get; set; }
    public string ?LicensePlateId { get; set; }
    public string ?Phone { get; set; }
    public VehicleType VehicleType { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public int Length { get; set; }
    public int ParkingLotId { get; set; }
    public ParkingLot ?parkingLot { get; set; }
    public TicketType TicketType { get; set; }
    public DateTime CheckInTime { get; set; }

    public string ClassCategory => VehicleTypeExtensions.GetClassCategory(VehicleType);
   }
}