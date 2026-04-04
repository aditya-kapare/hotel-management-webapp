using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.Persistance.Interfaces.Repositories;
using HotelManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelManagementSystem.DAL
{
    public class StayDAL : IStayDAL
    {
        private readonly HotelDbContext _context;

        public StayDAL(HotelDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Stay> GetAllStays()
        {
            return _context.Stays.ToList();
        }

        public Stay GetStayById(int stayId)
            => _context.Stays.FirstOrDefault(s => s.StayId == stayId);

        public IEnumerable<Stay> GetStaysByRoomNo(int roomNo)
        {
            return _context.Stays.Where(s => s.RoomNo == roomNo).ToList();
        }

        public IEnumerable<Stay> GetStaysByCustomerIdentityId(string customerIdentityId)
        {
            return _context.Stays.Where(s => s.CustomerIdentityId == customerIdentityId).ToList();
        }

        public IEnumerable<Stay> GetStaysByCheckInDate(DateTime date)
        {
            var target = date.Date;
            return _context.Stays.Where(s => s.CheckInAt.Date == target).ToList();
        }

        public void AddStay(Stay stay)
        {
            _context.Stays.Add(stay);
            _context.SaveChanges();
        }

        public void UpdateStay(Stay stay)
        {
            _context.Stays.Update(stay);
            _context.SaveChanges();
        }

        public void DeleteStay(int stayId)
        {
            var stay = _context.Stays.FirstOrDefault(s => s.StayId == stayId);
            if (stay != null)
            {
                _context.Stays.Remove(stay);
                _context.SaveChanges();
            }
        }
    }
}