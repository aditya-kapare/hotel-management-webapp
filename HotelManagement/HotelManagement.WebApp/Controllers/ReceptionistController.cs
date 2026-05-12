using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.WebApp.Controllers.Receptionist
{
    [Route("reception")]
    [Authorize(Roles = "Receptionist")]
    public class ReceptionistController : Controller
    {
        [HttpGet("home")]
        [HttpGet("")]
        public IActionResult Home()
        {
            return View("~/Views/ReceptionistHome.cshtml");
        }
    }
}