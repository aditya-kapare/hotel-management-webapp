using HotelManagement.WebApp.Application.Dtos.DropPickRequests;
using HotelManagement.WebApp.Application.Interfaces.Facades;
using HotelManagement.WebApp.ViewModels.DropPickRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

[Route("cabservice")]
public sealed class DropPickRequestsController : Controller
{
    private readonly IReceptionistServiceFacade _receptionist;

    public DropPickRequestsController(IReceptionistServiceFacade receptionist)
    {
        _receptionist = receptionist;
    }

    // =====================================================
    // INDEX
    // =====================================================
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var requests = await _receptionist.DropPickRequests.GetAllAsync();
        var stays = await _receptionist.Stays.GetAllAsync();
        var customers = await _receptionist.Customers.GetAllAsync();

        var model = requests.Select(r =>
        {
            var stay = stays.FirstOrDefault(s => s.StayId == r.StayId);
            var customer = customers
                .FirstOrDefault(c => c.IdentityId == stay?.CustomerIdentityId);

            return new DropPickRequestViewListModel
            {
                RequestId = r.RequestId,
                RoomNo = stay?.RoomNo ?? 0,
                CustomerName = customer?.Name ?? "Unknown",

                // ✅ USE WHAT THE REQUEST ALREADY HAS
                DriverName = r.DriverName,

                RequestType = r.RequestType.ToString(),
                RequestedAt = r.RequestedAt,

                CanEdit = stay?.CheckOutAt == null
            };
        }).ToList();

        return View(model);
    }
    //====================================================
    // CREATE (GET)
    // =====================================================

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Drivers = (await _receptionist.DropPickRequests.GetAvailableDriversAsync())
            .Select(d => new SelectListItem(d.Name, d.DriverId.ToString()))
            .ToList();

        return View(new CreateDropPickRequestViewModel());
    }


    // =====================================================
    // CREATE (POST)
    // =====================================================
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateDropPickRequestViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _receptionist.DropPickRequests.CreateAsync(
            new CreateDropPickRequest
            {
                StayId = model.StayId,
                DriverId = model.DriverId,
                RequestType = model.RequestType,

                // ✅ FIXED: derive DateTime from selected time
                RequestedAt = DateTime.Today.Add(model.SelectedTime),

                Notes = model.Notes
            });

        return RedirectToAction(nameof(Index));
    }

    // =====================================================
    // EDIT
    // =====================================================
    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var req = await _receptionist.DropPickRequests.GetByIdAsync(id);
        if (req == null) return RedirectToAction(nameof(Index));

        var stay = await _receptionist.Stays.GetByIdAsync(req.StayId);
        if (stay?.CheckOutAt != null)
            return RedirectToAction(nameof(Index));

        ViewBag.Drivers = (await _receptionist.DropPickRequests.GetAvailableDriversAsync())
            .Select(d => new SelectListItem(d.Name, d.DriverId.ToString()))
            .ToList();

        return View(new UpdateDropPickRequestViewModel
        {
            RequestId = req.RequestId,
            DriverId = req.DriverId,
            RequestType = req.RequestType,
            RequestedAt = req.RequestedAt,
            Notes = req.Notes
        });
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateDropPickRequestViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _receptionist.DropPickRequests.UpdateAsync(id, new UpdateDropPickRequest
        {
            DriverId = model.DriverId,
            RequestType = model.RequestType,
            RequestedAt = model.RequestedAt,
            Notes = model.Notes
        });

        return RedirectToAction(nameof(Index));
    }

    // =====================================================
    // DELETE
    // =====================================================
    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _receptionist.DropPickRequests.DeleteAsync(id);
        TempData["Success"] = "Request deleted successfully";
        return RedirectToAction(nameof(Index));
    }
    
}