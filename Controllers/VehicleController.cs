using Microsoft.AspNetCore.Mvc;
using ParkingGarageManagement.Dtos.Vehicle;
using ParkingGarageManagement.Models;
using ParkingGarageManagement.Services.VehicleService;


[ApiController]
[Route("api/vehicles")]
public class VehicleController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehicleController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpPost("checkin")]
    public async Task<ActionResult<Vehicle>> CheckInVehicle([FromBody] VehicleDto vehicleDto)
    {
        try
        {
            var vehicle = await _vehicleService.CheckInVehicle(vehicleDto);
            return Ok(vehicle);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    

    [HttpPost("checkout")]
    public async Task<ActionResult<Vehicle>> CheckOutVehicle([FromBody] CheckOutRequest request)
    {
        try
        {
            var vehicle = await _vehicleService.CheckOutVehicle(request.LicensePlateId);
            return Ok(vehicle);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("bulk-checkin")]
    public async Task<ActionResult<IEnumerable<ApiResponse<VehicleDto>>>> BulkCheckInVehicles()
    {
        var results = await _vehicleService.BulkCheckInVehicles();
        return Ok(results);
    }

}