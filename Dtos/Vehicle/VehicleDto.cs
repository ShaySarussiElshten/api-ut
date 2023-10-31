using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingGarageManagement.Dtos.ParkingLot;
using ParkingGarageManagement.Models;

namespace ParkingGarageManagement.Dtos.Vehicle
{
    public class VehicleDto
    {
        public string ?Name { get; set; }
        public string ?LicensePlateId { get; set; }
        public string ?Phone { get; set; }
        public VehicleType ?VehicleType { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public TicketType TicketType { get; set; }

    }
}