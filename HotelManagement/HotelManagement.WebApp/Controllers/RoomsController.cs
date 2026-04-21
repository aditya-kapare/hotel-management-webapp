using HotelManagement.WebApp.Application.Dtos.Room;
using HotelManagement.WebApp.Application.Facades;
using HotelManagement.WebApp.Application.Interfaces.Facades;
using Microsoft.AspNetCore.Authorization;
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


        [HttpGet("/rooms")]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> PublicIndex(
            string? type,
            string? acOption,
            string? cleanStatus,
            string? availabilityStatus)
        {
            return await IndexInternal(
                type,
                acOption,
                cleanStatus,
                availabilityStatus,
                isAdmin: false
            );
        }


        [HttpGet("/admin/rooms")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminIndex(
            string? type,
            string? acOption,
            string? cleanStatus,
            string? availabilityStatus)
        {
            return await IndexInternal(
                type,
                acOption,
                cleanStatus,
                availabilityStatus,
                isAdmin: true
            );
        }

  

        private async Task<IActionResult> IndexInternal(
            string? type,
            string? acOption,
            string? cleanStatus,
            string? availabilityStatus,
            bool isAdmin)
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

            ViewBag.IsReadOnly = !isAdmin;

            return View("Index", rooms);
        }

       
        [HttpGet("/admin/rooms/create")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("/admin/rooms/create")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoomRequest model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _admin.Rooms.CreateAsync(model);
            }
            catch (InvalidOperationException)
            {
                ModelState.AddModelError("RoomNo", "Room number already exists.");
                return View(model);
            }

            TempData["Success"] = "Room added successfully";
            return RedirectToAction(nameof(AdminIndex));
        }

        [HttpGet("/admin/rooms/edit/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var room = await _admin.Rooms.GetByRoomNoAsync(id);
            if (room == null) return NotFound();

            ViewBag.RoomNo = id;

            return View(new UpdateRoomRequest
            {
                RoomType = room.RoomType,
                AcOption = room.AcOption,
                AvailabilityStatus = room.AvailabilityStatus,
                CleanStatus = room.CleanStatus,
                Price = room.Price
            });
        }

        [HttpPost("/admin/rooms/edit/{id:int}")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateRoomRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.RoomNo = id;
                return View(request);
            }

            await _admin.Rooms.UpdateAsync(id, request);
            return RedirectToAction(nameof(AdminIndex));
        }

        [HttpGet("/admin/rooms/delete/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _admin.Rooms.GetByRoomNoAsync(id);
            if (room == null) return NotFound();

            return View(room);
        }

        [HttpPost("/admin/rooms/delete/{id:int}")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _admin.Rooms.DeleteAsync(id);
            return RedirectToAction(nameof(AdminIndex));
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