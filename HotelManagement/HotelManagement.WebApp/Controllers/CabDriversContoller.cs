using HotelManagement.WebApp.Application.Dtos.Drivers;
using HotelManagement.WebApp.Application.Interfaces.Facades;
using HotelManagement.WebApp.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace HotelManagement.WebApp.Controllers.Admin
{
    [Route("admin/cabdrivers")]
    [Authorize(Roles = "Admin")]
    public sealed class CabDriversController : Controller
    {
        private readonly IAdminServiceFacade _admin;

        public CabDriversController(IAdminServiceFacade admin)
        {
            _admin = admin;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(
            int? gender,
            string? carVendor,
            string? carType)
        {
            var drivers = await _admin.Drivers.GetAllAsync();

            // ✅ FILTER BY GENDER
            if (gender.HasValue)
            {
                drivers = drivers
                    .Where(d => (int)d.Gender == gender.Value)
                    .ToList();
            }

            // ✅ FILTER BY CAR VENDOR
            if (!string.IsNullOrWhiteSpace(carVendor))
            {
                drivers = drivers
                    .Where(d => d.CarVendor == carVendor)
                    .ToList();
            }

            // ✅ FILTER BY CAR TYPE
            if (!string.IsNullOrWhiteSpace(carType))
            {
                drivers = drivers
                    .Where(d => d.CarType == carType)
                    .ToList();
            }

            // ✅ Pass dropdown values
            ViewBag.Genders = Enum.GetValues(typeof(Gender));
            ViewBag.CarVendors = drivers.Select(d => d.CarVendor).Distinct().ToList();
            ViewBag.CarTypes = drivers.Select(d => d.CarType).Distinct().ToList();

            // ✅ Preserve selected values
            ViewBag.SelectedGender = gender;
            ViewBag.SelectedCarVendor = carVendor;
            ViewBag.SelectedCarType = carType;

            return View(drivers);
        }

        [HttpGet("edit/{driverId}")]
        public async Task<IActionResult> Edit(int driverId)
        {
            var driver = await _admin.Drivers.GetByIdAsync(driverId);
            if (driver is null) return NotFound();

            var model = new CabDriverRequest
            {
                Name = driver.Name,
                Age = driver.Age,
                Gender = driver.Gender,
                CarVendor = driver.CarVendor,
                CarType = driver.CarType
            };

            ViewBag.DriverId = driver.DriverId;
            return View(model);
        }

        [HttpPost("edit/{driverId}")]
        public async Task<IActionResult> Edit(int driverId, CabDriverRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DriverId = driverId;
                return View(request);
            }

            await _admin.Drivers.UpdateAsync(driverId, request);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("delete/{driverId}")]
        public async Task<IActionResult> Delete(int driverId)
        {
            var driver = await _admin.Drivers.GetByIdAsync(driverId);
            if (driver is null) return NotFound();

            return View(driver);
        }

        [HttpPost("delete/{driverId}")]
        public async Task<IActionResult> DeleteConfirmed(int driverId)
        {
            await _admin.Drivers.DeleteAsync(driverId);
            return RedirectToAction(nameof(Index));
        }
    }
}