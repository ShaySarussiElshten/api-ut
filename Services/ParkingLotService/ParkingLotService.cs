using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ParkingGarageManagement.Data;
using ParkingGarageManagement.Dtos.Vehicle;
using ParkingGarageManagement.Models;
using ParkingGarageManagement.Services.ParkingLotService;

 public class ParkingLotService : IParkingLotService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public ParkingLotService(IMapper mapper, DataContext context)
    {
        _context = context;
        _mapper = mapper;
    }

    
    public async Task<List<Vehicle>> GetGroupParkedVehicles(TicketType ticketType)
    {
        var vehicles = await _context.Vehicles
            .Join(_context.ParkingLots, 
                  vehicle => vehicle.ParkingLotId, 
                  parkingLot => parkingLot.Id, 
                  (vehicle, parkingLot) => new { Vehicle = vehicle, ParkingLot = parkingLot })
            .Where(result => result.ParkingLot.IsOccupied && result.Vehicle.TicketType == ticketType)
            .Select(result => result.Vehicle)
            .ToListAsync();

        return _mapper.Map<List<Vehicle>>(vehicles);
    }

    public async Task<ApiResponse<Vehicle>> GetVehicleByParkingLotOccupiedBy(string licensePlateId)
    {
        var parkingLot = await _context.ParkingLots
            .FirstOrDefaultAsync(pl => pl.OccupiedBy == licensePlateId);

        if (parkingLot == null)
            return new ApiResponse<Vehicle>("Car not found in any parking lot.");

        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(v => v.ParkingLotId == parkingLot.Id);

        if (vehicle == null)
            return new ApiResponse<Vehicle>("Car found in parking lot, but vehicle details are not available.");

        
        return new ApiResponse<Vehicle>(vehicle);
    }

    public async Task<List<object>> GetCurrentParkingGarageState()
    {
        // Get vehicles with associated parking lots
        var vehicleData = await _context.Vehicles
            .Join(_context.ParkingLots, 
                vehicle => vehicle.ParkingLotId, 
                parkingLot => parkingLot.Id, 
                (vehicle, parkingLot) => new { parkingLot , vehicle })
            .Select(t=> t.vehicle).Include(p => p.parkingLot)
            .ToListAsync();

        // Get parking lots that are not occupied
        var unoccupiedParkingLots = await _context.ParkingLots
            .Where(parkingLot => !parkingLot.IsOccupied)
            .ToListAsync();

        // Combine the lists
        var combinedList = new List<object>();
        combinedList.AddRange(vehicleData);
        combinedList.AddRange(unoccupiedParkingLots.Select(pl => new { parkingLot = pl }));

        return combinedList;
    }

    public async Task<ParkingLot?> AssignParkingLot(string licensePlateId, TicketType ticketType)
    {
        if (string.IsNullOrEmpty(licensePlateId)) throw new ArgumentNullException(nameof(licensePlateId));

        var availableParkingLot = await FindAvailableParkingLot(ticketType);
        if (availableParkingLot != null)
        {
            availableParkingLot.IsOccupied = true;
            availableParkingLot.OccupiedBy = licensePlateId;
            await _context.SaveChangesAsync();
        }
        return availableParkingLot;
    }

    public async Task ReleaseParkingLot(int parkingLotId)
    {
        var parkingLot = await _context.ParkingLots.FindAsync(parkingLotId);
        if (parkingLot != null && parkingLot.IsOccupied)
        {
            parkingLot.IsOccupied = false;
            parkingLot.OccupiedBy = null;
            await _context.SaveChangesAsync();
        }
    }

    private async Task<ParkingLot?> FindAvailableParkingLot(TicketType ticketType)
    {
        var lotRange = GetParkingLotRange(ticketType);
        if (lotRange == null) return null;

        return await _context.ParkingLots
            .Where(pl => pl.Id >= lotRange.Value.Item1 && pl.Id <= lotRange.Value.Item2 && !pl.IsOccupied)
            .FirstOrDefaultAsync();
    }
    
    private (int, int)? GetParkingLotRange(TicketType ticketType)
    {
        return ticketType switch
        {
            TicketType.VIP => (1, 10),
            TicketType.Value => (11, 30),
            TicketType.Regular => (31, 60),
            _ => null
        };
    }
}


