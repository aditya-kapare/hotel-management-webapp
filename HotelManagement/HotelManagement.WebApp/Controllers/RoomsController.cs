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
        public async Task<IActionResult> Index(RoomType? type)
        {
            var rooms = type is null
                ? await _admin.Rooms.GetAllAsync()
                : await _admin.Rooms.GetByTypeAsync(type.Value);

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
    }
}