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
        public static readonly int privousMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1;
        public readonly DateTime privousMonthStartDate = new(DateTime.Now.Year, privousMonth, 1);
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

            var countByPrivousMonth =
                totalBooking.Count(b =>
                    b.BookingDate >= privousMonthStartDate && b.BookingDate <= currentMonthStartDate);

            RadialBarChartVM radialBarChartVm = new();

            int increaseDecreaseRation = 100;

            if (countByPrivousMonth != 0)
            {
                //محاسبه درصد افزایش نسبت به ماه قبل
                increaseDecreaseRation =
                    Convert.ToInt32((countByCurrentMonth - countByPrivousMonth) / countByPrivousMonth * 100);
            }

            radialBarChartVm.TotalCount = totalBooking.Count();
            radialBarChartVm.CountInCurrentMonth = countByCurrentMonth;
            radialBarChartVm.HasRatioIncreased = countByCurrentMonth > countByPrivousMonth;
            radialBarChartVm.Series = new int[] { increaseDecreaseRation };

            return Json(radialBarChartVm);
        }
    }
}