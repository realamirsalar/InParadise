
$(document).ready(function () {
    loadTotalUserRadialChart();
});

function loadTotalUserRadialChart() {
    $('.chart-spinner').show();

    $.ajax({
        url: "/Dashboard/GetTotalUserRadialChartData",
        type : "GET",
        dataType: "json",
        success: function (data) {
            document.querySelector('#spanTotalUserCount').innerHTML = data.totalCount;

            var secctionCurrentCount = document.createElement('span');
            if (data.hasRatioIncreased) {
                secctionCurrentCount.className = "text-success me-1";
                secctionCurrentCount.innerHTML = '<i class="bi bi-arrow-up-right-circle"></i> <span>' + data.countInCurrentMonth + '</span>';
            }
            else {
                secctionCurrentCount.className = "text-danger me-1";
                secctionCurrentCount.innerHTML = '<i class="bi bi-arrow-down-right-circle"></i> <span>' + data.countInCurrentMonth + '</span>';
            }
            document.querySelector('#sectionUserCount').append(secctionCurrentCount);
            document.querySelector('#sectionUserCount').append("از ماه گذشته");

            loadRadialBarChart("totalUsersRadialChart", data);

            $('.chart-spinner').hide();
        }
    });
}
