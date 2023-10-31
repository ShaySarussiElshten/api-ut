using Microsoft.AspNetCore.Mvc;
using ParkingGarageManagement.Dtos.ParkingLot;
using ParkingGarageManagement.Dtos.Vehicle;
using ParkingGarageManagement.Models;
using ParkingGarageManagement.Services.ParkingLotService;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ParkingLotController : ControllerBase
{
    private readonly IParkingLotService _parkingLotService;

    public ParkingLotController(IParkingLotService parkingLotService)
    {
        _parkingLotService = parkingLotService;
    }

    

   [HttpGet("group-parked-vehicles")]
   public async Task<ActionResult<List<VehicleDto>>> GetGroupParkedVehicles([FromQuery] TicketType ticketType)
   {
      
        var vehicles = await _parkingLotService.GetGroupParkedVehicles(ticketType);
        if (vehicles == null)
        {
            return NotFound("No parked vehicles found for the specified ticket type.");
        }

        return Ok(vehicles);
        
    }

    [HttpGet("vehicle-by-plate/{licensePlateId}")]
    public async Task<ActionResult<VehicleDto>> GetVehicleByParkingLotOccupiedBy(string licensePlateId)
    {
        var vehicleDto = await _parkingLotService.GetVehicleByParkingLotOccupiedBy(licensePlateId);
        if (vehicleDto == null)
            return NotFound();

        return Ok(vehicleDto);
    }

    [HttpGet("status")]
    public async Task<IActionResult> GetParkingGarageStatus()
    {
        var parkingLots = await _parkingLotService.GetCurrentParkingGarageState();
        return Ok(parkingLots);
    }
}