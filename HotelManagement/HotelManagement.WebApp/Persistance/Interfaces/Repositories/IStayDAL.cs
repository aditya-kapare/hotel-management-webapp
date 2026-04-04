using HotelManagement.WebApp.Domain.Models;
using System;
using System.Collections.Generic;

namespace HotelManagement.WebApp.Persistance.Interfaces.Repositories
{
    public interface IStayDAL
    {
        IEnumerable<Stay> GetAllStays();
        Stay GetStayById(int stayId);
        IEnumerable<Stay> GetStaysByRoomNo(int roomNo);
        IEnumerable<Stay> GetStaysByCustomerIdentityId(string customerIdentityId);
        IEnumerable<Stay> GetStaysByCheckInDate(DateTime date);
        void AddStay(Stay stay);
        void UpdateStay(Stay stay);
        void DeleteStay(int stayId);
    }
}