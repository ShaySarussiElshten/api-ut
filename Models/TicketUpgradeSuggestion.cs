using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingGarageManagement.Models
{
    public class TicketUpgradeSuggestion
    {
        public TicketType SuggestedTicketType { get; set; }
        public int CostDifference { get; set; }
    }
}