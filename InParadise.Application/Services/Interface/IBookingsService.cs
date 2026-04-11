using System;
using System.Collections.Generic;
using System.Text;
using InParadise.Domain.Entities;

namespace InParadise.Application.Services.Interface
{
    public interface IBookingsService
    {
        void CreateBooking(Booking booking);
        Booking GetBookingById(int bookingId, string? IncludeProperties = null);
        IEnumerable<Booking> GetAllBooks(string userId, string? statusFilter = "", string? IncludeProperties = null);
    }
}