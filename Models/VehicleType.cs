using System.Text.Json.Serialization;

namespace ParkingGarageManagement.Models
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum VehicleType
    {
        Motorcycle, // Class A
        Private,    // Class A
        Crossover,  // Class A
        SUV,        // Class B
        Van,        // Class B
        Truck       // Class C
    }

    public static class VehicleTypeExtensions
   {
       public static string GetClassCategory(this VehicleType vehicleType)
       {
        switch (vehicleType)
        {
            case VehicleType.Motorcycle:
            case VehicleType.Private:
            case VehicleType.Crossover:
                return "A";
            case VehicleType.SUV:
            case VehicleType.Van:
                return "B";
            case VehicleType.Truck:
                return "C";
            default:
                throw new ArgumentOutOfRangeException(nameof(vehicleType), vehicleType, null);
        }
      }
   }
}