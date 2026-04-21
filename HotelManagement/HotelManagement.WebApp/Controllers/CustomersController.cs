using HotelManagement.WebApp.Application.Dtos.Customers;
using HotelManagement.WebApp.Application.Interfaces.Facades;
using HotelManagement.WebApp.ViewModels.Customers;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.WebApp.Controllers
{
    [Route("customers")]
    public sealed class CustomersController : Controller
    {
        private readonly IReceptionistServiceFacade _receptionistService;

        public CustomersController(IReceptionistServiceFacade receptionistService)
        {
            _receptionistService = receptionistService;
        }

      
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateCustomerViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

          
            var request = new CreateCustomerRequest
            {
                IdentityId = model.IdentityId,
                IdentityIdType = model.IdentityIdType,
                MobileNo = model.MobileNo,
                Name = model.Name,
                Gender = model.Gender,
                Address = model.Address,
                Country = model.Country
            };

            await _receptionistService.Customers.CreateAsync(request);
            
            TempData["Success"] = "Customer added successfully";
            return RedirectToAction(nameof(Index));

        }

        [HttpGet("")]
        public async Task<IActionResult> Index(
            string? identityType,
            string? gender,
            string? country,
            string? identityId)
        {
            var customers = await _receptionistService.Customers.GetAllAsync();

            // ----------------- FILTERING -----------------
            if (!string.IsNullOrWhiteSpace(identityType))
                customers = customers
                    .Where(c => c.IdentityIdType.ToString() == identityType)
                    .ToList();

            if (!string.IsNullOrWhiteSpace(gender))
                customers = customers
                    .Where(c => c.Gender.ToString() == gender)
                    .ToList();

            if (!string.IsNullOrWhiteSpace(country))
                customers = customers
                    .Where(c => c.Country == country)
                    .ToList();

            if (!string.IsNullOrWhiteSpace(identityId))
                customers = customers
                    .Where(c => c.IdentityId.Contains(identityId))
                    .ToList();

            PrepareFilters(customers, identityType, gender, country);

            return View("Index", customers);
        }



       
        private void PrepareFilters(
            IEnumerable<CustomerDto> customers,
            string? identityType,
            string? gender,
            string? country)
        {
            ViewBag.IdentityTypes = customers
                .Select(c => c.IdentityIdType.ToString())
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            ViewBag.Genders = customers
                .Select(c => c.Gender.ToString())
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            ViewBag.Countries = customers
                .Select(c => c.Country)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            ViewBag.SelectedIdentityType = identityType;
            ViewBag.SelectedGender = gender;
            ViewBag.SelectedCountry = country;
        }

        [HttpGet("edit/{identityId}")]
        public async Task<IActionResult> Edit(string identityId)
        {
            var customer = (await _receptionistService.Customers.GetAllAsync())
                .FirstOrDefault(c => c.IdentityId == identityId);

            if (customer == null)
                return NotFound();

            var vm = new EditCustomerViewModel
            {
                IdentityId = customer.IdentityId,
                IdentityIdType = customer.IdentityIdType,
                Name = customer.Name,
                MobileNo = customer.MobileNo,
                Gender = customer.Gender,
                Country = customer.Country,
                Address = customer.Address
            };

            return View(vm);
        }

        [HttpPost("edit/{identityId}")]
        public async Task<IActionResult> Edit(
       string identityId,
       EditCustomerViewModel model)
        {
            if (identityId != model.IdentityId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            var request = new UpdateCustomerRequest
            {
                Name = model.Name,
                MobileNo = model.MobileNo,
                Gender = model.Gender,
                Country = model.Country,
                Address = model.Address
            };

            await _receptionistService.Customers.UpdateAsync(
                model.IdentityId,
                request);

            return RedirectToAction(nameof(Index));
        }

    }
}