var dataTable;


$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblBooking').DataTable({
        "ajax": {
            url : '/booking/getall'
        },
        "columns": [
            { data: 'id', "width" : "5%"},
            { data: 'name', "width" : "5%"},
            { data: 'phone', "width" : "5%"},
            { data: 'email', "width" : "5%"},
            { data: 'status', "width" : "5%"},
            { data: 'checkInDate', "width" : "5%"},
            { data: 'nights', "width" : "5%"},
            { data: 'totalCasts', "width" : "5%"},
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group">
                        <a href="/booking/bookingDetails?bookingId=${data}" class="btn btn-outline-warning mx-2">
                            <i class="bi bi-pencil-square"></i> Details
                        </a>
                    </div>`
                }
            }
        ]
    });
};