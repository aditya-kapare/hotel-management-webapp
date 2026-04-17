using HotelManagement.WebApp.Application.Dtos.Billing;
using HotelManagement.WebApp.Application.Dtos.Stays;
using HotelManagement.WebApp.Application.Interfaces.Services;
using HotelManagement.WebApp.Application.Services.Stays;
using HotelManagement.WebApp.Domain.Enums;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;

namespace HotelManagement.WebApp.Application.Services
{
    public sealed class StayService : IStayService
    {
        private readonly IStayChargeCalculator _chargeCalculator;
        private readonly IStayDAL _stayDal;
        private readonly IRoomDAL _roomDal;
        private readonly ICustomerDAL _customerDal;

        public StayService(
            IStayDAL stayDal,
            IRoomDAL roomDal,
            ICustomerDAL customerDal,
            IStayChargeCalculator chargeCalculator)
        {
            _stayDal = stayDal;
            _roomDal = roomDal;
            _customerDal = customerDal;
            _chargeCalculator = chargeCalculator;
        }

        public async Task<IReadOnlyList<StayDto>> GetAllAsync()
        {
            var stays = await _stayDal.GetAllStaysAsync();
            return stays.Select(StayMapping.ToDto).ToList();
        }

        public async Task<IReadOnlyList<StayDto>> GetActiveAsync()
        {
            var stays = await _stayDal.GetAllStaysAsync();

            return stays
                .Where(s => s.CheckOutAt == null)
                .Select(StayMapping.ToDto)
                .ToList();
        }

        public async Task<IReadOnlyList<StayDto>> GetPastAsync()
        {
            var stays = await _stayDal.GetAllStaysAsync();

            return stays
                .Where(s => s.CheckOutAt != null)
                .Select(StayMapping.ToDto)
                .ToList();
        }

        public async Task<StayDto?> GetByIdAsync(int stayId)
        {
            if (stayId <= 0) return null;
            var stay = await _stayDal.GetStayByIdAsync(stayId);
            return stay is null ? null : StayMapping.ToDto(stay);
        }

        public async Task<IReadOnlyList<StayDto>> GetByRoomNoAsync(int roomNo)
        {
            if (roomNo <= 0) return [];
            var stays = await _stayDal.GetStaysByRoomNoAsync(roomNo);
            return stays.Select(StayMapping.ToDto).ToList();
        }

        public async Task<IReadOnlyList<StayDto>> GetByCustomerIdentityIdAsync(string customerIdentityId)
        {
            customerIdentityId = NormalizeId(customerIdentityId);
            if (string.IsNullOrWhiteSpace(customerIdentityId)) return [];
            var stays = await _stayDal.GetStaysByCustomerIdentityIdAsync(customerIdentityId);
            return stays.Select(StayMapping.ToDto).ToList();
        }

        public async Task<IReadOnlyList<StayDto>> GetByCheckInDateAsync(DateTime date)
        {
            var stays = await _stayDal.GetStaysByCheckInDateAsync(date.Date);
            return stays.Select(StayMapping.ToDto).ToList();
        }

        public async Task<StayDto> CheckInAsync(CheckInRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var normalized = NormalizeCheckIn(request);
            ValidateCheckIn(normalized);

            var customer = await _customerDal.GetCustomerByIdentityIdAsync(normalized.CustomerIdentityId);
            if (customer is null)
                throw new KeyNotFoundException($"Customer '{normalized.CustomerIdentityId}' was not found.");

            var room = await _roomDal.GetRoomByRoomNoAsync(normalized.RoomNo);
            if (room is null)
                throw new KeyNotFoundException($"Room '{normalized.RoomNo}' was not found.");

            if (room.AvailabilityStatus != AvailabilityStatus.Available)
                throw new InvalidOperationException($"Room '{normalized.RoomNo}' is not available.");

            if (room.CleanStatus != CleanStatus.Clean)
                throw new InvalidOperationException($"Room '{normalized.RoomNo}' is not clean.");

            var checkInAt = normalized.CheckInAt ?? DateTime.Now;

            var stay = StayMapping.ToEntity(normalized, checkInAt);
            await _stayDal.AddStayAsync(stay);

            room.AvailabilityStatus = AvailabilityStatus.Occupied;
            await _roomDal.UpdateRoomAsync(room);

            return StayMapping.ToDto(stay);
        }

        public async Task<StayDto> UpdateAsync(int stayId, UpdateStayRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (stayId <= 0) throw new ArgumentException("StayId must be positive.", nameof(stayId));

            var normalized = NormalizeUpdate(request);
            ValidateUpdate(normalized);

            // ✅ Read is required here to validate state & room move
            var stay = await _stayDal.GetStayByIdAsync(stayId);
            if (stay is null)
                throw new KeyNotFoundException($"Stay '{stayId}' was not found.");

            if (stay.CheckOutAt is not null)
                throw new InvalidOperationException($"Stay '{stayId}' is already checked out.");

            if (stay.RoomNo != normalized.RoomNo)
            {
                var oldRoom = await _roomDal.GetRoomByRoomNoAsync(stay.RoomNo);
                if (oldRoom is not null)
                {
                    oldRoom.AvailabilityStatus = AvailabilityStatus.Available;
                    oldRoom.CleanStatus = CleanStatus.Dirty;
                    await _roomDal.UpdateRoomAsync(oldRoom);
                }

                var newRoom = await _roomDal.GetRoomByRoomNoAsync(normalized.RoomNo);
                if (newRoom is null)
                    throw new KeyNotFoundException($"Room '{normalized.RoomNo}' was not found.");

                if (newRoom.AvailabilityStatus != AvailabilityStatus.Available)
                    throw new InvalidOperationException($"Room '{normalized.RoomNo}' is not available.");

                if (newRoom.CleanStatus != CleanStatus.Clean)
                    throw new InvalidOperationException($"Room '{normalized.RoomNo}' is not clean.");

                newRoom.AvailabilityStatus = AvailabilityStatus.Occupied;
                await _roomDal.UpdateRoomAsync(newRoom);
            }

            StayMapping.Apply(normalized, stay);

            var updated = await _stayDal.UpdateStayAsync(stay);
            if (!updated)
                throw new KeyNotFoundException($"Stay '{stayId}' was not found.");

            return StayMapping.ToDto(stay);
        }

        public async Task<StayDto> CheckOutAsync(int stayId, CheckOutRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (stayId <= 0) throw new ArgumentException("StayId must be positive.", nameof(stayId));

            var normalized = NormalizeCheckOut(request);
            ValidateCheckOut(normalized);

            var stay = await _stayDal.GetStayByIdAsync(stayId);
            if (stay is null)
                throw new KeyNotFoundException($"Stay '{stayId}' was not found.");

            if (stay.CheckOutAt is not null)
                throw new InvalidOperationException($"Stay '{stayId}' is already checked out.");

            var checkOutAt = normalized.CheckOutAt ?? DateTime.Now;

            var room = await _roomDal.GetRoomByRoomNoAsync(stay.RoomNo);
            if (room is null)
                throw new KeyNotFoundException($"Room '{stay.RoomNo}' was not found.");

            var bill = _chargeCalculator.CalculateBill(
                roomPricePerNight: room.Price,
                checkInAt: stay.CheckInAt,
                checkOutAt: checkOutAt,
                depositPaid: stay.DepositPaid,
                additionalAmountPaid: normalized.AmountPaid
            );

            if (bill.TotalPaid > bill.TotalCharge)
                throw new InvalidOperationException($"Amount paid ({bill.TotalPaid}) exceeds total charge ({bill.TotalCharge}).");

            stay.CheckOutAt = checkOutAt;

            // ✅ AmountPaid = additional paid beyond deposit
            stay.AmountPaid = normalized.AmountPaid;

            stay.PendingAmount = bill.Pending;

            var updated = await _stayDal.UpdateStayAsync(stay);
            if (!updated)
                throw new KeyNotFoundException($"Stay '{stayId}' was not found.");

            room.AvailabilityStatus = AvailabilityStatus.Available;
            room.CleanStatus = CleanStatus.Dirty;
            await _roomDal.UpdateRoomAsync(room);

            return StayMapping.ToDto(stay);
        }

        public async Task<bool> DeleteAsync(int stayId)
        {
            if (stayId <= 0) return false;

            // ✅ No read-first: single DB op delete
            return await _stayDal.DeleteStayAsync(stayId);
        }

        // Normalize + validate helpers
        private static string NormalizeId(string id) => (id ?? string.Empty).Trim();

        private static CheckInRequest NormalizeCheckIn(CheckInRequest r) => new()
        {
            CustomerIdentityId = NormalizeId(r.CustomerIdentityId),
            RoomNo = r.RoomNo,
            CheckInAt = r.CheckInAt,
            DepositPaid = r.DepositPaid
        };

        private static UpdateStayRequest NormalizeUpdate(UpdateStayRequest r) => new()
        {
            RoomNo = r.RoomNo,
            CheckInAt = r.CheckInAt,
            DepositPaid = r.DepositPaid,
            AmountPaid = r.AmountPaid,
            PendingAmount = r.PendingAmount
        };

        private static CheckOutRequest NormalizeCheckOut(CheckOutRequest r) => new()
        {
            CheckOutAt = r.CheckOutAt,
            AmountPaid = r.AmountPaid,
            PendingAmount = r.PendingAmount
        };

        private static void ValidateCheckIn(CheckInRequest r)
        {
            if (string.IsNullOrWhiteSpace(r.CustomerIdentityId))
                throw new ArgumentException("CustomerIdentityId is required.", nameof(r.CustomerIdentityId));
            if (r.RoomNo <= 0)
                throw new ArgumentException("RoomNo must be positive.", nameof(r.RoomNo));
            if (r.DepositPaid < 0)
                throw new ArgumentException("DepositPaid cannot be negative.", nameof(r.DepositPaid));
        }

        private static void ValidateUpdate(UpdateStayRequest r)
        {
            if (r.RoomNo <= 0)
                throw new ArgumentException("RoomNo must be positive.", nameof(r.RoomNo));
            if (r.DepositPaid < 0)
                throw new ArgumentException("DepositPaid cannot be negative.", nameof(r.DepositPaid));
            if (r.AmountPaid < 0)
                throw new ArgumentException("AmountPaid cannot be negative.", nameof(r.AmountPaid));
            if (r.PendingAmount < 0)
                throw new ArgumentException("PendingAmount cannot be negative.", nameof(r.PendingAmount));
        }

        private static void ValidateCheckOut(CheckOutRequest r)
        {
            if (r.AmountPaid < 0)
                throw new ArgumentException("AmountPaid cannot be negative.", nameof(r.AmountPaid));
            if (r.PendingAmount < 0)
                throw new ArgumentException("PendingAmount cannot be negative.", nameof(r.PendingAmount));
        }

        public async Task<BillingSummaryDto> GetBillingSummaryAsync(int stayId)
        {
            if (stayId <= 0)
                throw new ArgumentException("StayId must be positive.", nameof(stayId));

            var stay = await _stayDal.GetStayByIdAsync(stayId)
                ?? throw new KeyNotFoundException($"Stay '{stayId}' was not found.");

            var room = await _roomDal.GetRoomByRoomNoAsync(stay.RoomNo)
                ?? throw new KeyNotFoundException($"Room '{stay.RoomNo}' was not found.");

            var effectiveCheckOut = stay.CheckOutAt ?? DateTime.Now;

            var bill = _chargeCalculator.CalculateBill(
                roomPricePerNight: room.Price,
                checkInAt: stay.CheckInAt,
                checkOutAt: effectiveCheckOut,
                depositPaid: stay.DepositPaid,
                additionalAmountPaid: stay.AmountPaid
            );

            var nights = _chargeCalculator.CalculateNights(
                stay.CheckInAt,
                effectiveCheckOut
            );

            return new BillingSummaryDto
            {
                Nights = nights,
                RatePerNight = room.Price,
                TotalCharge = bill.TotalCharge,
                DepositPaid = stay.DepositPaid,
                AdditionalPaid = stay.AmountPaid,
                TotalPaid = bill.TotalPaid,
                PendingAmount = bill.Pending
            };
        }
    }
}