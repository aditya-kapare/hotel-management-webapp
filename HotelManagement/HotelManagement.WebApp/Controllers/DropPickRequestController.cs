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
        var requests = await _receptionist.DropPickRequests.GetRequestListAsync();

        var model = requests.Select(r => new DropPickRequestViewListModel
        {
            RequestId = r.RequestId,
            RequestedAt = r.RequestedAt,
            RequestType = r.RequestType.ToString(),
            Notes = r.Notes,

            StayId = r.StayId,
            DriverId = r.DriverId,
            
            RoomNo = r.RoomNo,
            
            DriverName = r.DriverName,
            
            CustomerName = r.CustomerName,
            CustomerMobileNo = r.CustomerPhone,
            
            CanEdit = r.CanEdit

        }).ToList();

        return View(model);
    }
    //====================================================
    // CREATE (GET)
    // =====================================================

    //[HttpGet("create")]
    //public async Task<IActionResult> Create()
    //{
    //    ViewBag.Drivers = (await _receptionist.DropPickRequests.GetAvailableDriversAsync())
    //        .Select(d => new SelectListItem(d.Name, d.DriverId.ToString()))
    //        .ToList();

    //    return View(new DropPickRequestViewListModel());
    //}

    [HttpGet("create")]
    public async Task<IActionResult> Create(int? stayId, int? identityId)
    {
        var model = new DropPickRequestViewListModel();

        // 1️⃣ If stayId is provided → load stay + customer
        if (stayId.HasValue && stayId.Value > 0)
        {
            var stay = await _receptionist.Stays.GetByIdAsync(stayId.Value);
            if (stay != null)
            {
                model.StayId = stay.StayId;
                model.RoomNo = stay.RoomNo;

                var customer = await _receptionist.Customers.GetByIdentityIdAsync(stay.CustomerIdentityId);

                if (customer != null)
                {
                    model.CustomerName = customer.Name;
                    model.CustomerMobileNo = customer.MobileNo;
                }
            }
        }
        // 2️⃣ Else if only customer identity is provided → load customer only
        else if (identityId.HasValue && identityId.Value > 0)
        {
            var customer = await _receptionist.Customers.GetByIdentityIdAsync(identityId.Value.ToString());

            if (customer != null)
            {
                model.CustomerName = customer.Name;
                model.CustomerMobileNo = customer.MobileNo;
            }
        }

        ViewBag.Drivers = (await _receptionist.DropPickRequests.GetAvailableDriversAsync())
            .Select(d => new SelectListItem(d.Name, d.DriverId.ToString()))
            .ToList();

        return View(model);
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

    // =====================================================
    // DETAILS
    // =====================================================
    [HttpGet("details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var requests = await _receptionist
            .DropPickRequests
            .GetRequestListAsync();

        var dto = requests.FirstOrDefault(r => r.RequestId == id);
        if (dto == null)
            return RedirectToAction(nameof(Index));

        var model = new DropPickRequestViewListModel
        {
            RequestId = dto.RequestId,
            StayId = dto.StayId,
            DriverId = dto.DriverId,

            RoomNo = dto.RoomNo,

            CustomerName = dto.CustomerName,
            CustomerMobileNo = dto.CustomerPhone,

            DriverName = dto.DriverName,

            RequestType = dto.RequestType.ToString(),
            RequestedAt = dto.RequestedAt,
            Notes = dto.Notes,

            CanEdit = dto.CanEdit
        };

        return View(model);
    }


    [HttpPost("create/load-stay")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoadStay(DropPickRequestViewListModel model)
    {
        if (model.StayId <= 0)
        {
            ModelState.AddModelError(nameof(model.StayId), "Enter a valid Stay ID");
            return await ReloadCreateView(model);
        }

        var stay = await _receptionist.Stays.GetByIdAsync(model.StayId);
        if (stay == null)
        {
            ModelState.AddModelError(nameof(model.StayId), "Stay not found");
            return await ReloadCreateView(model);
        }

        var customer = await _receptionist.Customers.GetByIdentityIdAsync(stay.CustomerIdentityId);

        model.RoomNo = stay.RoomNo;
        model.CustomerName = customer?.Name ?? "Unknown";
        model.CustomerMobileNo = customer?.MobileNo ?? string.Empty;

        return await ReloadCreateView(model);
    }

    private async Task<IActionResult> ReloadCreateView(DropPickRequestViewListModel model)
    {
        ViewBag.Drivers = (await _receptionist.DropPickRequests.GetAvailableDriversAsync())
            .Select(d => new SelectListItem(d.Name, d.DriverId.ToString()))
            .ToList();

        return View("Create", model);
    }

    [HttpPost("create/load-customer")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoadCustomer(DropPickRequestViewListModel model)
    {
        if (string.IsNullOrWhiteSpace(model.CustomerMobileNo))
        {
            ModelState.AddModelError(
                nameof(model.CustomerMobileNo),
                "Enter a valid customer mobile number");
            return await ReloadCreateView(model);
        }

        // 1️⃣ Find customer (no stay requirement)
        var customer = (await _receptionist.Customers.GetAllAsync())
            .FirstOrDefault(c => c.MobileNo == model.CustomerMobileNo);

        if (customer == null)
        {
            ModelState.AddModelError(
                nameof(model.CustomerMobileNo),
                "Customer not found");
            return await ReloadCreateView(model);
        }

        // 2️⃣ Populate ONLY customer details
        model.CustomerName = customer.Name;
        model.CustomerMobileNo = customer.MobileNo;

        // ⚠️ StayId intentionally NOT resolved here

        return await ReloadCreateView(model);
    }
}