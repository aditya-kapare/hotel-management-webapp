using HotelManagement.WebApp.Application.Dtos.Drivers;
using HotelManagement.WebApp.Application.Interfaces.Services;
using HotelManagement.WebApp.Application.Services.Drivers;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using System.Text.Json;

namespace HotelManagement.WebApp.Application.Services
{
    public sealed class CabDriverService : ICabDriverService
    {
        private readonly ICabDriverDAL _driverDal;

        public CabDriverService(ICabDriverDAL driverDal)
        {
            _driverDal = driverDal;
        }

        public async Task<IReadOnlyList<CabDriverDto>> GetAllAsync()
        {
            var drivers = await _driverDal.GetAllDriversAsync();
            return drivers.Select(CabDriverMapping.ToDto).ToList();
        }

        public async Task<CabDriverDto?> GetByIdAsync(int driverId)
        {
            if (driverId <= 0) return null;

            var driver = await _driverDal.GetDriverByIdAsync(driverId);
            return driver is null ? null : CabDriverMapping.ToDto(driver);
        }

        public async Task<CabDriverDto> CreateAsync(CabDriverRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var normalized = NormalizeCreate(request);
            ValidateCreate(normalized);

            var entity = CabDriverMapping.ToEntity(normalized);
            await _driverDal.AddDriverAsync(entity);

            return CabDriverMapping.ToDto(entity);

            //var jsonCabDriver = JsonSerializer.Serialize(request);

        }

        public async Task<CabDriverDto> UpdateAsync(int driverId, CabDriverRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (driverId <= 0) throw new ArgumentException("DriverId must be positive.", nameof(driverId));

            var normalized = NormalizeUpdate(request);
            ValidateCreate(normalized);

            var entity = CabDriverMapping.ToEntity(driverId, normalized);

            var updated = await _driverDal.UpdateDriverAsync(entity);
            if (!updated)
                throw new KeyNotFoundException($"Driver '{driverId}' was not found.");

            return CabDriverMapping.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(int driverId)
        {
            if (driverId <= 0) return false;

            return await _driverDal.DeleteDriverAsync(driverId);
        }

        private static CabDriverRequest NormalizeCreate(CabDriverRequest r) => new()
        {
            Name = (r.Name ?? string.Empty).Trim(),
            Age = r.Age,
            Gender = r.Gender,
            CarVendor = (r.CarVendor ?? string.Empty).Trim(),
            CarType = (r.CarType ?? string.Empty).Trim()
        };

        private static CabDriverRequest NormalizeUpdate(CabDriverRequest r) => new()
        {
            Name = (r.Name ?? string.Empty).Trim(),
            Age = r.Age,
            Gender = r.Gender,
            CarVendor = (r.CarVendor ?? string.Empty).Trim(),
            CarType = (r.CarType ?? string.Empty).Trim()
        };

        private static void ValidateCreate(CabDriverRequest r)
        {
            if (string.IsNullOrWhiteSpace(r.Name))
                throw new ArgumentException("Name is required.", nameof(r.Name));
            if (string.IsNullOrWhiteSpace(r.CarVendor))
                throw new ArgumentException("CarVendor is required.", nameof(r.CarVendor));
            if (string.IsNullOrWhiteSpace(r.CarType))
                throw new ArgumentException("CarType is required.", nameof(r.CarType));
            if (r.Age < 0)
                throw new ArgumentException("Age cannot be negative.", nameof(r.Age));
        }
    }
}