using ParkingGarageManagement.Dtos.Vehicle;
using ParkingGarageManagement.Models;

namespace ParkingGarageManagement.Services.VehicleService
{
    public interface IVehicleService
    {
        Task<CheckInVehicleResponse> CheckInVehicle(VehicleDto vehicleDto);
        Task<ApiResponse<VehicleDto>> CheckOutVehicle(string licensePlateId);

        Task<IEnumerable<ApiResponse<VehicleDto>>> BulkCheckInVehicles();
    }
}