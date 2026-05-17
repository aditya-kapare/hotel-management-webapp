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

        
            private readonly IHttpClientFactory _clientFactory;

            public StayService(IHttpClientFactory clientFactory)
            {
                _clientFactory = clientFactory;
            }

            private HttpClient GetClient()
            {
                return _clientFactory.CreateClient("client");
            }

            /// <summary>
            /// GET: api/stays
            /// </summary>
            public async Task<IReadOnlyList<StayDto>> GetAllAsync()
            {
                return await GetClient()
                    .GetFromJsonAsync<IReadOnlyList<StayDto>>("api/stays")
                    ?? new List<StayDto>();
            }

            /// <summary>
            /// GET: api/stays/{stayId}
            /// </summary>
            public async Task<StayDto?> GetByIdAsync(int stayId)
            {
                return await GetClient()
                    .GetFromJsonAsync<StayDto>($"api/stays/{stayId}");
            }

            /// <summary>
            /// GET: api/stays/active
            /// </summary>
            public async Task<IReadOnlyList<StayDto>> GetActiveAsync()
            {
                return await GetClient()
                    .GetFromJsonAsync<IReadOnlyList<StayDto>>("api/stays/active")
                    ?? new List<StayDto>();
            }

            /// <summary>
            /// GET: api/stays/past
            /// </summary>
            public async Task<IReadOnlyList<StayDto>> GetPastAsync()
            {
                return await GetClient()
                    .GetFromJsonAsync<IReadOnlyList<StayDto>>("api/stays/past")
                    ?? new List<StayDto>();
            }

            /// <summary>
            /// GET: api/stays/by-date/{date}
            /// </summary>
            public async Task<IReadOnlyList<StayDto>> GetByCheckInDateAsync(DateTime date)
            {
                return await GetClient()
                    .GetFromJsonAsync<IReadOnlyList<StayDto>>($"api/stays/by-date/{date:yyyy-MM-dd}")
                    ?? new List<StayDto>();
            }

            /// <summary>
            /// POST: api/stays (Create Stay / Check-in)
            /// </summary>
            public async Task<StayDto> CheckInAsync(CheckInRequest request)
            {
                var response = await GetClient().PostAsJsonAsync("api/stays", request);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<StayDto>();
            }

            /// <summary>
            /// POST: api/stays/{stayId}/checkout
            /// </summary>
            public async Task<StayDto> CheckOutAsync(int stayId, CheckOutRequest request)
            {
                var response = await GetClient()
                    .PostAsJsonAsync($"api/stays/{stayId}/checkout", request);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<StayDto>();
            }

            /// <summary>
            /// PUT: api/stays/{stayId}
            /// </summary>
            public async Task<StayDto> UpdateAsync(int stayId, UpdateStayRequest request)
            {
                var response = await GetClient()
                    .PutAsJsonAsync($"api/stays/{stayId}", request);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<StayDto>();
            }

            /// <summary>
            /// DELETE: api/stays/{stayId}
            /// </summary>
            public async Task<bool> DeleteAsync(int stayId)
            {
                var response = await GetClient()
                    .DeleteAsync($"api/stays/{stayId}");

                return response.IsSuccessStatusCode;
            }

            /// <summary>
            /// GET: api/stays/{stayId}/billing
            /// </summary>
            public async Task<BillingSummaryDto> GetBillingSummaryAsync(int stayId)
            {
                return await GetClient()
                    .GetFromJsonAsync<BillingSummaryDto>($"api/stays/{stayId}/billing");
            }

            /// <summary>
            /// NOT DIRECT API → filter from GetAll
            /// </summary>
            public async Task<IReadOnlyList<StayDto>> GetByCustomerIdentityIdAsync(string customerIdentityId)
            {
                var data = await GetAllAsync();

                return data
                    .Where(s => s.CustomerIdentityId == customerIdentityId)
                    .ToList();
            }

            /// <summary>
            /// NOT DIRECT API → filter from GetAll
            /// </summary>
            public async Task<IReadOnlyList<StayDto>> GetByRoomNoAsync(int roomNo)
            {
                var data = await GetAllAsync();

                return data
                    .Where(s => s.RoomNo == roomNo)
                    .ToList();
            }

            /// <summary>
            /// No API → reuse CheckIn
            /// </summary>
            public async Task<StayDto> ForceCheckInAsync(CheckInRequest request)
            {
                return await CheckInAsync(request);
            }
        }
    }



