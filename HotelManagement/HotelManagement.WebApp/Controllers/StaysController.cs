using HotelManagement.WebApp.Application.Dtos.Stays;
using HotelManagement.WebApp.Application.Interfaces.Facades;
using HotelManagement.WebApp.Domain.Enums;
using HotelManagement.WebApp.ViewModels.Stays;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace HotelManagement.WebApp.Controllers
{
    [Route("stays")]
    public sealed class StaysController : Controller
    {
        private readonly IReceptionistServiceFacade _stayService;

        public StaysController(IReceptionistServiceFacade stayService)
        {
            _stayService = stayService;
        }

        [HttpGet("")]
        public IActionResult Home()
        {
            return View();
        }

        // ✅ LIST
        [HttpGet("list")]
        public async Task<IActionResult> Index(string? customer, int? roomNo)
        {
            var stays = await _stayService.Stays.GetActiveAsync();

            if (!string.IsNullOrWhiteSpace(customer))
            {
                customer = customer.Trim().ToLower();

                stays = stays.Where(s =>
                    s.CustomerIdentityId.ToLower().Contains(customer) ||
                    (s.Customer != null && s.Customer.Name.ToLower().Contains(customer))
                ).ToList();
            }

            if (roomNo.HasValue)
            {
                stays = stays.Where(s => s.RoomNo == roomNo.Value).ToList();
            }

            ViewBag.Customer = customer;
            ViewBag.RoomNo = roomNo;

            return View(stays);
        }

        // ✅ EDIT GET
        [HttpGet("edit/{stayId:int}")]
        public async Task<IActionResult> Edit(int stayId)
        {
            var stay = await _stayService.Stays.GetByIdAsync(stayId);
            if (stay is null)
                return NotFound();

            var model = new UpdateStayViewModel
            {
                StayId = stay.StayId,
                CustomerIdentityId = stay.CustomerIdentityId,
                CustomerName = stay.Customer?.Name,
                MobileNo = stay.Customer?.MobileNo,
                RoomNo = stay.RoomNo,
                CheckInAt = stay.CheckInAt,
                AmountPaid = stay.AmountPaid,
                PendingAmount = stay.PendingAmount
            };

            return View(model);
        }

        // ✅ EDIT POST
        [HttpPost("edit/{stayId:int}")]
        public async Task<IActionResult> Edit(int stayId, UpdateStayViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

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

        // ✅ CHECKOUT GET
        [HttpGet("checkout/{stayId:int}")]
        public async Task<IActionResult> CheckOut(int stayId)
        {
            var stay = await _stayService.Stays.GetByIdAsync(stayId);
            if (stay is null)
                return NotFound();

            var model = new CheckOutStayViewModel
            {
                StayId = stay.StayId,
                CustomerIdentityId = stay.CustomerIdentityId,
                CustomerName = stay.Customer?.Name,
                MobileNo = stay.Customer?.MobileNo,
                RoomNo = stay.RoomNo,
                CheckInAt = stay.CheckInAt,
                CheckOutAt = DateTime.Now,
                AmountPaid = stay.AmountPaid,
                DepositPaid = stay.DepositPaid
            };

            var rooms = await _stayService.Rooms.GetAllAsync();
            var room = rooms.FirstOrDefault(r => r.RoomNo == stay.RoomNo);
            ViewBag.RoomPrice = room?.Price ?? 0;

            return View(model);
        }

        // ✅ CHECKOUT POST
        [HttpPost("checkout/{stayId:int}")]
        public async Task<IActionResult> CheckOut(int stayId, CheckOutStayViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var rooms = await _stayService.Rooms.GetAllAsync();
            var room = rooms.FirstOrDefault(r => r.RoomNo == model.RoomNo);

            if (room == null)
            {
                ModelState.AddModelError("", "Room not found");
                return View(model);
            }

            var totalAmount = room.Price;

            if (model.AmountPaid > totalAmount)
            {
                TempData["Error"] = "Amount paid cannot exceed room price";
                return RedirectToAction(nameof(CheckOut), new { stayId });
            }

            if (model.AmountPaid < totalAmount)
            {
                TempData["Error"] = "Amount paid is less than required";
                return RedirectToAction(nameof(CheckOut), new { stayId });
            }

            var request = new CheckOutRequest
            {
                CheckOutAt = model.CheckOutAt,
                AmountPaid = model.AmountPaid
            };

            await _stayService.Stays.CheckOutAsync(stayId, request);
            return RedirectToAction("Index");
        }

        // ✅ HISTORY
        [HttpGet("history")]
        public async Task<IActionResult> History(string? customer, int? roomNo)
        {
            var stays = await _stayService.Stays.GetPastAsync();

            if (!string.IsNullOrWhiteSpace(customer))
            {
                customer = customer.Trim().ToLower();

                stays = stays.Where(s =>
                    s.CustomerIdentityId.ToLower().Contains(customer) ||
                    (s.Customer != null && s.Customer.Name.ToLower().Contains(customer))
                ).ToList();
            }

            if (roomNo.HasValue)
            {
                stays = stays.Where(s => s.RoomNo == roomNo.Value).ToList();
            }

            ViewBag.Customer = customer;
            ViewBag.RoomNo = roomNo;

            return View(stays);
        }

        // ✅ DETAILS
        [HttpGet("details/{stayId:int}")]
        public async Task<IActionResult> ViewDetails(int stayId)
        {
            if (stayId <= 0)
                return BadRequest();

            var stay = await _stayService.Stays.GetByIdAsync(stayId);

            if (stay is null)
                return NotFound();

            return View(stay);
        }

        // ✅ FORCE CHECKIN
        [HttpPost("force-checkin")]
        public async Task<IActionResult> ForceCheckIn()
        {
            var model = JsonSerializer.Deserialize<CheckInStayViewModel>(
                TempData["PendingCheckIn"]!.ToString()!);

            await _stayService.Stays.ForceCheckInAsync(new CheckInRequest
            {
                CustomerIdentityId = model.CustomerIdentityId,
                RoomNo = model.RoomNo!.Value,
                CheckInAt = model.CheckInAt,
                DepositPaid = model.DepositPaid
            });

            TempData["Success"] = "Customer checked in (room auto-cleaned)";
            return RedirectToAction("Index", "Stays");
        }
    }
}