
$(document).ready(function () {
    loadTotalRevenueRadialChart();
});

function loadTotalRevenueRadialChart() {
    $('.chart-spinner').show();

    $.ajax({
        url: "/Dashboard/GetTotalRevenueRadialChartData",
        type : "GET",
        dataType: "json",
        success: function (data) {
            document.querySelector('#spanTotalRevenueCount').innerHTML = data.totalCount;

            var secctionCurrentCount = document.createElement('span');
            if (data.hasRatioIncreased) {
                secctionCurrentCount.className = "text-success me-1";
                secctionCurrentCount.innerHTML = '<i class="bi bi-arrow-up-right-circle"></i> <span>' + data.countInCurrentMonth + '</span>';
            }
            else {
                secctionCurrentCount.className = "text-danger me-1";
                secctionCurrentCount.innerHTML = '<i class="bi bi-arrow-down-right-circle"></i> <span>' + data.countInCurrentMonth + '</span>';
            }
            document.querySelector('#sectionRevenueCount').append(secctionCurrentCount);
            document.querySelector('#sectionRevenueCount').append("از ماه گذشته");

            loadRadialBarChart("totalRevenuesRadialChart", data);

            $('.chart-spinner').hide();
        }
    });
}
