using AutoMapper;
using ParkingGarageManagement.Models;
using ParkingGarageManagement.Dtos.Vehicle;
using ParkingGarageManagement.Dtos.ParkingLot;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Map from Vehicle to VehicleDto and vice versa
        CreateMap<Vehicle, VehicleDto>();
        CreateMap<VehicleDto, Vehicle>();
        CreateMap<ParkingLot, ParkingLotDto>();
        CreateMap<ParkingLotDto, ParkingLot>();
    }
}