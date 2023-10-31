using System.Text.Json.Serialization;
namespace ParkingGarageManagement.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TicketType
    {
        VIP,
        Value,
        Regular
    }

    public static class TicketTypeProperties
    {

     public static int GetCost(TicketType ticketType)
    {
        return ticketType switch
        {
            TicketType.VIP => 200,
            TicketType.Value => 100,
            TicketType.Regular => 50,
            _ => 0
        };
    }
      public static (int lotStart, int lotEnd, int cost, TimeSpan? timeLimit) GetProperties(TicketType ticketType)
     {
        return ticketType switch
        {
            TicketType.VIP => (1, 10, 200, null), // No time limit for VIP
            TicketType.Value => (11, 30, 100, TimeSpan.FromHours(72)),
            TicketType.Regular => (31, 60, 50, TimeSpan.FromHours(24)),
            _ => throw new ArgumentOutOfRangeException(nameof(ticketType), ticketType, null)
        };
     }

      public static (int maxHeight, int maxWidth, int maxLength) GetDimensions(TicketType ticketType)
     {
        return ticketType switch
        {
            TicketType.VIP => (int.MaxValue, int.MaxValue, int.MaxValue), // No dimension limit for VIP
            TicketType.Value => (2500, 2400, 5000),
            TicketType.Regular => (2000, 2000, 3000),
            _ => throw new ArgumentOutOfRangeException(nameof(ticketType), ticketType, null)
        };
     }

      public static string[] GetAllowedVehicleClasses(TicketType ticketType)
     {
        return ticketType switch
        {
            TicketType.VIP => new[] { "A", "B", "C" }, // All classes for VIP
            TicketType.Value => new[] { "A", "B" },
            TicketType.Regular => new[] { "A" },
            _ => throw new ArgumentOutOfRangeException(nameof(ticketType), ticketType, null)
        };
     }
    }

    
}