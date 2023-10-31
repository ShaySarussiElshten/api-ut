using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingGarageManagement.Dtos.ParkingLot;
using ParkingGarageManagement.Dtos.Vehicle;
using ParkingGarageManagement.Models;

namespace ParkingGarageManagement.Services.ParkingLotService
{
    public interface IParkingLotService
    {
        Task<List<object>> GetCurrentParkingGarageState();
        Task<ParkingLot?> AssignParkingLot(string licensePlateId, TicketType ticketType);
        Task ReleaseParkingLot(int parkingLotId);

        Task<List<Vehicle>> GetGroupParkedVehicles(TicketType ticketType);

        Task<ApiResponse<Vehicle>> GetVehicleByParkingLotOccupiedBy(string licensePlateId);  

    }
}