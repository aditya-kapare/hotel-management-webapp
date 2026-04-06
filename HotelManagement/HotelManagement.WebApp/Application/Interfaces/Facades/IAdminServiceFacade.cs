using HotelManagement.WebApp.Application.Interfaces.Services;

namespace HotelManagement.WebApp.Application.Interfaces.Facades
{
    public interface IAdminServiceFacade
    {
        IEmployeeService Employees { get; }
        IRoomService Rooms { get; }
        ICabDriverService Drivers { get; }
    }
}