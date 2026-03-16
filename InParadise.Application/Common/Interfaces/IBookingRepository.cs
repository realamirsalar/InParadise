using System;
using System.Collections.Generic;
using System.Text;
using InParadise.Domain.Entities;

namespace InParadise.Application.Common.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        public void Update(Booking entity);
        public void UpdateStatus(int bookingId, string bookingStatus);
        public void UpdatePaymentID(int bookingId, string sessionId, string paymentIntentId);
    }
}