using HotelManagement.WebApp.Application.Dtos.Stays;
using HotelManagement.WebApp.Application.Interfaces.Facades;
using HotelManagement.WebApp.Domain.Enums;
using HotelManagement.WebApp.ViewModels.Stays;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public async Task<IActionResult> Index(
    string? customer,
    int? roomNo)
        {
            var stays = await _stayService.Stays.GetActiveAsync();

            if (!string.IsNullOrWhiteSpace(customer))
            {
                customer = customer.Trim().ToLower();

                stays = stays.Where(s =>
                    s.CustomerIdentityId.ToLower().Contains(customer) ||
                    s.CustomerName.ToLower().Contains(customer)
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

        // --------------------------------------------------
        // e. UPDATE CHECKED-IN CUSTOMER (GET)
        // --------------------------------------------------
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
                CustomerName = stay.CustomerName,
                MobileNo = stay.MobileNo,
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
                CustomerName = stay.CustomerName,
                MobileNo = stay.MobileNo,
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
        [HttpGet("checkin/{identityId}")]
        public async Task<IActionResult> CheckIn(string identityId)
        {
            var customer = await _stayService.Customers
                .GetByIdentityIdAsync(identityId);

            if (customer == null)
                return NotFound();

            var model = new CheckInStayViewModel
            {
                CustomerIdentityId = customer.IdentityId,
                CustomerName = customer.Name,
                MobileNo = customer.MobileNo,
                RoomTypes = Enum.GetNames(typeof(RoomType))
                    .Select(x => new SelectListItem(x, x)),
                AcOptions = Enum.GetNames(typeof(AcOption))
                    .Select(x => new SelectListItem(x, x)),
                AvailableRooms = Enumerable.Empty<SelectListItem>()
            };

            return View(model);
        }


        // --------------------------------------------------
        // CHECK-IN CUSTOMER (POST)
        // --------------------------------------------------

        [HttpPost("checkin/{identityId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn(string identityId, CheckInStayViewModel model)
        {
            // Always rebind dropdowns
            model.RoomTypes = Enum.GetNames(typeof(RoomType))
                .Select(x => new SelectListItem(x, x));

            model.AcOptions = Enum.GetNames(typeof(AcOption))
                .Select(x => new SelectListItem(x, x));

            if (!string.IsNullOrEmpty(model.RoomType) &&
                !string.IsNullOrEmpty(model.AcOption))
            {
                var rooms = await _stayService.Rooms.GetAllAsync();
                var availableRooms = rooms
                    .Where(r =>
                        r.AvailabilityStatus == AvailabilityStatus.Available &&
                        r.RoomType.ToString() == model.RoomType &&
                        r.AcOption.ToString() == model.AcOption)
                    .ToList();

                model.AvailableRooms = availableRooms
                    .Select(r => new SelectListItem(r.RoomNo.ToString(), r.RoomNo.ToString()));

                if (model.RoomNo.HasValue)
                {
                    var room = availableRooms.FirstOrDefault(r => r.RoomNo == model.RoomNo.Value);
                    if (room != null)
                        model.Price = room.Price;
                }
            }

            // FILTER
            if (model.ActionType == "Filter")
            {
                ModelState.Clear();
                return View(model);
            }

            // FINAL SUBMIT
            if (!ModelState.IsValid)
                return View(model);

            await _stayService.Stays.CheckInAsync(new CheckInRequest
            {
                CustomerIdentityId = identityId,
                RoomNo = model.RoomNo!.Value,
                CheckInAt = model.CheckInAt,
                DepositPaid = model.DepositPaid
            });

            TempData["Success"] = "Customer checked in successfully";
            return RedirectToAction("Index", "Stays");
        }


        [HttpGet("GetAvailableRooms")]
        public async Task<IActionResult> GetAvailableRooms(
    string roomType,
    string acOption)
        {
            var rooms = await _stayService.Rooms.GetAllAsync();

            var result = rooms
                .Where(r =>
                    r.AvailabilityStatus == AvailabilityStatus.Available &&
                    r.RoomType.ToString() == roomType &&
                    r.AcOption.ToString() == acOption)
                .Select(r => new
                {
                    roomNo = r.RoomNo
                })
                .ToList();

            return Json(result);
        }

        [HttpGet("GetRoomPrice")]
        public async Task<IActionResult> GetRoomPrice(int roomNo)
        {
            var rooms = await _stayService.Rooms.GetAllAsync();

            var room = rooms.FirstOrDefault(r => r.RoomNo == roomNo);

            if (room == null)
                return Json(new { price = 0 });

            return Json(new { price = room.Price });
        }
        // --------------------------------------------------
        // VIEW STAY DETAILS (GET)
        // --------------------------------------------------
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

        // --------------------------------------------------
        // List page - Past / Historical stays
        // --------------------------------------------------
        [HttpGet("history")]
        public async Task<IActionResult> History(
            string? customer,
            int? roomNo)
        {
            // Fetch NON-ACTIVE (checked-out) stays
            var stays = await _stayService.Stays.GetPastAsync();

            if (!string.IsNullOrWhiteSpace(customer))
            {
                customer = customer.Trim().ToLower();
                stays = stays.Where(s =>
                    s.CustomerIdentityId.ToLower().Contains(customer) ||
                    s.CustomerName.ToLower().Contains(customer)
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
    }
}   