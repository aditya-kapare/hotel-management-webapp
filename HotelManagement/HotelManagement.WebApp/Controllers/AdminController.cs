using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.WebApp.Controllers.Admin
{
    [Route("admin")]
    public class AdminController : Controller
    {
            [HttpGet("home")]
            public IActionResult Home()
            {
                // Explicitly tell MVC the view name
                return View("~/Views/AdminHome.cshtml");
            }
        }


    
}
