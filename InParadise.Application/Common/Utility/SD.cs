using System;
using System.Collections.Generic;
using System.Text;
using InParadise.Domain.Entities;

namespace InParadise.Application.Common.Utility
{
    //Static Detail 
    public static class SD
    {
        public const string CustomerRole = "Customer";
        public const string AdminRole = "Admin";

        public const string StatusPending = "در دست انجام";
        public const string StatusApproved = "تایید شده";
        public const string StatusCheckedIn = "انجام شده";
        public const string StatusCompleted = "تکمیل";
        public const string StatusCancelled = "لغو شده";
        public const string StatusRefunded = "برگشت داده شده";

        public const string ZarinPalGateway = "زرین پال";

        public static int VillaRoomsAvailableCount(int villaId,
            List<VillaNumber> villaNumberList, DateOnly checkInDate, int nights,
            List<Booking> bookings)
        {
            List<int> bookingInDate = new();
            int finalAvailableRoomForAllNights = int.MaxValue;
            var roomsInVilla = villaNumberList.Where(x => x.VillaId == villaId).Count();

            for (int i = 0; i < nights; i++)
            {
                var villasBooked = bookings.Where(u => u.CheckInDate <= checkInDate.AddDays(i)
                                                       && u.CheckOutDate > checkInDate.AddDays(i) &&
                                                       u.VillaId == villaId);

                foreach (var booking in villasBooked)
                {
                    if (!bookingInDate.Contains(booking.Id))
                    {
                        bookingInDate.Add(booking.Id);
                    }
                }

                var totalAvailableRooms = roomsInVilla - bookingInDate.Count;
                if (totalAvailableRooms == 0)
                {
                    return 0;
                }
                else
                {
                    if (finalAvailableRoomForAllNights > totalAvailableRooms)
                    {
                        finalAvailableRoomForAllNights = totalAvailableRooms;
                    }
                }
            }

            return finalAvailableRoomForAllNights;
        }
    }
}