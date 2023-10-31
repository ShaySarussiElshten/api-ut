using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingGarageManagement.Models
{
    public class VehicleWithParkingLot
    {
        public Vehicle Vehicle { get; set; }
        public ParkingLot ParkingLot { get; set; }
    }
}