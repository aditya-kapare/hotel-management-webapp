using HotelManagement.WebApp.Application.Interfaces.Facades;
using HotelManagement.WebApp.Application.Dtos.Drivers;
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
            var allDrivers = await _admin.Drivers.GetAllAsync();

            var drivers = allDrivers.AsQueryable();

            if (gender.HasValue)
                drivers = drivers.Where(d => (int)d.Gender == gender.Value);

            if (!string.IsNullOrWhiteSpace(carVendor))
                drivers = drivers.Where(d => d.CarVendor == carVendor);

            if (!string.IsNullOrWhiteSpace(carType))
                drivers = drivers.Where(d => d.CarType == carType);

            var filteredDrivers = drivers.ToList();

            ViewBag.Genders = Enum.GetValues(typeof(Gender));
            ViewBag.CarVendors = allDrivers.Select(d => d.CarVendor).Distinct().ToList();
            ViewBag.CarTypes = allDrivers.Select(d => d.CarType).Distinct().ToList();

            ViewBag.SelectedGender = gender;
            ViewBag.SelectedCarVendor = carVendor;
            ViewBag.SelectedCarType = carType;

            ViewBag.FiltersApplied =
                gender.HasValue ||
                !string.IsNullOrWhiteSpace(carVendor) ||
                !string.IsNullOrWhiteSpace(carType);

            return View(filteredDrivers);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(
            string governmentId,
            string name,
            int age,
            int gender,
            string carVendor,
            string carType)
        {
            var request = new CabDriverForCreationDTO(
                governmentId,
                name,
                age,
                (Gender)gender,
                carVendor,
                carType
            );

            try
            {
                await _admin.Drivers.CreateAsync(request);
            }
            catch (InvalidOperationException ex)
            {
                // ✅ Duplicate GovernmentId or business rule violation
                ModelState.AddModelError(
                    nameof(governmentId),
                    "A cab driver with this Government ID already exists."
                );

                return View();
            }

            TempData["SuccessMessage"] = "Cab driver added successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("edit/{driverId}")]
        public async Task<IActionResult> Edit(int driverId)
        {
            var driver = await _admin.Drivers.GetByIdAsync(driverId);
            if (driver is null) return NotFound();

            var model = new CabDriverForUpdateDTO(
                driver.GovernmentId,
                driver.Name,
                driver.Age,
                (Gender)driver.Gender,
                driver.CarVendor,
                driver.CarType
            );

            ViewBag.DriverId = driver.DriverId;
            return View(model);
        }

        [HttpPost("edit/{driverId}")]
        public async Task<IActionResult> Edit(
            int driverId,
            CabDriverForUpdateDTO request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DriverId = driverId;
                return View(request);
            }

            try
            {
                await _admin.Drivers.UpdateByIdAsync(driverId, request);
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    nameof(request.GovernmentId),
                    "Another cab driver already uses this Government ID."
                );

                ViewBag.DriverId = driverId;
                return View(request);
            }

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
            await _admin.Drivers.DeleteByIdAsync(driverId);
            return RedirectToAction(nameof(Index));
        }
    }
}