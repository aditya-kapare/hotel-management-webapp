using HotelManagement.WebApp.Application.Interfaces.Facades;
using HotelManagement.WebApp.Application.Interfaces.Services;

namespace HotelManagement.WebApp.Application.Facades
{
    public sealed class ReceptionistServiceFacade : IReceptionistServiceFacade
    {
        public ICustomerService Customers { get; }
        public IRoomService Rooms { get; }
        public IStayService Stays { get; }
        public IDropPickRequestService DropPickRequests { get; }

        public ReceptionistServiceFacade(
            ICustomerService customerService,
            IRoomService roomService,
            IStayService stayService,
            IDropPickRequestService dropPickService)
        {
            Customers = customerService;
            Rooms = roomService;
            Stays = stayService;
            DropPickRequests = dropPickService;
        }
    }
}