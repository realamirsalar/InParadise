using InParadise.Application.Common.Interfaces;
using InParadise.Application.Common.Utility;
using InParadise.Application.Services.Interface;
using InParadise.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace InParadise.Application.Services.Implementation
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVillaService _villaService;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingService(IUnitOfWork unitOfWork, HttpClient httpClient, IConfiguration configuration,
            IVillaService villaService, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
            _configuration = configuration;
            _villaService = villaService;
            _userManager = userManager;
        }

        public async Task<Booking> SetupNewBookingAsync(int villaId, string userId, DateOnly checkInDate, int nights)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            var villa = _villaService.GetVillaById(villaId, includeProperties: "VillaAmenity");
            Booking booking = new()
            {
                VillaId = villaId,
                Villa = _villaService.GetVillaById(villaId, includeProperties: "VillaAmenity"),
                CheckInDate = checkInDate,
                Nights = nights,
                CheckOutDate = checkInDate.AddDays(nights),
                UserId = userId,
                Phone = user.PhoneNumber,
                Email = user.Email,
                Name = user.Name
            };
            booking.TotalCost = booking.Villa.Price * nights;
            return booking;
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

        public Booking GetBookingWithAvailableVillaNumbers(int bookingId)
        {
            Booking booking = _unitOfWork.Booking.Get(b => b.Id == bookingId, includeProperties: "User,Villa");

            if (booking.VillaNumber == 0 && booking.Status == SD.StatusApproved)
            {
                var availableVillaNumber = AssignAvailableVillaNumberByVilla(booking.VillaId);

                booking.VillaNumbers = _unitOfWork.VillaNumberRepository.GetAll(vn =>
                    vn.VillaId == booking.VillaId && availableVillaNumber.Any(a => a == vn.NumberOfVilla)).ToList();
            }

            return booking;
        }

        public Booking GetBookingByAuthority(string authority)
        {
            return _unitOfWork.Booking.Get(b => b.Authority == authority);
        }

        public IEnumerable<Booking> GetAllBooks(string userId, string? statusFilter = "",
            string? IncludeProperties = null)
        {
            IEnumerable<string> statusList = string.IsNullOrEmpty(statusFilter)
                ? new List<string>() // اگر خالی بود یک لیست خالی می‌سازیم تا ارور ندهد
                : statusFilter.ToLower().Split(",");
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

        public Booking FinalBooking(Booking booking)
        {
            var villa = _villaService.GetVillaById(booking.VillaId);

            booking.Status = SD.StatusPending;
            booking.BookingDate = DateTime.Now;
            booking.TotalCost = villa.Price * booking.Nights;
            return booking;
        }

        public void UpdateStatus(int bookingId, string bookingStatus, bool isPay, int villaNumber = 0)
        {
            var bookingFromDb = _unitOfWork.Booking.Get(b => b.Id == bookingId, tracked: true);
            if (bookingFromDb != null)
            {
                bookingFromDb.Status = bookingStatus;
                if (bookingStatus == SD.StatusCheckedIn)
                {
                    bookingFromDb.VillaNumber = villaNumber;
                    bookingFromDb.ActualCheckInDate = DateTime.Now;
                }

                if (bookingStatus == SD.StatusCompleted)
                {
                    bookingFromDb.ActualCheckOutDate = DateTime.Now;
                }

                if (isPay == true)
                {
                    bookingFromDb.IsPaymentSuccessful = isPay;
                }
            }

            _unitOfWork.Save();
        }

        public void UpdatePayment(int bookingId, string authority, string paymentGetWay, string? refId)
        {
            var bookingFromDb = _unitOfWork.Booking.Get(b => b.Id == bookingId, tracked: true);
            if (bookingFromDb != null)
            {
                if (!string.IsNullOrEmpty(authority))
                {
                    bookingFromDb.Authority = authority;
                }

                if (!string.IsNullOrEmpty(paymentGetWay))
                {
                    bookingFromDb.PaymentGateway = paymentGetWay;
                }

                if (!string.IsNullOrEmpty(refId))
                {
                    bookingFromDb.RefId = refId;
                    bookingFromDb.PaymentDate = DateTime.Now;
                }
            }

            _unitOfWork.Save();
        }

        public List<int> AssignAvailableVillaNumberByVilla(int villaId)
        {
            List<int> availableVillaNumbers = new();

            var villaNumberes = _unitOfWork.VillaNumberRepository.GetAll(vn => vn.VillaId == villaId);

            var checkedInVilla =
                _unitOfWork.Booking.GetAll(b => b.VillaId == villaId && b.Status == SD.StatusCheckedIn)
                    .Select(b => b.VillaNumber);
            foreach (var villaNumber in villaNumberes)
            {
                if (!checkedInVilla.Contains(villaNumber.NumberOfVilla))
                {
                    availableVillaNumbers.Add(villaNumber.NumberOfVilla);
                }
            }

            return availableVillaNumbers;
        }
    }
}