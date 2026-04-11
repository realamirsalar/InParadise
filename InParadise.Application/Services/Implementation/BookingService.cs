using InParadise.Application.Common.Interfaces;
using InParadise.Application.Services.Interface;
using InParadise.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace InParadise.Application.Services.Implementation
{
    public class BookingService : IBookingsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public BookingService(IUnitOfWork unitOfWork, HttpClient httpClient, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public void CreateBooking(Booking booking)
        {
            _unitOfWork.Booking.Insert(booking);
            _unitOfWork.Save();
        }

        public Booking GetBookingById(int bookingId, string? IncludeProperties = null)
        {
            return _unitOfWork.Booking.Get(b => b.Id == bookingId, includeProperties: IncludeProperties);
        }

        public IEnumerable<Booking> GetAllBooks(string userId, string? statusFilter = "",
            string? IncludeProperties = null)
        {
            IEnumerable<string> statusList = statusFilter.ToLower().Split(",");
            if (!string.IsNullOrEmpty(statusFilter) && !string.IsNullOrEmpty(userId))
            {
                return _unitOfWork.Booking.GetAll(u => statusList.Contains(u.Status.ToLower()) && u.UserId == userId,
                    includeProperties: IncludeProperties);
            }
            else
            {
                if (!string.IsNullOrEmpty(statusFilter))
                {
                    return _unitOfWork.Booking.GetAll(u => statusList.Contains(u.Status.ToLower()),
                        includeProperties: IncludeProperties);
                }

                if (!string.IsNullOrEmpty(userId))
                {
                    return _unitOfWork.Booking.GetAll(u => u.UserId == userId, includeProperties: IncludeProperties);
                }
            }

            return _unitOfWork.Booking.GetAll(includeProperties: IncludeProperties);
        }
    }
}