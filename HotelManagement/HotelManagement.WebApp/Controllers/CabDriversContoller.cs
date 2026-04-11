using HotelManagement.WebApp.Application.Dtos.Drivers;
using HotelManagement.WebApp.Application.Interfaces.Facades;
using HotelManagement.WebApp.Domain.Enums;
using Microsoft.AspNetCore.Mvc;


namespace HotelManagement.WebApp.Controllers.Admin
{
    [Route("admin/cabdrivers")]
    public sealed class CabDriversController : Controller
    {
        private readonly IAdminServiceFacade _admin;

        public CabDriversController(IAdminServiceFacade admin)
        {
            _admin = admin;
        }

        // ✅ k. View all cab drivers
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var drivers = await _admin.Drivers.GetAllAsync();
            return View(drivers);
        }

            // ✅ CREATE PAGE
            [HttpGet("create")]
            public IActionResult Create()
            {
                return View();
            }

            // ✅ CREATE POST (NO antiforgery)
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

                // ✅ Success message
                TempData["SuccessMessage"] = "Cab driver added successfully";

                // ✅ Go back to list page
                return RedirectToAction(nameof(Index));
            }
        

    // ✅ m. Update driver details (GET)
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

        // ✅ m. Update driver details (POST)
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

        // ✅ DELETE (GET – confirmation)
        [HttpGet("delete/{driverId}")]
        public async Task<IActionResult> Delete(int driverId)
        {
            var driver = await _admin.Drivers.GetByIdAsync(driverId);
            if (driver is null) return NotFound();

            return View(driver);
        }

        // ✅ DELETE (POST)
        [HttpPost("delete/{driverId}")]
        public async Task<IActionResult> DeleteConfirmed(int driverId)
        {
            await _admin.Drivers.DeleteAsync(driverId);
            return RedirectToAction(nameof(Index));
        }
    }
}