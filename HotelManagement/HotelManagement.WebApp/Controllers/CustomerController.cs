using HotelManagement.WebApp.Application.Dtos.Customers;
using HotelManagement.WebApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.WebApp.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // /Customer
        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllAsync();
            return View(customers);
        }

     
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateCustomerRequest());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            await _customerService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }

        // /Customer/Edit/{identityId}
        [HttpGet]
        public async Task<IActionResult> Edit(string identityId)
        {
            var customer = await _customerService.GetByIdentityIdAsync(identityId);
            if (customer == null) return NotFound();

            ViewBag.IdentityId = customer.IdentityId;

            return View(new UpdateCustomerRequest
            {
                Name = customer.Name,
                MobileNo = customer.MobileNo,
                Gender = customer.Gender,
                Address = customer.Address,
                Country = customer.Country
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string identityId, UpdateCustomerRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.IdentityId = identityId;
                return View(request);
            }

            await _customerService.UpdateAsync(identityId, request);
            return RedirectToAction(nameof(Index));
        }

        // /Customer/Delete/{identityId}
        [HttpGet]
        public async Task<IActionResult> Delete(string identityId)
        {
            var customer = await _customerService.GetByIdentityIdAsync(identityId);
            if (customer == null) return NotFound();

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string identityId)
        {
            await _customerService.DeleteAsync(identityId);
            return RedirectToAction(nameof(Index));
        }

        // /Customer/Details/{identityId}
        public async Task<IActionResult> Details(string identityId)
        {
            var customer = await _customerService.GetByIdentityIdAsync(identityId);
            if (customer == null) return NotFound();

            return View(customer);
        }
    }
}
