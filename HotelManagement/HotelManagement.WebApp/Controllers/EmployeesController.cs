using HotelManagement.WebApp.Application.Dtos.Employee;
using HotelManagement.WebApp.Application.Interfaces.Facades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.WebApp.Controllers
{

    [Authorize(Roles = "Admin")]
    [Route("admin/employees")]
    public sealed class EmployeesController : Controller
    {
        private readonly IAdminServiceFacade _admin;

        public EmployeesController(IAdminServiceFacade admin)
        {
            _admin = admin;
        }

        // GET: /admin/employees
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var employees = await _admin.Employees.GetAllAsync();
            return View(employees);
        }

        // GET: /admin/employees/home
        [HttpGet("home")]
        public IActionResult Home()
        {
            return View();
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View(new EmployeeDetailsDto());
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateEmployeeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            await _admin.Employees.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("ViewById")]
        public IActionResult ViewById()
        {
            return View();
        }

        [HttpGet("Details")]
        public async Task<IActionResult> Details(string aadharNo)
        {
            var employee = await _admin.Employees.GetByAadharAsync(aadharNo);
            return View(employee);
        }

        // EDIT
        [HttpGet("edit/{aadharNo}")]
        public async Task<IActionResult> Edit(string aadharNo)
        {
            var employee = await _admin.Employees.GetByAadharAsync(aadharNo);
            if (employee is null) return NotFound();

            var model = new UpdateEmployeeRequest
            {
                Name = employee.Name,
                Age = employee.Age,
                Gender = employee.Gender,
                EmployeePosition = employee.EmployeePosition,
                Salary = employee.Salary,
                MobileNo = employee.MobileNo,
                EmailId = employee.EmailId
            };

            ViewBag.AadharNo = employee.AadharNo;
            return View(model);
        }

        [HttpPost("edit/{aadharNo}")]
        public async Task<IActionResult> Edit(string aadharNo, UpdateEmployeeRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AadharNo = aadharNo;
                return View(request);
            }

            await _admin.Employees.UpdateAsync(aadharNo, request);
            return RedirectToAction(nameof(Details), new { aadharNo });
        }

        // DELETE
        [HttpGet("delete/{aadharNo}")]
        public async Task<IActionResult> Delete(string aadharNo)
        {
            var employee = await _admin.Employees.GetByAadharAsync(aadharNo);
            if (employee is null) return NotFound();
            return View(employee);
        }

        [HttpPost("delete/{aadharNo}")]
        public async Task<IActionResult> DeleteConfirmed(string aadharNo)
        {
            var success = await _admin.Employees.DeleteAsync(aadharNo);
            if (!success) return BadRequest("Delete failed.");

            TempData["SuccessMessage"] =
                $"Employee with Aadhaar {aadharNo} deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}