using HotelManagement.WebApp.Application.Interfaces.Services;

namespace HotelManagement.WebApp.Application.Interfaces.Facades
{
    public interface IReceptionistServiceFacade
    {
        ICustomerService Customers { get; }
        IRoomService Rooms { get; }
        IStayService Stays { get; }
        IDropPickRequestService DropPickRequests { get; }
    }
}