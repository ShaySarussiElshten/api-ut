using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingGarageManagement.Models
{
    public class CheckInVehicleResponse
    {
        public bool Success { get; set; }
        public string ?Message { get; set; }
        public Vehicle ?Vehicle { get; set; }
        public TicketUpgradeSuggestion ?TicketUpgradeSuggestion { get; set; }
    }
}