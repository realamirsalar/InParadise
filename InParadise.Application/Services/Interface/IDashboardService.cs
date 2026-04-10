using InParadise.Application.Common.DTO;

namespace InParadise.Application.Services.Interface
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