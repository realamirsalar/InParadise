using System;
using System.Collections.Generic;
using System.Text;
using InParadise.Domain.Entities;

namespace InParadise.Application.Services.Interface
{
    public interface IBookingService
    {
        void CreateBooking(Booking booking);
        Booking GetBookingById(int bookingId, string? IncludeProperties = null);
        Booking GetBookingWithAvailableVillaNumbers(int bookingId);
        Booking GetBookingByAuthority(string authority);

        IEnumerable<Booking> GetAllBooks(string? userId = "", string? statusFilter = "",
            string? IncludeProperties = null);

        public void UpdateStatus(int bookingId, string bookingStatus, bool isPay, int villaNumber);
        public void UpdatePayment(int bookingId, string authority, string paymentGetWay, string? refId);

        List<int> AssignAvailableVillaNumberByVilla(int villaId);
    }
}