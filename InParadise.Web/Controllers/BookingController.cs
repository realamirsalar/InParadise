using System.Security.Claims;
using InParadise.Application.Common.Interfaces;
using InParadise.Application.Common.Utility;
using InParadise.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        public IActionResult Index()
        {
            IEnumerable<Booking> bookings;
            if (User.IsInRole(SD.AdminRole))
            {
                bookings = _unitOfWork.Booking.GetAll(includeProperties: "User,Villa");
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                bookings = _unitOfWork.Booking.GetAll(b => b.UserId == userId, includeProperties: "User,Villa");
            }

            return View(bookings);
        }

        [Authorize]
        [HttpGet]
        public IActionResult FinalizeBooking(int villaId, int nights, DateOnly checkInDate)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ApplicationUser user = _unitOfWork.User.Get(u => u.Id == userId);

            Booking booking = new()
            {
                VillaId = villaId,
                Villa = _unitOfWork.VillaRepository.Get(v => v.Id == villaId, includeProperties: "VillaAmenity"),
                CheckInDate = checkInDate,
                Nights = nights,
                CheckOutDate = checkInDate.AddDays(nights),
                UserId = userId,
                Phone = user.PhoneNumber,
                Email = user.Email,
                Name = user.Name
            };
            booking.TotalCost = booking.Villa.Price * nights;
            return View(booking);
        }

        [Authorize]
        [HttpPost]
        public IActionResult FinalizeBooking(Booking booking)
        {
            var villa = _unitOfWork.VillaRepository.Get(v => v.Id == booking.VillaId);
            booking.TotalCost = villa.Price * booking.Nights;

            booking.Status = SD.StatusPending;

            _unitOfWork.Booking.Insert(booking);
            _unitOfWork.Save();

            return RedirectToAction(nameof(BookingConfirmation), new { bookingId = booking.Id });
        }

        [Authorize]
        public IActionResult BookingConfirmation(int bookingId)
        {
            return View(bookingId);
        }

        [Authorize]
        public IActionResult BookingDetails(int bookingId)
        {
            Booking booking = _unitOfWork.Booking.Get(b => b.Id == bookingId, includeProperties: "User,Villa");

            if (booking.VillaNumber == 0 && booking.Status == SD.StatusApproved)
            {
                var availableVillaNumber = AssignAvailableVillaNumberByVilla(booking.VillaId);

                booking.VillaNumbers = _unitOfWork.VillaNumberRepository.GetAll(vn =>
                    vn.VillaId == booking.VillaId && availableVillaNumber.Any(a => a == vn.NumberOfVilla)).ToList();
            }

            return View(booking);
        }

        private List<int> AssignAvailableVillaNumberByVilla(int villaId)
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

        #region API Call

        //[HttpGet]
        //[Authorize]
        //public IActionResult GetAll()
        //{
        //    IEnumerable<Booking> bookings;
        //    if (User.IsInRole(SD.AdminRole))
        //    {
        //        bookings = _unitOfWork.Booking.GetAll(includeProperties: "User,Villa");
        //    }
        //    else
        //    {
        //        var claimsIdentity = (ClaimsIdentity)User.Identity;
        //        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

        //        bookings = _unitOfWork.Booking.GetAll(b => b.UserId == userId, includeProperties: "User,Villa");
        //    }

        //    return Json(new { data = bookings });
        //}

        #endregion
    }
}