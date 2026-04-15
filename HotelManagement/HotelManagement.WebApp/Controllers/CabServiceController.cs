using HotelManagement.WebApp.Application.Dtos.DropPickRequests;
using HotelManagement.WebApp.Application.Interfaces.Facades;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.WebApp.Controllers
{
    [Route("cabservice")]
    public sealed class CabServiceController : Controller
    {
        private readonly IReceptionistServiceFacade _reception;

        public CabServiceController(IReceptionistServiceFacade reception)
        {
            _reception = reception;
        }

        // ===================== INDEX =====================
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var requests = await _reception.DropPickRequests.GetAllAsync();
            return View(requests);
        }

        // ===================== CREATE (GET) =====================
        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            // ✅ ONLY ACTIVE STAYS
            var activeStays = (await _reception.Stays.GetAllAsync())
                .Where(s => s.CheckOutAt == null)
                .ToList();

            ViewBag.Stays = activeStays;
            ViewBag.Drivers =
                await _reception.DropPickRequests.GetAvailableDriversAsync();

            return View(new CreateDropPickRequest());
        }

        // ===================== CREATE (POST) =====================
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDropPickRequest request)
        {
            try
            {
                var created = await _reception.DropPickRequests.CreateAsync(request);

                TempData["SuccessMessage"] =
                    $"Cab request created successfully. Request ID: {created.RequestId}";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                ViewBag.Stays = (await _reception.Stays.GetAllAsync())
                    .Where(s => s.CheckOutAt == null)
                    .ToList();

                ViewBag.Drivers =
                    await _reception.DropPickRequests.GetAvailableDriversAsync();

                return View(request);
            }
        }

        // ===================== EDIT =====================
        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var req = await _reception.DropPickRequests.GetByIdAsync(id);
            if (req == null) return NotFound();

            ViewBag.Drivers =
                await _reception.DropPickRequests.GetAvailableDriversAsync();

            return View(new UpdateDropPickRequest
            {
                DriverId = req.DriverId,
                RequestType = req.RequestType,
                RequestedAt = req.RequestedAt,
                Notes = req.Notes
            });
        }
        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateDropPickRequest request)
        {
            try
            {
                await _reception.DropPickRequests.UpdateAsync(id, request);

                TempData["SuccessMessage"] = "Cab request updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                // ✅ THIS IS WHERE YOU SHOW CHECKOUT MESSAGE
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // ===================== DELETE =====================
        [HttpGet("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var req = await _reception.DropPickRequests.GetByIdAsync(id);
            if (req == null) return NotFound();

            return View(req);
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _reception.DropPickRequests.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


    }
}