using InParadise.Application.Common.Interfaces;
using InParadise.Application.Common.Utility;
using InParadise.Infrastructure.Repository;
using InParadise.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InParadise.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public static readonly int previousMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1;
        public readonly DateTime previousMonthStartDate = new(DateTime.Now.Year, previousMonth, 1);
        public readonly DateTime currentMonthStartDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTotalBookingRadialChartData()
        {
            var totalBooking =
                _unitOfWork.Booking.GetAll(b => b.Status != SD.StatusPending || b.Status == SD.StatusCancelled);

            var countByCurrentMonth =
                totalBooking.Count(b => b.BookingDate >= currentMonthStartDate && b.BookingDate <= DateTime.Now);

            var countBypreviousMonth =
                totalBooking.Count(b =>
                    b.BookingDate >= previousMonthStartDate && b.BookingDate <= currentMonthStartDate);

            return Json(GetRadialChartDataModel(totalBooking.Count(), countByCurrentMonth, countBypreviousMonth));
        }

        public async Task<IActionResult> GetTotalUserRadialChartData()
        {
            var totalUser =
                _unitOfWork.User.GetAll();

            var countByCurrentMonth =
                totalUser.Count(u => u.CreateAt >= currentMonthStartDate && u.CreateAt <= DateTime.Now);

            var countBypreviousMonth =
                totalUser.Count(u => u.CreateAt >= previousMonthStartDate && u.CreateAt <= currentMonthStartDate);


            return Json(GetRadialChartDataModel(totalUser.Count(), countByCurrentMonth, countBypreviousMonth));
        }

        public async Task<IActionResult> GetTotalRevenueRadialChartData()
        {
            var totalBooking =
                _unitOfWork.Booking.GetAll(b => b.Status != SD.StatusPending || b.Status == SD.StatusCancelled);

            var totalRevenue = Convert.ToInt64(totalBooking.Sum(b => b.TotalCost));

            var countByCurrentMonth =
                totalBooking.Where(b => b.BookingDate >= currentMonthStartDate && b.BookingDate <= DateTime.Now)
                    .Sum(b => b.TotalCost);

            var countBypreviousMonth =
                totalBooking.Where(b =>
                        b.BookingDate >= previousMonthStartDate && b.BookingDate <= currentMonthStartDate)
                    .Sum(b => b.TotalCost);

            return Json(GetRadialChartDataModel(totalBooking.Count(), countByCurrentMonth, countBypreviousMonth));
        }

        public async Task<IActionResult> GetBookingPieChartData()
        {
            var totalBooking =
                _unitOfWork.Booking.GetAll(b =>
                    b.BookingDate >= DateTime.Now.AddDays(-30) &&
                    (b.Status != SD.StatusPending || b.Status == SD.StatusCancelled));

            //مشتری هایی که فقط یک سفارش از ابتدا داشته ان یا همان مشتریان جدید
            var customerWithOneBooking =
                totalBooking.GroupBy(b => b.UserId).Where(x => x.Count() == 1).Select(x => x.Key).ToList();

            int bookingByNewCustomer = customerWithOneBooking.Count();
            int bookingByReturningCustomer = totalBooking.Count() - bookingByNewCustomer;

            PieChartVM pieChartVm = new()
            {
                Lables = new string[] { "رزور های  جدید", "روزو های مشتریان قدیمی" },
                Series = new decimal[] { 3, 5 }
            };

            return Json(pieChartVm);
        }

        public async Task<IActionResult> GetMemberAndBookingLineChartData()
        {
            var bookingData = _unitOfWork.Booking.GetAll(b => b.BookingDate >= DateTime.Now.AddDays(-30))
                .GroupBy(b => b.BookingDate.Date).Select(u => new
                {
                    DateTime = u.Key,
                    NewBookingCount = u.Count()
                });
            var customerData = _unitOfWork.User.GetAll(b => b.CreateAt >= DateTime.Now.AddDays(-30))
                .GroupBy(b => b.CreateAt.Date).Select(u => new
                {
                    DateTime = u.Key,
                    NewCustomerCount = u.Count()
                });

            var leftJoin =
                bookingData.GroupJoin(customerData, booking => booking.DateTime, customer => customer.DateTime,
                    (booking, customer) => new
                    {
                        booking.DateTime,
                        booking.NewBookingCount,
                        NewCustomerCount = customer.Select(x => x.NewCustomerCount).SingleOrDefault()
                    });


            var rightJoin =
                customerData.GroupJoin(bookingData, customer => customer.DateTime, booking => booking.DateTime,
                    (customer, booking) => new
                    {
                        customer.DateTime,
                        NewBookingCount = booking.Select(x => x.NewBookingCount).SingleOrDefault(),
                        customer.NewCustomerCount
                    });

            var mergeData = leftJoin.Union(rightJoin).OrderBy(x => x.DateTime).ToList();

            var newBookingDate = mergeData.Select(m => m.NewBookingCount).ToArray();
            var newCustomerDate = mergeData.Select(m => m.NewCustomerCount).ToArray();
            var categories = mergeData.Select(m => m.DateTime.ToString("MM/dd/yyyy")).ToArray();

            List<ChartData> chartDataList = new List<ChartData>()
            {
                new ChartData()
                {
                    Name = "رزرو های جدید",
                    Data = newBookingDate
                },
                new ChartData()
                {
                    Name = " کاربران جدید",
                    Data = newCustomerDate
                }
            };

            LineChartVM lineChartVM = new LineChartVM()
            {
                Categories = categories,
                Series = chartDataList
            };


            return Json(lineChartVM);
        }


        private static RadialBarChartVM GetRadialChartDataModel(long totalCount, double currentMonthCount,
            double prevMonthCount)
        {
            RadialBarChartVM radialBarChartVm = new();

            int increaseDecreaseRation = 100;

            if (prevMonthCount != 0)
            {
                //محاسبه درصد افزایش نسبت به ماه قبل
                increaseDecreaseRation =
                    Convert.ToInt32((currentMonthCount - prevMonthCount) / prevMonthCount * 100);
            }

            radialBarChartVm.TotalCount = totalCount;
            radialBarChartVm.CountInCurrentMonth = Convert.ToInt32(currentMonthCount);
            radialBarChartVm.HasRatioIncreased = currentMonthCount > prevMonthCount;
            radialBarChartVm.Series = new int[] { increaseDecreaseRation };

            return radialBarChartVm;
        }
    }
}