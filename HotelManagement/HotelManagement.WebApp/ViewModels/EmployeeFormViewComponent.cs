using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.WebApp.ViewComponents
{
    public class EmployeeFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(object model)
        {
            return View(model);
        }
    }
}