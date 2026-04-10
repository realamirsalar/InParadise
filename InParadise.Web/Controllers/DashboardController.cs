using InParadise.Application.Common.DTO;
using InParadise.Application.Common.Interfaces;
using InParadise.Application.Common.Utility;
using InParadise.Application.Services.Interface;
using InParadise.Infrastructure.Repository;
using InParadise.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InParadise.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTotalBookingRadialChartData()
        {
            return Json(await _dashboardService.GetTotalBookingRadialChartData());
        }

        public async Task<IActionResult> GetTotalUserRadialChartData()
        {
            return Json(await _dashboardService.GetTotalUserRadialChartData());
        }

        public async Task<IActionResult> GetTotalRevenueRadialChartData()
        {
            return Json(await _dashboardService.GetTotalRevenueRadialChartData());
        }

        public async Task<IActionResult> GetBookingPieChartData()
        {
            return Json(await _dashboardService.GetBookingPieChartData());
        }

        public async Task<IActionResult> GetMemberAndBookingLineChartData()
        {
            return Json(await _dashboardService.GetMemberAndBookingLineChartData());
        }
    }
}