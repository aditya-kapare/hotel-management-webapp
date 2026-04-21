using Azure.Core;
using HotelManagement.WebApp.Application.Dtos.DropPickRequests;
using HotelManagement.WebApp.Application.Interfaces.Facades;
using HotelManagement.WebApp.Domain.Enums;
using HotelManagement.WebApp.Domain.Models;
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
    public IActionResult Home()
    {
        return View("Home");   // ✅ NO MODEL
    }


    [HttpGet("list")]
    public async Task<IActionResult> Index()
    {
        var requests = await _receptionist
            .DropPickRequests
            .GetRequestListAsync();

        var model = requests
            .Where(r => r.RequestStatus != DropPickStatus.Cancelled)
            .Select(r => new DropPickRequestViewListModel
            {
                RequestId = r.RequestId,
                RoomNo = r.RoomNo,
                CustomerName = r.CustomerName,
                CustomerMobileNo = r.CustomerPhone,
                DriverName = r.DriverName,
                Status = r.RequestStatus,
                CanEdit = r.CanEdit
            })
            .ToList();

        return View("Index", model);
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create(int? stayId, int? identityId)
    {
        var model = new CreateDropPickRequestCompositeViewModel();

        // 1️⃣ If stayId is provided → load stay + customer
        if (stayId.HasValue && stayId.Value > 0)
        {
            var stay = await _receptionist.Stays.GetByIdAsync(stayId.Value);
            if (stay != null)
            {
                if (stay.CheckOutAt != null)
                {
                    TempData["Error"] = "Cab cannot be booked after customer check-out";
                    return RedirectToAction("Index", "Customers");
                }
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
        //else if (identityId.HasValue && identityId.Value > 0)
        //{
        //    var customer = await _receptionist.Customers.GetByIdentityIdAsync(identityId.Value.ToString());

        //    if (customer != null)
        //    {
        //        model.CustomerName = customer.Name;
        //        model.CustomerMobileNo = customer.MobileNo;
        //    }
        //}

        ViewBag.Drivers = (await _receptionist.DropPickRequests.GetAvailableDriversAsync())
            .Select(d => new SelectListItem(d.Name, d.DriverId.ToString()))
            .ToList();

        return View(model);
    }

    //[HttpPost("create")]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Create(CreateDropPickRequestViewModel model)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        // 🔁 Map Create VM → List VM (required for Create view)
    //        var viewModel = new DropPickRequestViewListModel
    //        {
    //            StayId = model.StayId,
    //            RoomNo = model.RoomNo,
    //            CustomerName = model.CustomerName,
    //            DriverId = model.DriverId,
    //            Notes = model.Notes,
    //            RequestType = model.RequestType.ToString()
    //        };

    //        ViewBag.Drivers = (await _receptionist
    //            .DropPickRequests
    //            .GetAvailableDriversAsync())
    //            .Select(d => new SelectListItem(d.Name, d.DriverId.ToString()))
    //            .ToList();

    //        return View("Create", viewModel);
    //    }

    //    await _receptionist.DropPickRequests.CreateAsync(
    //        new CreateDropPickRequest
    //        {
    //            StayId = model.StayId,
    //            DriverId = model.DriverId,
    //            RequestType = model.RequestType,
    //            RequestedAt = DateTime.Today.Add(model.SelectedTime),
    //            Notes = model.Notes
    //        });

    //    return RedirectToAction(nameof(Index));
    //}

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateDropPickRequestCompositeViewModel model)
    {
        if (!ModelState.IsValid)
            return await ReloadCreateView(model);

        try
        {
            await _receptionist.DropPickRequests.CreateAsync(
                new DropPickRequest
                {
                    StayId = model.StayId,
                    DriverId = model.DriverId,
                    RequestType = model.RequestType,
                    RequestedAt = model.RequestedAt,
                    Notes = model.Notes
                });
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
            return await ReloadCreateView(model);
        }

        return RedirectToAction(nameof(Index));
    }


    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var req = await _receptionist.DropPickRequests.GetRequestByIdAsync(id);
        if (req == null)
            return RedirectToAction(nameof(Index));

        //var customer = await _receptionist.Customers.GetByIdentityIdAsync(req.CustomerPhone);
        //var driver = await _receptionist.Drivers.GetByIdAsync(req.DriverId);

        if (req.RequestStatus is DropPickStatus.Completed or DropPickStatus.Cancelled)
            return RedirectToAction(nameof(Index));

        // 1. Get available drivers
        var availableDrivers = (await _receptionist
            .DropPickRequests
            .GetAvailableDriversAsync())
            .Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.DriverId.ToString()
            })
            .ToList();

        // 2. Check if current driver is already in available list
        bool currentDriverExists = availableDrivers
            .Any(d => d.Value == req.DriverId.ToString());

        // 3. If not, add current driver at top
        if (!currentDriverExists && req.DriverId != null)
        {
            availableDrivers.Insert(0, new SelectListItem
            {
                Text = $"{req.DriverName} (Current)",
                Value = req.DriverId.ToString(),
                Selected = true
            });
        }
        else
        {
            // 4. If exists, just mark it selected
            availableDrivers
                .First(d => d.Value == req.DriverId.ToString())
                .Selected = true;
        }

        ViewBag.Drivers = availableDrivers;

        return View(new UpdateDropPickRequestViewModel
        {
            RequestId = req.RequestId,
            DriverId = req.DriverId,
            RequestType = req.RequestType,
            Status = req.RequestStatus,
            RequestedAt = req.RequestedAt,

            CustomerName = req.CustomerName,
            CustomerPhone = req.CustomerPhone,
            CurrentDriverName = req.DriverName,

            Notes = req.Notes
        });
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateDropPickRequestViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var drivers = (await _receptionist
    .DropPickRequests
    .GetAvailableDriversAsync())
    .Select(d => new SelectListItem
    {
        Text = d.Name,
        Value = d.DriverId.ToString(),
        Selected = d.DriverId == model.DriverId
    })
    .ToList();

ViewBag.Drivers = drivers;

            return View(model);
        }

        await _receptionist.DropPickRequests.UpdateAsync(
            id,
            new UpdateDropPickRequest
            {
                RequestId = id,
                DriverId = model.DriverId,
                RequestType = model.RequestType,
                RequestedAt = model.RequestedAt,
                Status = model.Status,
                Notes = model.Notes
            });

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _receptionist.DropPickRequests.DeleteAsync(id);
        TempData["Success"] = "Request cancelled successfully";
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
    public async Task<IActionResult> LoadStay(CreateDropPickRequestCompositeViewModel model)
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

        var customer = await _receptionist
            .Customers
            .GetByIdentityIdAsync(stay.CustomerIdentityId);

        model.RoomNo = stay.RoomNo;
        model.CustomerName = customer?.Name ?? "Unknown";
        model.CustomerMobileNo = customer?.MobileNo ?? string.Empty;

        return await ReloadCreateView(model);
    }

    private async Task<IActionResult> ReloadCreateView(
    CreateDropPickRequestCompositeViewModel model)
    {
        ViewBag.Drivers = (await _receptionist
            .DropPickRequests
            .GetAvailableDriversAsync())
            .Select(d => new SelectListItem(d.Name, d.DriverId.ToString()))
            .ToList();

        return View("Create", model);
    }
    [HttpGet("history")]
    public async Task<IActionResult> History()
    {
        var requests = await _receptionist
            .DropPickRequests
            .GetRequestListAsync();

        var model = requests
            .Where(r => (r.RequestStatus == DropPickStatus.Cancelled) || (r.RequestStatus == DropPickStatus.Completed))
            .Select(r => new DropPickRequestViewListModel
            {
                RequestId = r.RequestId,
                RoomNo = r.RoomNo,
                CustomerName = r.CustomerName,
                CustomerMobileNo = r.CustomerPhone,
                DriverName = r.DriverName,
                Status = r.RequestStatus,
                CanEdit = false // 🔒 IMPORTANT
            })
            .ToList();

        return View("History", model);
    }


    //[HttpPost("create/load-customer")]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> LoadCustomer(CreateDropPickRequestCompositeViewModel model)
    //{
    //    if (string.IsNullOrWhiteSpace(model.CustomerMobileNo))
    //    {
    //        ModelState.AddModelError(
    //            nameof(model.CustomerMobileNo),
    //            "Enter a valid customer mobile number");
    //        return await ReloadCreateView(model);
    //    }

    //    var customer = (await _receptionist.Customers.GetAllAsync())
    //        .FirstOrDefault(c => c.MobileNo == model.CustomerMobileNo);

    //    if (customer == null)
    //    {
    //        ModelState.AddModelError(
    //            nameof(model.CustomerMobileNo),
    //            "Customer not found");
    //        return await ReloadCreateView(model);
    //    }

    //    model.CustomerName = customer.Name;
    //    model.CustomerMobileNo = customer.MobileNo;

    //    return await ReloadCreateView(model);
    //}
}