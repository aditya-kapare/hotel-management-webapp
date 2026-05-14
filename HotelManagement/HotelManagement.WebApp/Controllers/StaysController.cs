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

            // ✅ Check if room is occupied by another stay
            var activeStays = await _stayService.Stays.GetActiveAsync();

            var roomOccupied = activeStays.Any(s =>
                s.RoomNo == model.RoomNo &&
                s.StayId != stayId); // allow same stay

            if (roomOccupied)
            {
                ModelState.AddModelError(nameof(model.RoomNo),
                    "Selected room is already occupied");

                return View(model); // ✅ NO exception
            }

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
                DepositPaid = stay.DepositPaid,
                RoomPrice = stay.RoomPrice
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

        // ✅ CHECK-IN GET
    

        //public async Task<IActionResult> CheckIn(string identityId)
        //{
        //    if (string.IsNullOrWhiteSpace(identityId))
        //        return BadRequest();

        //    // ✅ Load customer details
        //    var customer = await _stayService.Customers.GetByIdentityIdAsync(identityId);
        //    if (customer == null)
        //        return NotFound();

        //    var model = new CheckInStayViewModel
        //    {
        //        CustomerIdentityId = identityId,
        //        CustomerName = customer.Name,
        //        MobileNo = customer.MobileNo,
        //        CheckInAt = DateTime.Now,

        //        // ✅ STATIC ROOM TYPES (NO ROOM SERVICE)
        //        RoomTypes = new List<SelectListItem>
        //{
        //    new SelectListItem { Value = "Deluxe", Text = "Deluxe" },
        //    new SelectListItem { Value = "SemiDeluxe", Text = "Semi Deluxe" },
        //    new SelectListItem { Value = "DoubleBed", Text = "Double Bed" },
        //    new SelectListItem { Value = "SingleBed", Text = "Single Bed" }
        //},

        //        // ✅ STATIC AC OPTIONS (NO ROOM SERVICE)
        //        AcOptions = new List<SelectListItem>
        //{
        //    new SelectListItem { Value = "AC", Text = "AC" },
        //    new SelectListItem { Value = "NonAC", Text = "Non‑AC" }
        //}
        //    };

        //    return View(model);
        //}

        [HttpGet("checkin")]
        public async Task<IActionResult> CheckIn(string identityId)
        {
            var customer = await _stayService.Customers.GetByIdentityIdAsync(identityId);

            var model = new CheckInStayViewModel
            {
                CustomerIdentityId = identityId,
                CustomerName = customer.Name,
                MobileNo = customer.MobileNo,
                CheckInAt = DateTime.Now
            };

            ReloadCheckInDropdowns(model);
            return View(model);
        }

        [HttpGet("GetAvailableRooms")]
        public async Task<IActionResult> GetAvailableRooms(string roomType, string acOption)
        {
            if (string.IsNullOrWhiteSpace(roomType) || string.IsNullOrWhiteSpace(acOption))
                return Ok(new List<object>());

            // ✅ Convert UI strings → enum → int
            if (!Enum.TryParse<RoomType>(roomType, true, out var parsedRoomType))
                return Ok(new List<object>());

            if (!Enum.TryParse<AcOption>(acOption, true, out var parsedAcOption))
                return Ok(new List<object>());

            var rooms = await _stayService.Rooms.GetAllAsync();

            var availableRooms = rooms
                .Where(r =>
                    r.AvailabilityStatus == (int)AvailabilityStatus.Available &&
                    r.RoomType == (int)parsedRoomType &&
                    r.AcOption == (int)parsedAcOption
                )
                .Select(r => new
                {
                    roomNo = r.RoomNo
                })
                .ToList();

            return Ok(availableRooms);
        }


        // ✅ CHECK-IN POST
        [HttpPost("checkin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn(CheckInStayViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ReloadCheckInDropdowns(model);
                return View(model);
            }

            if (!model.RoomNo.HasValue)
            {
                ModelState.AddModelError(nameof(model.RoomNo), "Please select a room");
                ReloadCheckInDropdowns(model);
                return View(model);
            }

            await _stayService.Stays.CheckInAsync(new CheckInRequest
            {
                CustomerIdentityId = model.CustomerIdentityId,
                RoomNo = model.RoomNo.Value,
                CheckInAt = model.CheckInAt,
                DepositPaid = model.DepositPaid
            });

            TempData["Success"] = "Customer checked in successfully";
            return RedirectToAction("Index");
        }
        private void ReloadCheckInDropdowns(CheckInStayViewModel model)
        {
            model.RoomTypes = new List<SelectListItem>
    {
        new SelectListItem { Value = "Deluxe", Text = "Deluxe" },
        new SelectListItem { Value = "SemiDeluxe", Text = "Semi Deluxe" },
        new SelectListItem { Value = "DoubleBed", Text = "Double Bed" },
        new SelectListItem { Value = "SingleBed", Text = "Single Bed" }
    };

            model.AcOptions = new List<SelectListItem>
    {
        new SelectListItem { Value = "AC", Text = "AC" },
        new SelectListItem { Value = "NonAC", Text = "Non‑AC" }
    };
        }
        [HttpPost("checkin/load-rooms")]
        public async Task<IActionResult> LoadRooms(CheckInStayViewModel model)
        {
            ReloadCheckInDropdowns(model);

            if (!string.IsNullOrEmpty(model.RoomType) &&
                !string.IsNullOrEmpty(model.AcOption))
            {
                var rooms = await _stayService.Rooms.GetAllAsync();

                var availableRooms = rooms
                    .Where(r =>
                        r.AvailabilityStatus == (int)AvailabilityStatus.Available &&
                        r.RoomType == (int)Enum.Parse<RoomType>(model.RoomType) &&
                        r.AcOption == (int)Enum.Parse<AcOption>(model.AcOption))
                    .Select(r => new SelectListItem
                    {
                        Value = r.RoomNo.ToString(),
                        Text = $"Room {r.RoomNo}"
                    })
                    .ToList();

                model.AvailableRooms = availableRooms;
            }

            return View("CheckIn", model);
        }
    }
}





