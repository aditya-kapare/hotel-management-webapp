using HotelManagement.WebApp.Application.Dtos.Stays;
using HotelManagement.WebApp.Domain.Models;

namespace HotelManagement.WebApp.Application.Services.Stays
{
    internal static class StayMapping
    {
        internal static StayDto ToDto(Stay s) => new()
        {
            StayId = s.StayId,
            CheckInAt = s.CheckInAt,
            CheckOutAt = s.CheckOutAt,
            DepositPaid = s.DepositPaid,
            AmountPaid = s.AmountPaid,
            PendingAmount = s.PendingAmount,
            RoomNo = s.RoomNo,
            CustomerIdentityId = s.CustomerIdentityId
        };

        internal static Stay ToEntity(CheckInRequest r, DateTime checkInAt) => new()
        {
            CheckInAt = checkInAt,
            CheckOutAt = null,
            DepositPaid = r.DepositPaid,
            AmountPaid = 0,
            PendingAmount = 0,
            RoomNo = r.RoomNo,
            CustomerIdentityId = r.CustomerIdentityId
        };

        internal static void Apply(UpdateStayRequest r, Stay s)
        {
            s.RoomNo = r.RoomNo;
            s.CheckInAt = r.CheckInAt;
            s.DepositPaid = r.DepositPaid;
            s.AmountPaid = r.AmountPaid;
            s.PendingAmount = r.PendingAmount;
        }

        internal static void ApplyCheckOut(CheckOutRequest r, Stay s, DateTime checkOutAt)
        {
            s.CheckOutAt = checkOutAt;
            s.AmountPaid = r.AmountPaid;
            s.PendingAmount = r.PendingAmount;
        }
    }
}