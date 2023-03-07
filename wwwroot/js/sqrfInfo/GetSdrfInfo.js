
$(function () {
    $("#table").dataTable({
        searchPanes: true,
        processing: true,
        serverSide: true,
        filter: true,
        order: [0, "desc"],
        pageLength: 25,
        lengthMenu: [10, 25, 50, 100, 200, 500, 1000],
        autoWidth: false,
        ajax: {
            "async": "true",
            "url": "/MktSdrfInfo/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                "targets": [10],
                "orderable": false,
                "searchable": false
            }
        ],
        language: {
            "searchPlaceholder": "Search for your desire results",
            "search": '<i class="fa fa-2x fa-search" aria-hidden="true"></i>',
            "emptyTable": "<h3>No data available</h3>",
            "info": "Showing _START_ to _END_ of _TOTAL_ entries",
            "infoFiltered": "(filtered from _MAX_ total entries)",
            "thousands": ",",
            "processing": "<i class='fa fa-4x fa-spinner' aria-hidden='true'></i>",
            "zeroRecords": "<h3>No matching records found</h3>",
            "paginate": {
                "first": "<i class='fa fa-angle-double-left' aria-hidden='true'></i>",
                "last": "<i class='fa fa-angle-double-right' aria-hidden='true'></i>",
                "next": "<i class='fa fa-angle-right' aria-hidden='true'></i>",
                "previous": "<i class='fa fa-angle-left' aria-hidden='true'></i>"
            },
            "aria": {
                "sortAscending": ": activate to sort column ascending",
                "sortDescending": ": activate to sort column descending"
            }
        },
        columns: [
            { "data": "sdrF_NO", "name": "SDRF_NO", "autoWidth": true },
            {
                "data": "transdate",
                "name": "TRANSDATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "rnD_ANALYSIS_SHEET.mkT_QUERY_NO", "name": "RND_ANALYSIS_SHEET.MKT_QUERY_NO", "autoWidth": true },
            { "data": "construction", "name": "CONSTRUCTION", "autoWidth": true },
            { "data": "teaM_M.teaM_NAME", "name": "TEAM_M.TEAM_NAME", "autoWidth": true },
            {
                "data": "teaM_R.teaM_NAME", "name": "TEAM_R.TEAM_NAME", "autoWidth": true,
                "render": function (data, type, row) {
                    return data ? data : "N/A";
                }
            },
            {
                "data": "mkT_DGM_APPROVE", "name": "MKT_DGM_APPROVE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data ? "<span class='text-success'>Approved</span>" : "<span class='text-danger'>Not Approved</span>";
                }
            },
            {
                "data": "rnD_APPROVE", "name": "RND_APPROVE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data ? "<span class='text-success'>Approved</span>" : "<span class='text-danger'>Not Approved</span>";
                }
            },
            {
                "data": "plN_APPROVE", "name": "PLN_APPROVE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data ? "<span class='text-success'>Approved</span>" : "<span class='text-danger'>Not Approved</span>";
                }
            },
            {
                "data": "planT_HEAD_APPROVE", "name": "PLANT_HEAD_APPROVE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data ? "<span class='text-success'>Approved</span>" : "<span class='text-danger'>Not Approved</span>";
                }
            },
            {
                "render": function (data, type, row) {
                    return `<a href='#' class='fa fa-trash fa-2x-custom text-danger mr-2' onclick=deleteSwal(event,'${row.encryptedId}');></a> <a href='/MktSdrfInfo/EditSdrf?id=${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning'></a>`;
                }
            }
        ]
    });
});

function deleteSwal(e, item) {
    e.preventDefault();
    swal({
        title: "Please Confirm",
        text: "Are you sure to delete?",
        type: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, sir",
        cancelButtonText: "Not at all"
    },
        function (isConfirm) {
            if (isConfirm) {
                window.location = `/MktSdrfInfo/DeleteSdrf?id=${item}`;
            }
        });
}