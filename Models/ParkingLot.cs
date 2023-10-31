using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ParkingGarageManagement.Models
{
    public class ParkingLot
    {
        public int Id { get; set; }
        public bool IsOccupied { get; set; }
        public string ?OccupiedBy { get; set; }
    }
}