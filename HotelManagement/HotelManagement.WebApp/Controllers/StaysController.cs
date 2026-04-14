using HotelManagement.WebApp.Application.Dtos.Stays;
using HotelManagement.WebApp.Application.Interfaces.Facades;
using HotelManagement.WebApp.ViewModels.Stays;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.WebApp.Controllers
{
    //[Authorize(Roles = "Receptionist")]
    [Route("stays")]
    public sealed class StaysController : Controller
    {
        private readonly IReceptionistServiceFacade _stayService;

        public StaysController(IReceptionistServiceFacade stayService)
        {
            _stayService = stayService;
        }

        // Landing page
        [HttpGet("")]
        public IActionResult Home()
        {
            return View();
        }

        // List page
        [HttpGet("list")]
        public async Task<IActionResult> Index()
        {
            var stays = await _stayService.Stays.GetAllAsync();
            return View(stays);
        }

        // --------------------------------------------------
        // e. UPDATE CHECKED-IN CUSTOMER (GET)
        // --------------------------------------------------
        [HttpGet("edit/{stayId:int}")]
        public async Task<IActionResult> Edit(int stayId)
        {
            var stay = await _stayService.Stays.GetByIdAsync(stayId);
            if (stay is null)
                return NotFound();

            // Domain → ViewModel
            var model = new UpdateStayViewModel
            {
                StayId = stay.StayId,
                CustomerIdentityId = stay.CustomerIdentityId,
                RoomNo = stay.RoomNo,
                CheckInAt = stay.CheckInAt,
                AmountPaid = stay.AmountPaid,
                PendingAmount = stay.PendingAmount
            };

            return View(model);
        }

        // --------------------------------------------------
        // e. UPDATE CHECKED-IN CUSTOMER (POST)
        // --------------------------------------------------
        [HttpPost("edit/{stayId:int}")]
        public async Task<IActionResult> Edit(
            int stayId,
            UpdateStayViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // ViewModel → DTO
            var request = new UpdateStayRequest
            {
                CustomerIdentityId = model.CustomerIdentityId,
                RoomNo = model.RoomNo,
                CheckInAt = model.CheckInAt,
                AmountPaid = model.AmountPaid,
                PendingAmount = model.PendingAmount
            };

            await _stayService.Stays.UpdateAsync(stayId, request);
            return RedirectToAction("Index");
        }

        // --------------------------------------------------
        // d. CHECK-OUT CUSTOMER (GET)
        // --------------------------------------------------
        [HttpGet("checkout/{stayId:int}")]
        public async Task<IActionResult> CheckOut(int stayId)
        {
            var stay = await _stayService.Stays.GetByIdAsync(stayId);
            if (stay is null)
                return NotFound();

            // Domain → ViewModel
            var model = new CheckOutStayViewModel
            {
                StayId = stay.StayId,
                CustomerIdentityId = stay.CustomerIdentityId,
                RoomNo = stay.RoomNo,
                CheckInAt = stay.CheckInAt,
                CheckOutAt = DateTime.Now,
                AmountPaid = stay.AmountPaid,
                DepositPaid = stay.DepositPaid
            };

            return View(model);
        }

        // --------------------------------------------------
        // d. CHECK-OUT CUSTOMER (POST)
        // --------------------------------------------------
        [HttpPost("checkout/{stayId:int}")]
        public async Task<IActionResult> CheckOut(
            int stayId,
            CheckOutStayViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // ViewModel → DTO
            var request = new CheckOutRequest
            {
                CheckOutAt = model.CheckOutAt,
                AmountPaid = model.AmountPaid
            };

            await _stayService.Stays.CheckOutAsync(stayId, request);
            return RedirectToAction("Index");
        }

        // --------------------------------------------------
        // CHECK-IN CUSTOMER (GET)
        // --------------------------------------------------
        [HttpGet("checkin")]
        public async Task<IActionResult> CheckIn(string? identityId)
        {
            var model = new CheckInStayViewModel
            {
                CheckInAt = DateTime.Now
            };

            // Case 1: coming from Customers list
            if (!string.IsNullOrWhiteSpace(identityId))
            {
                var customer = await _stayService.Customers
                    .GetByIdentityIdAsync(identityId);

                if (customer == null)
                    return NotFound();

                model.CustomerIdentityId = customer.IdentityId;
                model.CustomerFound = true;
            }

            return View(model);
        }

        // --------------------------------------------------
        // CHECK-IN CUSTOMER (POST)
        // --------------------------------------------------
        [HttpPost("checkin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn(CheckInStayViewModel model, string action)
        {
            // -------------------------------
            // SEARCH CUSTOMER
            // -------------------------------
            if (action == "search")
            {
                if (string.IsNullOrWhiteSpace(model.SearchIdentityId))
                {
                    ModelState.AddModelError("", "Please enter Identity ID");
                    return View(model);
                }

                var customer = await _stayService.Customers
                    .GetByIdentityIdAsync(model.SearchIdentityId);

                if (customer == null)
                {
                    ModelState.AddModelError("", "Customer not found");
                    return View(model);
                }

                model.CustomerIdentityId = customer.IdentityId;
                model.CustomerFound = true;
                if (model.CheckInAt == null)
                {
                    model.CheckInAt = DateTime.Now;
                }

                return View(model);
            }

            // -------------------------------
            // FINAL CHECK-IN
            // -------------------------------
            if (!ModelState.IsValid || !model.CustomerFound)
                return View(model);

            var request = new CheckInRequest
            {
                CustomerIdentityId = model.CustomerIdentityId!,
                RoomNo = model.RoomNo,
                CheckInAt = model.CheckInAt,
                DepositPaid = model.DepositPaid
            };

            await _stayService.Stays.CheckInAsync(request);

            return RedirectToAction("Index");
        }

    }
}   