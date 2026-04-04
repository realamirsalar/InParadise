
$(document).ready(function () {
    loadTotalBookingRadialChart();
});

function loadTotalBookingRadialChart() {
    $('.chart-spinner').show();

    $.ajax({
        url: "/Dashboard/GetTotalBookingRadialChartData",
        type : "GET",
        dataType: "json",
        success: function (data) {
            document.querySelector('#spanTotalBookingCount').innerHTML = data.totalCount;

            var secctionCurrentCount = document.createElement('span');
            if (data.hasRatioIncreased) {
                secctionCurrentCount.className = "text-success me-1";
                secctionCurrentCount.innerHTML = '<i class="bi bi-arrow-up-right-circle"></i> <span>' + data.countInCurrentMonth + '</span>';
            }
            else {
                secctionCurrentCount.className = "text-danger me-1";
                secctionCurrentCount.innerHTML = '<i class="bi bi-arrow-down-right-circle"></i> <span>' + data.countInCurrentMonth + '</span>';
            }
            document.querySelector('#sectionBookingCount').append(secctionCurrentCount);
            document.querySelector('#sectionBookingCount').append("از ماه گذشته");

            loadRadialBarChart("totalBookingsRadialChart", data);

            $('.chart-spinner').hide();
        }
    });
}
