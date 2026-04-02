using InParadise.Application.Common.Interfaces;
using InParadise.Application.Common.Utility;
using InParadise.Domain.Entities;
using InParadise.Infrastructure.Data;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace InParadise.Infrastructure.Repository
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private readonly ApplicationDbContext _db;

        public BookingRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }

        public void Update(Booking entity)
        {
            this._db.Bookings.Update(entity);
        }

        public void UpdateStatus(int bookingId, string bookingStatus, bool isPay)
        {
            var bookingFromDb = _db.Bookings.SingleOrDefault(b => b.Id == bookingId);
            if (bookingFromDb != null)
            {
                bookingFromDb.Status = bookingStatus;
                if (bookingStatus == SD.StatusCheckedIn)
                {
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
        }

        public void UpdatePayment(int bookingId, string authority, string paymentGetWay, string? refId)
        {
            var bookingFromDb = _db.Bookings.SingleOrDefault(b => b.Id == bookingId);
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
        }
    }
}