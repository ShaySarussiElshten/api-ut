
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ParkingGarageManagement.Data;
using ParkingGarageManagement.Dtos.Vehicle;
using ParkingGarageManagement.Models;
using ParkingGarageManagement.Services.ParkingLotService;
using ParkingGarageManagement.Services.VehicleService;
using ParkingGarageManagement.Utilities;


public class VehicleService : IVehicleService
{
    private readonly DataContext _context;
    private readonly IParkingLotService _parkingLotService;
    private readonly IMapper _mapper;

    public VehicleService(DataContext context, IParkingLotService parkingLotService, IMapper mapper)
    {
        _context = context;
        _parkingLotService = parkingLotService;
        _mapper = mapper;
    }

    public async Task<CheckInVehicleResponse> CheckInVehicle(VehicleDto vehicleDto)
    {
        if (string.IsNullOrEmpty(vehicleDto.LicensePlateId))
        {
            return new CheckInVehicleResponse { Success = false, Message = "License Plate ID is required." };
        }

        // Check if a vehicle with the same LicensePlateId is already checked in
        var existingVehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.LicensePlateId == vehicleDto.LicensePlateId);
        if (existingVehicle != null)
        {
            return new CheckInVehicleResponse { Success = false, Message = "A vehicle with this License Plate ID is already checked in." };
        }

        if (!ValidateVehicleDimensions(vehicleDto))
        {
            return CheckAlternativeTicket(vehicleDto);
        }
        
        var parkingLot = await _parkingLotService.AssignParkingLot(vehicleDto.LicensePlateId, vehicleDto.TicketType);
        if (parkingLot == null)
        {
            return new CheckInVehicleResponse { Success = false, Message = "No available parking lots for the selected ticket type." };
        }

        var vehicle = _mapper.Map<Vehicle>(vehicleDto);
        vehicle.ParkingLotId = parkingLot.Id;

        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();

        return new CheckInVehicleResponse { Success = true, Vehicle = vehicle };
    }
    public async Task<ApiResponse<VehicleDto>> CheckOutVehicle(string licensePlateId)
    {
        // Retrieve the vehicle from the database
        var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.LicensePlateId == licensePlateId);
        if (vehicle == null)
        {
            return new ApiResponse<VehicleDto>("Car not found");
        }

        // Release the parking lot
        await _parkingLotService.ReleaseParkingLot(vehicle.ParkingLotId);

        // Remove the vehicle from the database
        _context.Vehicles.Remove(vehicle);
        await _context.SaveChangesAsync();

        var vehicleDto = _mapper.Map<VehicleDto>(vehicle);
        return new ApiResponse<VehicleDto>(vehicleDto);

    } 
    
    private async Task<ApiResponse<VehicleDto>> CheckInAndMapVehicle(VehicleDto vehicleDto)
    {
        var response = await CheckInVehicle(vehicleDto);
        
        if (response.Success)
        {
            var vehicle = _mapper.Map<VehicleDto>(response.Vehicle);
            return new ApiResponse<VehicleDto>(vehicle);
        }
        else
        {
            return new ApiResponse<VehicleDto>(response.Message);
        }
    }

    public async Task<IEnumerable<ApiResponse<VehicleDto>>> BulkCheckInVehicles()
    {
        var tasks = new List<Task<ApiResponse<VehicleDto>>>();

        for (int i = 0; i < 5; i++)
        {
            var randomVehicleDto = VehicleDataGenerator.GenerateRandomVehicle();
            tasks.Add(CheckInAndMapVehicle(randomVehicleDto));
        }

        var results = await Task.WhenAll(tasks);

        return results;
    }
    private bool ValidateVehicleDimensions(VehicleDto vehicleDto)
    {
        if (!vehicleDto.VehicleType.HasValue)
        {
            throw new InvalidOperationException("Vehicle type is required.");
        }

        var vehicleType = vehicleDto.VehicleType.Value;
        var (maxHeight, maxWidth, maxLength) = TicketTypeProperties.GetDimensions(vehicleDto.TicketType);
        var allowedClasses = TicketTypeProperties.GetAllowedVehicleClasses(vehicleDto.TicketType);

        var resultDimension = vehicleDto.Height <= maxHeight && 
               vehicleDto.Width <= maxWidth && 
               vehicleDto.Length <= maxLength && 
               allowedClasses.Contains(vehicleType.GetClassCategory());
        
        return resultDimension;
    }

    private (TicketType, int)? SuggestAlternativeTicket(VehicleDto vehicleDto)
    {
      var currentCost = TicketTypeProperties.GetCost(vehicleDto.TicketType);
      var suitableTickets = Enum.GetValues(typeof(TicketType))
                              .Cast<TicketType>()
                              .Where(tt => tt != vehicleDto.TicketType) // Exclude the current ticket type
                              .Select(tt => 
                              {
                                  var alternativeVehicleDto = _mapper.Map<VehicleDto>(vehicleDto);
                                  alternativeVehicleDto.TicketType = tt;
                                  return alternativeVehicleDto;
                              })
                              .Where(vdto => ValidateVehicleDimensions(vdto))
                              .Select(vdto => (vdto.TicketType, costDifference: TicketTypeProperties.GetCost(vdto.TicketType) - currentCost))
                              .Where(t => t.costDifference > 0); // You might want to suggest only more expensive tickets, but adjust as necessary

    // Order by costDifference to suggest the ticket with the smallest cost difference
    return suitableTickets.OrderBy(t => t.costDifference).FirstOrDefault();
   }  
 
    private CheckInVehicleResponse CheckAlternativeTicket(VehicleDto vehicleDto)
    {
        var alternativeTicket = SuggestAlternativeTicket(vehicleDto);
        if (alternativeTicket.HasValue)
        {
            // Return the suggestion to the client
            return new CheckInVehicleResponse
            {
                Success = false,
                Message = "Vehicle dimensions are not valid or ticket type is not matching",
                TicketUpgradeSuggestion = new TicketUpgradeSuggestion
                {
                    SuggestedTicketType = alternativeTicket.Value.Item1,
                    CostDifference = alternativeTicket.Value.Item2
                }
            };
        }
        else
        {
            return new CheckInVehicleResponse { Success = false, Message = "No suitable ticket type available for the vehicle dimensions." };
        }
    }
  }
