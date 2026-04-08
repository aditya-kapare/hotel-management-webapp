using HotelManagement.WebApp.Application.Interfaces.Facades;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.WebApp.Controllers
{ 
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
            return View(employees); // DTO list goes to View
        }
    }
}
