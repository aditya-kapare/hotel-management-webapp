using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.DAL
{
    public class StayDAL : IStayDAL
    {
        private readonly HotelDbContext _context;

        public StayDAL(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Stay>> GetAllStaysAsync()
        {
            return await _context.Stays.ToListAsync();
        }

        public async Task<Stay?> GetStayByIdAsync(int stayId)
        {
            return await _context.Stays
                .FirstOrDefaultAsync(s => s.StayId == stayId);
        }

        public async Task<IEnumerable<Stay>> GetStaysByRoomNoAsync(int roomNo)
        {
            return await _context.Stays
                .Where(s => s.RoomNo == roomNo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Stay>> GetStaysByCustomerIdentityIdAsync(string customerIdentityId)
        {
            return await _context.Stays
                .Where(s => s.CustomerIdentityId == customerIdentityId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Stay>> GetStaysByCheckInDateAsync(DateTime date)
        {
            var target = date.Date;
            return await _context.Stays
                .Where(s => s.CheckInAt.Date == target)
                .ToListAsync();
        }

        public async Task AddStayAsync(Stay stay)
        {
            await _context.Stays.AddAsync(stay);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStayAsync(Stay stay)
        {
            _context.Stays.Update(stay);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStayAsync(int stayId)
        {
            var stay = await _context.Stays
                .FirstOrDefaultAsync(s => s.StayId == stayId);

            if (stay != null)
            {
                _context.Stays.Remove(stay);
                await _context.SaveChangesAsync();
            }
        }
    }
}