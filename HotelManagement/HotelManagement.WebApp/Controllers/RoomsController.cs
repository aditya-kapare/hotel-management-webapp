using HotelManagement.WebApp.Application.Dtos.Room;
using HotelManagement.WebApp.Application.Interfaces.Facades;
using HotelManagement.WebApp.Domain.Enums;
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
                isAdmin: false);
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
                isAdmin: true);
        }

        private async Task<IActionResult> IndexInternal(
            string? type,
            string? acOption,
            string? cleanStatus,
            string? availabilityStatus,
            bool isAdmin)
        {
            var rooms = await _admin.Rooms.GetAllAsync();

            int? selectedType = int.TryParse(type, out var t) ? t : null;
            int? selectedAc = int.TryParse(acOption, out var ac) ? ac : null;
            int? selectedClean = int.TryParse(cleanStatus, out var cs) ? cs : null;
            int? selectedAvailability = int.TryParse(availabilityStatus, out var av) ? av : null;

            if (selectedType.HasValue)
                rooms = rooms.Where(r => r.RoomType == selectedType.Value).ToList();

            if (selectedAc.HasValue)
                rooms = rooms.Where(r => r.AcOption == selectedAc.Value).ToList();

            if (selectedClean.HasValue)
                rooms = rooms.Where(r => r.CleanStatus == selectedClean.Value).ToList();

            if (selectedAvailability.HasValue)
                rooms = rooms.Where(r => r.AvailabilityStatus == selectedAvailability.Value).ToList();

            PrepareFilters(
                selectedType,
                selectedAc,
                selectedClean,
                selectedAvailability);

            ViewBag.IsReadOnly = !isAdmin;
            return View("Index", rooms);
        }

        // -------------------------
        // CREATE
        // -------------------------
        [HttpGet("/admin/rooms/create")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("/admin/rooms/create")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomForCreationDTO model)
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

        // -------------------------
        // EDIT
        // -------------------------
        [HttpGet("/admin/rooms/edit/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var room = await _admin.Rooms.GetByRoomNoAsync(id);
            if (room == null) return NotFound();

            ViewBag.RoomNo = id;

            return View(new RoomForUpdateDTO(
                room.RoomType,
                room.AcOption,
                room.AvailabilityStatus,
                room.CleanStatus,
                room.Price));
        }

        [HttpPost("/admin/rooms/edit/{id:int}")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RoomForUpdateDTO request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.RoomNo = id;
                return View(request);
            }

            try
            {
                await _admin.Rooms.UpdateAsync(id, request);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(
                    nameof(request.Price),
                    "Room update failed due to a business rule violation."
                );

                ViewBag.RoomNo = id;
                return View(request);
            }

            return RedirectToAction(nameof(AdminIndex));
        }

        // -------------------------
        // DELETE
        // -------------------------
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

        // -------------------------
        // FILTER PREP (CabDriver-style ✅)
        // -------------------------
        private void PrepareFilters(
            int? selectedType,
            int? selectedAc,
            int? selectedClean,
            int? selectedAvailability)
        {
            ViewBag.RoomTypes = Enum.GetValues(typeof(RoomType));
            ViewBag.AcOptions = Enum.GetValues(typeof(AcOption));
            ViewBag.CleanStatuses = Enum.GetValues(typeof(CleanStatus));
            ViewBag.AvailabilityStatuses = Enum.GetValues(typeof(AvailabilityStatus));

            ViewBag.SelectedType = selectedType;
            ViewBag.SelectedAc = selectedAc;
            ViewBag.SelectedClean = selectedClean;
            ViewBag.SelectedAvailability = selectedAvailability;
        }
    }
}