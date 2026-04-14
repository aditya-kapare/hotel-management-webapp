using HotelManagement.WebApp.Application.Dtos.Stays;
using HotelManagement.WebApp.Application.Interfaces.Facades;
using HotelManagement.WebApp.ViewModels.Stays;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.WebApp.Controllers
{
    [Authorize(Roles = "Receptionist")]
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
    }
}   