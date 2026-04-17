using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.WebApp.Controllers.Admin
{
    [Route("admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        [HttpGet("home")]
        [HttpGet("")]
        public IActionResult Home()
            {
                // Explicitly tell MVC the view name
                return View("~/Views/AdminHome.cshtml");
            }
        }


    
}
