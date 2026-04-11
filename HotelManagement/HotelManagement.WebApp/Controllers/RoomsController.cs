using HotelManagement.WebApp.Application.Dtos.Room;
using HotelManagement.WebApp.Application.Interfaces.Facades;
using HotelManagement.WebApp.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.WebApp.Controllers
{
    public class RoomsController : Controller
    {
        private readonly IAdminServiceFacade _admin;

        public RoomsController(IAdminServiceFacade admin)
        {
            _admin = admin;
        }

        // Common: Admin + Receptionist
        //public async Task<IActionResult> Index(string? type)
        //{
        //    var rooms = string.IsNullOrEmpty(type)
        //        ? await _admin.Rooms.GetAllAsync()
        //        : Enum.TryParse<RoomType>(type, out var parsed)
        //            ? await _admin.Rooms.GetByTypeAsync(parsed)
        //            : await _admin.Rooms.GetAllAsync();

        //    // Prepare UI data (no enum exposure to view)
        //    ViewBag.RoomTypes = rooms
        //        .Select(r => r.RoomType.ToString())
        //        .Distinct()
        //        .ToList();

        //    ViewBag.SelectedType = type;

        //    return View(rooms);
        //}

        public async Task<IActionResult> Index(
        string? type,
        string? acOption,
        string? cleanStatus,
        string? availabilityStatus)
        {
            var rooms = await _admin.Rooms.GetAllAsync();

            if (!string.IsNullOrEmpty(type))
                rooms = rooms.Where(r => r.RoomType.ToString() == type).ToList();

            if (!string.IsNullOrEmpty(acOption))
                rooms = rooms.Where(r => r.AcOption.ToString() == acOption).ToList();

            if (!string.IsNullOrEmpty(cleanStatus))
                rooms = rooms.Where(r => r.CleanStatus.ToString() == cleanStatus).ToList();

            if (!string.IsNullOrEmpty(availabilityStatus))
                rooms = rooms.Where(r => r.AvailabilityStatus.ToString() == availabilityStatus).ToList();

            PrepareFilters(rooms, type, acOption, cleanStatus, availabilityStatus);

            return View(rooms);
        }
        // Admin only
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoomRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            await _admin.Rooms.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }

        // Admin only
        public async Task<IActionResult> Edit(int id)
        {
            var room = await _admin.Rooms.GetByRoomNoAsync(id);
            if (room == null) return NotFound();

            var model = new UpdateRoomRequest
            {
                RoomType = room.RoomType,
                AcOption = room.AcOption,
                AvailabilityStatus = room.AvailabilityStatus,
                CleanStatus = room.CleanStatus,
                Price = room.Price
            };

            ViewBag.RoomNo = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateRoomRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.RoomNo = id;
                return View(request);
            }

            await _admin.Rooms.UpdateAsync(id, request);
            return RedirectToAction(nameof(Index));
        }

        // Admin only
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _admin.Rooms.GetByRoomNoAsync(id);
            if (room == null) return NotFound();

            return View(room);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _admin.Rooms.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private void PrepareFilters(
         IEnumerable<RoomDto> rooms,
         string? type,
         string? acOption,
         string? cleanStatus,
         string? availabilityStatus)
        {
            ViewBag.RoomTypes = rooms
                .Select(r => r.RoomType.ToString())
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            ViewBag.AcOptions = new[] { "AC", "NonAC" };
            ViewBag.CleanStatuses = new[] { "Clean", "Dirty" };
            ViewBag.AvailabilityStatuses = new[] { "Available", "Occupied" };

            ViewBag.SelectedType = type;
            ViewBag.SelectedAc = acOption;
            ViewBag.SelectedClean = cleanStatus;
            ViewBag.SelectedAvailability = availabilityStatus;
        }
    }
}