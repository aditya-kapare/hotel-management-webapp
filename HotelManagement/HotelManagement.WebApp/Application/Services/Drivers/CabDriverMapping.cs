using HotelManagement.WebApp.Application.Dtos.Drivers;
using HotelManagement.WebApp.Domain.Models;

namespace HotelManagement.WebApp.Application.Services.Drivers
{
    internal static class CabDriverMapping
    {
        internal static CabDriverDtobak ToDto(CabDriver d) => new()
        {
            DriverId = d.DriverId,
            Name = d.Name,
            Age = d.Age,
            Gender = d.Gender,
            CarVendor = d.CarVendor,
            CarType = d.CarType
        };

        internal static CabDriver ToEntity(CabDriverRequest r) => new()
        {
            Name = r.Name,
            Age = r.Age,
            Gender = r.Gender,
            CarVendor = r.CarVendor,
            CarType = r.CarType
        };

        internal static CabDriver ToEntity(int driverId, CabDriverRequest r) => new()
        {
            DriverId = driverId,
            Name = r.Name,
            Age = r.Age,
            Gender = r.Gender,
            CarVendor = r.CarVendor,
            CarType = r.CarType
        };


        internal static void Apply(CabDriverRequest r, CabDriver d)
        {
            d.Name = r.Name;
            d.Age = r.Age;
            d.Gender = r.Gender;
            d.CarVendor = r.CarVendor;
            d.CarType = r.CarType;
        }
    }
}