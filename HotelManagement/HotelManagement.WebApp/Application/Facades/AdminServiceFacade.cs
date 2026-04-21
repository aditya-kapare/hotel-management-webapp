using HotelManagement.WebApp.Application.Interfaces.Facades;
using HotelManagement.WebApp.Application.Interfaces.Services;

namespace HotelManagement.WebApp.Application.Facades
{
    /// <summary>
    /// Facade providing access to admin-related services.
    /// </summary>
    public sealed class AdminServiceFacade : IAdminServiceFacade
    {
        public IEmployeeService Employees { get; }
        public IRoomService Rooms { get; }
        public ICabDriverService Drivers { get; }

       
        public AdminServiceFacade(
            IEmployeeService employeeService,
            IRoomService roomService,
            ICabDriverService cabDriverService)
        {
            Employees = employeeService;
            Rooms = roomService;
            Drivers = cabDriverService;
        }
    }
}
