using InParadise.Application.Common.DTO;
using InParadise.Application.Common.Interfaces;
using InParadise.Application.Common.Utility;
using InParadise.Application.Services.Interface;
using InParadise.Domain.Entities;
using InParadise.Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;

namespace InParadise.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;

        public BookingController(IBookingService bookingService, HttpClient httpClient, IConfiguration configuration,
            IVillaService villaService, IVillaNumberService villaNumberService,
            UserManager<ApplicationUser> userManager)
        {
            _bookingService = bookingService;
            _httpClient = httpClient;
            _configuration = configuration;
            _villaService = villaService;
            _villaNumberService = villaNumberService;
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Index(string? status)
        {
            IEnumerable<Booking> bookings;
            if (User.IsInRole(SD.AdminRole))
            {
                bookings = string.IsNullOrEmpty(status)
                    ? _bookingService.GetAllBooks(IncludeProperties: "User,Villa")
                    : _bookingService.GetAllBooks(statusFilter: status, IncludeProperties: "User,Villa");
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                bookings = string.IsNullOrEmpty(status)
                    ? _bookingService.GetAllBooks(userId: userId, IncludeProperties: "User,Villa")
                    : _bookingService.GetAllBooks(userId, status, "User,Villa");
            }

            return View(bookings);
        }

        [Authorize]
        [HttpGet]
        public IActionResult FinalizeBooking(int villaId, int nights, DateOnly checkInDate)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ApplicationUser user = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();

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
            return View(booking);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> FinalizeBooking(Booking booking)
        {
            var villa = _villaService.GetVillaById(booking.VillaId);

            booking.Status = SD.StatusPending;
            booking.BookingDate = DateTime.Now;
            booking.TotalCost = villa.Price * booking.Nights;

            if (!_villaService.IsVillaByAvailableDate(villa.Id, booking.Nights, booking.CheckInDate))
            {
                ViewData["Error"] = "این ویلا قبلا رزرو شده است.";
                return RedirectToAction(nameof(FinalizeBooking), new
                {
                    VillaId = booking.VillaId,
                    nights = booking.Nights,
                    checkInDate = booking.CheckInDate
                });
            }

            _bookingService.CreateBooking(booking);

            ////////////////////////////////////////////////////////////////////////////////////


            //var domain = Request.Scheme + @"://" + Request.Host.Value + @"/";
            var requestData = new ZarinPalRequestDto()
            {
                //merchant_id = "00000000-0000-0000-0000-000000000000",
                merchant_id = _configuration.GetValue<string>("ZarinPal:MerchantId"),
                amount = (int)booking.TotalCost,
                description = "این یک تست است ومن درحال آموزش هستم.",
                //callback_url = $"{domain}booking/BookingConfirmation?bookingId={booking.Id}"
                callback_url = Url.Action(action: "BookingVerify", controller: "Booking",
                    values: null,
                    protocol: Request.Scheme)
            };
            //کار این خط: ترجمه زبان سی‌شارپ به زبان بین‌المللی اینترنت (JSON)
            var json = JsonSerializer.Serialize(requestData);
            //گذاشتن متن داخل یک پاکت نامه استاندارد برای اداره پست 
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // آدرس سندباکس برای درخواست پرداخت
            var requestUrl = _configuration.GetValue<string>("ZarinPal:PaymentRequestUrl");
            var response =
                await _httpClient.PostAsync(requestUrl, content);
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
            //var dataNode = jsonDoc.RootElement.GetProperty("data");


            // (اصلاح شده): خواندن امنِ JSON برای جلوگیری از خطای برنامه در صورت ارور دادن زرین‌پال
            if (jsonDoc.RootElement.TryGetProperty("data", out JsonElement dataNode) &&
                dataNode.ValueKind == JsonValueKind.Object)
            {
                if (dataNode.TryGetProperty("code", out JsonElement codeNode) && codeNode.GetInt32() == 100)
                {
                    // دریافت Authority (کد شناسه پرداخت)
                    string authority = dataNode.GetProperty("authority").GetString();
                    _bookingService.UpdatePayment(booking.Id, authority, SD.ZarinPalGateway, null);

                    // هدایت کاربر به درگاه پرداخت تستی
                    var paymentGetWay = _configuration.GetValue<string>(SD.ZarinPalPaymentGatewayUrl);
                    string paymentUrl = paymentGetWay + authority;
                    return Redirect(paymentUrl);
                }
            }

            return RedirectToAction(nameof(BookingFailed));

            /////////////////////////////////////////////////////////////////////////////////////


            //return RedirectToAction(nameof(BookingConfirmation), new { bookingId = booking.Id });
        }

        [Authorize]
        public async Task<IActionResult> BookingVerify(string authority, string status)
        {
            var booking = _bookingService.GetBookingByAuthority(authority);
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
                merchant_id = _configuration.GetValue<string>(SD.ZarinPalMerchantId),
                amount = (long)booking.TotalCost,
                authority = authority
            };

            var json = JsonSerializer.Serialize(verifyData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            // آدرس سندباکس برای تایید پرداخت
            var verifyUrl = _configuration.GetValue<string>(SD.ZarinPalPaymentVerificationUrl);
            var response =
                await _httpClient.PostAsync(verifyUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();


            using var jsonDoc = JsonDocument.Parse(responseString);

            if (jsonDoc.RootElement.TryGetProperty("data", out JsonElement dataNode) &&
                dataNode.ValueKind == JsonValueKind.Object)
            {
                if (dataNode.TryGetProperty("code", out JsonElement codeNode))
                {
                    int code = codeNode.GetInt32();

                    if (code == 100 || code == 101)
                    {
                        // (اصلاح شده): جلوگیری از خطای سرریز. ref_id باید string یا long باشد.
                        string refId = dataNode.GetProperty("ref_id").GetInt64().ToString();

                        // (نکته): فرض کردم پارامتر آخر UpdatePayment از نوع string است (که باید باشد)
                        _bookingService.UpdatePayment(booking.Id, booking.Authority, SD.ZarinPalGateway, refId);
                        _bookingService.UpdateStatus(booking.Id, SD.StatusApproved, true, 0);

                        return RedirectToAction(nameof(BookingConfirmation), new { bookingId = booking.Id });
                    }
                }
            }

            return RedirectToAction(nameof(BookingFailed));
        }

        [Authorize]
        public IActionResult BookingConfirmation(int bookingId)
        {
            var bookingFromDb = _bookingService.GetBookingById(bookingId, IncludeProperties: "User,Villa");
            return View(bookingFromDb);
        }

        public IActionResult BookingFailed()
        {
            return View();
        }

        [Authorize]
        public IActionResult BookingDetails(int bookingId)
        {
            Booking booking = _bookingService.GetBookingWithAvailableVillaNumbers(bookingId);

            return View(booking);
        }

        [HttpPost]
        [Authorize(Roles = SD.AdminRole)]
        public IActionResult CheckIn(Booking booking)
        {
            _bookingService.UpdateStatus(booking.Id, SD.StatusCheckedIn, true, booking.VillaNumber);
            TempData["Success"] = "ورود با موفقیت ثبت شد.";
            return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.AdminRole)]
        public IActionResult Checkout(Booking booking)
        {
            _bookingService.UpdateStatus(booking.Id, SD.StatusCompleted, true, booking.VillaNumber);
            TempData["Success"] = "روزو با موفقیت تکمیل شد.";
            return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.AdminRole)]
        public IActionResult CancelBooking(Booking booking)
        {
            _bookingService.UpdateStatus(booking.Id, SD.StatusCancelled, true, 0);
            TempData["Error"] = "روزو با موقیت کنسل شد.";
            return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
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