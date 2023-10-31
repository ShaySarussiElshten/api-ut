using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingGarageManagement.Dtos.ParkingLot
{
    public class ParkingLotDto
    {
        public int Id { get; set; }
        public bool IsOccupied { get; set; }
        public string ?OccupiedBy { get; set; }
      
    }
}