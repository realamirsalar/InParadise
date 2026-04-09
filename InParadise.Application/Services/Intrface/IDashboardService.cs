using System;
using System.Collections.Generic;
using System.Text;
using InParadise.Application.Common.DTO;

namespace InParadise.Application.Services.Intrface
{
    public interface IDashboardService
    {
        Task<RadialBarChartDto> GetTotalBookingRadialChartData();
        Task<RadialBarChartDto> GetTotalUserRadialChartData();
        Task<RadialBarChartDto> GetTotalRevenueRadialChartData();
        Task<PieChartDto> GetBookingPieChartData();
        Task<LineChartDto> GetMemberAndBookingLineChartData();
    }
}