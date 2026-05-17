using HotelManagement.WebApp.Application.Dtos.Drivers;
using HotelManagement.WebApp.Application.Dtos.DropPickRequests;
using HotelManagement.WebApp.Application.Interfaces.Services;
using HotelManagement.WebApp.Application.Services.DropPickRequests;
using HotelManagement.WebApp.Domain.Enums;
using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using System.Text.Json;




  

    namespace HotelManagement.WebApp.Application.Services
    {
        public sealed class DropPickRequestService : IDropPickRequestService
        {
            private readonly IHttpClientFactory _clientFactory;

            public DropPickRequestService(IHttpClientFactory clientFactory)
            {
                _clientFactory = clientFactory;
            }

            private HttpClient GetClient()
            {
                return _clientFactory.CreateClient("client");
            }

        /// <summary>GET: api/droppickrequests</summary>

        public async Task<IReadOnlyList<DropPickRequestDto>> GetAllAsync()
        {
            return await GetClient()
                .GetFromJsonAsync<IReadOnlyList<DropPickRequestDto>>(
                    "api/droppickrequests")
                ?? new List<DropPickRequestDto>();
        }


        /// <summary>GET: api/droppickrequests/{id}</summary>
        public async Task<DropPickRequestDto?> GetByIdAsync(int requestId)
            {
                return await GetClient()
                    .GetFromJsonAsync<DropPickRequestDto>($"api/droppickrequests/{requestId}");
            }

            /// <summary>GET: api/droppickrequests/stay/{stayId}</summary>
            public async Task<IReadOnlyList<DropPickRequestDto>> GetByStayIdAsync(int stayId)
            {
                return await GetClient()
                    .GetFromJsonAsync<IReadOnlyList<DropPickRequestDto>>($"api/droppickrequests/stay/{stayId}")
                    ?? new List<DropPickRequestDto>();
            }

            /// <summary>GET: api/droppickrequests/driver/{driverId}</summary>
            public async Task<IReadOnlyList<DropPickRequestDto>> GetByDriverIdAsync(int driverId)
            {
                return await GetClient()
                    .GetFromJsonAsync<IReadOnlyList<DropPickRequestDto>>($"api/droppickrequests/driver/{driverId}")
                    ?? new List<DropPickRequestDto>();
            }

        /// <summary>GET: available drivers (returns only IDs from API)</summary>
        public async Task<IReadOnlyList<CabDriverBriefDto>> GetAvailableDriversAsync()
        {
            return await GetClient()
                .GetFromJsonAsync<IReadOnlyList<CabDriverBriefDto>>("api/droppickrequests/drivers/available")
                ?? new List<CabDriverBriefDto>();
        }

        /// <summary>POST: api/droppickrequests</summary>
        public async Task<DropPickRequestDto> CreateAsync(CreateDropPickRequest request)
            {
                var response = await GetClient()
                    .PostAsJsonAsync("api/droppickrequests", request);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<DropPickRequestDto>();
            }

            /// <summary>Interface-required overload</summary>
            public async Task<DropPickRequest> CreateAsync(DropPickRequest request)
            {
                var dto = new CreateDropPickRequest
                {
                    RequestedAt = request.RequestedAt,
                    Notes = request.Notes,
                    RequestType = (int) request.RequestType,
                    StayId = request.StayId,
                    DriverId = request.DriverId
                };

                var result = await CreateAsync(dto);

                return new DropPickRequest
                {
                    RequestId = result.RequestId,
                    Notes = result.Notes,
                    RequestType = result.RequestType,
                    Status = result.Status,
                    StayId = result.StayId,
                    DriverId = result.DriverId
                };
            }

            /// <summary>PUT: api/droppickrequests/{id}</summary>
            public async Task<DropPickRequestDto> UpdateAsync(int requestId, UpdateDropPickRequest request)
            {
                var response = await GetClient()
                    .PutAsJsonAsync($"api/droppickrequests/{requestId}", request);

                response.EnsureSuccessStatusCode();


               response.EnsureSuccessStatusCode();
               return await GetByIdAsync(requestId);

        }

        /// <summary>DELETE</summary>
        public async Task<bool> DeleteAsync(int requestId)
            {
                var response = await GetClient()
                    .DeleteAsync($"api/droppickrequests/{requestId}");

                return response.IsSuccessStatusCode;
            }

        /// <summary>FILTER: Ongoing requests</summary>

        public async Task<IReadOnlyList<DropPickRequestDto>> GetOngoingListAsync()
        {
            var data = await GetAllAsync();

            return data
                .Where(x => x.Status == DropPickStatus.Assigned
                         || x.Status == DropPickStatus.InProgress)
                .ToList();
        }


        /// <summary>FILTER: Completed requests</summary>
        public async Task<IReadOnlyList<DropPickRequestDto>> GetPastListAsync()
            {
                var data = await GetAllAsync();
                return data.Where(x => x.Status == DropPickStatus.Completed).ToList();
            }

            /// <summary>Alias</summary>
            public async Task<DropPickRequestDto?> GetRequestByIdAsync(int requestId)
            {
                return await GetByIdAsync(requestId);
            }

            /// <summary>Alias</summary>
            public async Task<IReadOnlyList<DropPickRequestDto>> GetRequestListAsync()
            {
                return await GetAllAsync();
            }
        }
    }


