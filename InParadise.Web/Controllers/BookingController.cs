using InParadise.Application.Common.Interfaces;
using InParadise.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InParadise.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult FinalizeBooking(int villaId, int nights, DateOnly checkInDate)
        {
            Booking booking = new()
            {
                VillaId = villaId,
                Villa = _unitOfWork.VillaRepository.Get(v => v.Id == villaId, includeProperties: "VillaAmenity"),
                CheckInDate = checkInDate,
                Nights = nights,
                CheckOutDate = checkInDate.AddDays(nights)
            };
            booking.TotalCost = booking.Villa.Price * nights;
            return View(booking);
        }
    }
}