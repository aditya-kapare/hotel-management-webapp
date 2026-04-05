using HotelManagement.WebApp.Application.Dtos.Drivers;
using HotelManagement.WebApp.Application.Interfaces.Services;
using HotelManagement.WebApp.Application.Services.Drivers;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;

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

        public async Task<CabDriverDto> CreateAsync(CreateCabDriverRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var normalized = NormalizeCreate(request);
            ValidateCreate(normalized);

            var entity = CabDriverMapping.ToEntity(normalized);

            await _driverDal.AddDriverAsync(entity);

            return CabDriverMapping.ToDto(entity);
        }

        public async Task<CabDriverDto> UpdateAsync(int driverId, UpdateCabDriverRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (driverId <= 0) throw new ArgumentException("DriverId must be positive.", nameof(driverId));

            var normalized = NormalizeUpdate(request);
            ValidateUpdate(normalized);

            var existing = await _driverDal.GetDriverByIdAsync(driverId);
            if (existing is null)
                throw new KeyNotFoundException($"Driver '{driverId}' was not found.");

            CabDriverMapping.Apply(normalized, existing);

            await _driverDal.UpdateDriverAsync(existing);

            return CabDriverMapping.ToDto(existing);
        }

        public async Task<bool> DeleteAsync(int driverId)
        {
            if (driverId <= 0) return false;

            var existing = await _driverDal.GetDriverByIdAsync(driverId);
            if (existing is null) return false;

            await _driverDal.DeleteDriverAsync(driverId);
            return true;
        }

        // Minimal normalization/validation
        private static CreateCabDriverRequest NormalizeCreate(CreateCabDriverRequest r) => new()
        {
            Name = (r.Name ?? string.Empty).Trim(),
            Age = r.Age,
            Gender = r.Gender,
            CarVendor = (r.CarVendor ?? string.Empty).Trim(),
            CarType = (r.CarType ?? string.Empty).Trim()
        };

        private static UpdateCabDriverRequest NormalizeUpdate(UpdateCabDriverRequest r) => new()
        {
            Name = (r.Name ?? string.Empty).Trim(),
            Age = r.Age,
            Gender = r.Gender,
            CarVendor = (r.CarVendor ?? string.Empty).Trim(),
            CarType = (r.CarType ?? string.Empty).Trim()
        };

        private static void ValidateCreate(CreateCabDriverRequest r)
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

        private static void ValidateUpdate(UpdateCabDriverRequest r)
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