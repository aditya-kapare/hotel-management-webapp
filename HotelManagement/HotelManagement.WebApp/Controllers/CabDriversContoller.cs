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


            if (gender.HasValue)
            {
                drivers = drivers
                    .Where(d => (int)d.Gender == gender.Value)
                    .ToList();
            }


            if (!string.IsNullOrWhiteSpace(carVendor))
            {
                drivers = drivers
                    .Where(d => d.CarVendor == carVendor)
                    .ToList();
            }


            if (!string.IsNullOrWhiteSpace(carType))
            {
                drivers = drivers
                    .Where(d => d.CarType == carType)
                    .ToList();
            }


            ViewBag.Genders = Enum.GetValues(typeof(Gender));
            ViewBag.CarVendors = drivers.Select(d => d.CarVendor).Distinct().ToList();
            ViewBag.CarTypes = drivers.Select(d => d.CarType).Distinct().ToList();


            ViewBag.SelectedGender = gender;
            ViewBag.SelectedCarVendor = carVendor;
            ViewBag.SelectedCarType = carType;

            return View(drivers);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create(
            string name,
            int age,
            int gender,
            string carVendor,
            string carType)
        {
            var request = new CabDriverRequest
            {
                Name = name,
                Age = age,
                Gender = (Gender)gender,
                CarVendor = carVendor,
                CarType = carType
            };

            await _admin.Drivers.CreateAsync(request);


            TempData["SuccessMessage"] = "Cab driver added successfully";


            return RedirectToAction(nameof(Index));
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