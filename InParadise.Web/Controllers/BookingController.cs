using InParadise.Application.Common.DTO;
using InParadise.Application.Common.Interfaces;
using InParadise.Application.Common.Utility;
using InParadise.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace InParadise.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;
        private readonly string _merchantId = "00000000-0000-0000-0000-000000000000";

        public BookingController(IUnitOfWork unitOfWork, HttpClient httpClient)
        {
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
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
        public async Task<IActionResult> FinalizeBooking(Booking booking)
        {
            var villa = _unitOfWork.VillaRepository.Get(v => v.Id == booking.VillaId);
            booking.TotalCost = villa.Price * booking.Nights;

            booking.Status = SD.StatusPending;

            _unitOfWork.Booking.Insert(booking);
            _unitOfWork.Save();


            ////////////////////////////////////////////////////////////////////////////////////


            //var domain = Request.Scheme + @"://" + Request.Host.Value + @"/";
            var requestData = new ZarinPalRequestDto()
            {
                //merchant_id = "00000000-0000-0000-0000-000000000000",
                merchant_id = _merchantId,
                amount = (int)booking.TotalCost,
                description = "این یک تست است ومن درحال آموزش هستم.",
                //callback_url = $"{domain}booking/BookingConfirmation?bookingId={booking.Id}"
                callback_url = Url.Action(action: "BookingVerify", controller: "Booking",
                    values: new { bookingId = booking.Id, amount = booking.TotalCost },
                    protocol: Request.Scheme)
            };
            //کار این خط: ترجمه زبان سی‌شارپ به زبان بین‌المللی اینترنت (JSON)
            var json = JsonSerializer.Serialize(requestData);
            //گذاشتن متن داخل یک پاکت نامه استاندارد برای اداره پست 
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // آدرس سندباکس برای درخواست پرداخت
            var response =
                await _httpClient.PostAsync("https://sandbox.zarinpal.com/pg/v4/payment/request.json", content);
            var responseString = await response.Content.ReadAsStringAsync();

            //تبدیل متن خام به یک سند منظم و قابل جستجو برای سی‌شارپ
            ///////////////////////////////////////////////////////////
            //کلمه
            //using:
            //این سند منظم، مقداری از حافظه موقت
            //(RAM)
            //سرور شما را اشغال می‌کند.
            //کلمه
            //using
            //به سی‌شارپ می‌گوید:
            //«به محض اینکه در انتهای این متد کارم با این متغیر تمام شد، بلافاصله آن را از حافظه پاک کن (زباله‌روبی کن) تا سیستم کند نشود.»
            using var jsonDoc = JsonDocument.Parse(responseString);
            // باز کردن پوشه اصلی اطلاعات (بخش data) در نامه زرین‌پال
            var dataNode = jsonDoc.RootElement.GetProperty("data");


            if (dataNode.ValueKind != JsonValueKind.Null && dataNode.GetProperty("code").GetInt32() == 100)
            {
                // دریافت Authority (کد شناسه پرداخت)
                string authority = dataNode.GetProperty("authority").GetString();
                _unitOfWork.Booking.UpdatePayment(booking.Id, authority, SD.ZarinPalGateway, null);
                _unitOfWork.Save();
                // هدایت کاربر به درگاه پرداخت تستی
                string paymentUrl = $"https://sandbox.zarinpal.com/pg/StartPay/{authority}";
                return Redirect(paymentUrl);
            }

            return RedirectToAction(nameof(BookingFailed));

            /////////////////////////////////////////////////////////////////////////////////////


            //return RedirectToAction(nameof(BookingConfirmation), new { bookingId = booking.Id });
        }

        [Authorize]
        public async Task<IActionResult> BookingVerify(int bookingId, string authority, string status, int amount)
        {
            var booking = _unitOfWork.Booking.Get(b => b.Authority == authority);
            if (booking == null)
            {
                return RedirectToAction(nameof(BookingFailed));
            }

            // زرین پال وضعیت را به صورت OK یا NOK برمی‌گرداند
            if (status != "OK")
            {
                return RedirectToAction(nameof(BookingFailed));
            }

            var verifyData = new ZarinPalVerifyDto()
            {
                merchant_id = _merchantId,
                amount = amount,
                authority = authority
            };

            var json = JsonSerializer.Serialize(verifyData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            // آدرس سندباکس برای تایید پرداخت
            var response =
                await _httpClient.PostAsync("https://sandbox.zarinpal.com/pg/v4/payment/verify.json", content);
            var responseString = await response.Content.ReadAsStringAsync();


            using var jsonDoc = JsonDocument.Parse(responseString);
            var dataNode = jsonDoc.RootElement.GetProperty("data");

            if (dataNode.ValueKind != JsonValueKind.Null)
            {
                int code = dataNode.GetProperty("code").GetInt32();

                if (code == 100 || code == 101)
                {
                    int refId = dataNode.GetProperty("ref_id").GetInt32();
                    _unitOfWork.Booking.UpdatePayment(bookingId, booking.Authority, SD.ZarinPalGateway, refId);
                    _unitOfWork.Booking.UpdateStatus(bookingId, SD.StatusApproved);
                    _unitOfWork.Save();
                    return RedirectToAction(nameof(BookingConfirmation), new { bookingId = booking.Id });
                }
            }

            return RedirectToAction(nameof(BookingFailed));
        }

        [Authorize]
        public IActionResult BookingConfirmation(int bookingId)
        {
            var bookingFromDb = _unitOfWork.Booking.Get(b => b.Id == bookingId, includeProperties: "User,Villa");
            return View(bookingFromDb);
        }

        public IActionResult BookingFailed()
        {
            return View();
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

        [HttpPost]
        [Authorize(Roles = SD.AdminRole)]
        public IActionResult CheckIn(Booking booking)
        {
            //_bookingService.UpdateStatus(booking.Id, SD.StatusCompleted, booking.VillaNumber);
            //TempData["Success"] = "Booking Completed Successfully.";
            return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.AdminRole)]
        public IActionResult Checkout(Booking booking)
        {
            //_bookingService.UpdateStatus(booking.Id, SD.StatusCompleted, booking.VillaNumber);
            //TempData["Success"] = "Booking Completed Successfully.";
            return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.AdminRole)]
        public IActionResult CancelBooking(Booking booking)
        {
            //_bookingService.UpdateStatus(booking.Id, SD.StatusCompleted, booking.VillaNumber);
            //TempData["Success"] = "Booking Completed Successfully.";
            return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
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