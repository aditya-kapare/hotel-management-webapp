using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.WebApp.Presentation.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["HideSidebar"] = true;
            return View();
        }
    }
}