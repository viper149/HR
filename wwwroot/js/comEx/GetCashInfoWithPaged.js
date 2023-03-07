
$(function () {
    $("#table").dataTable({
        searchPanes: true,
        processing: true,
        serverSide: true,
        filter: true,
        order: [[0, "desc"]],
        pageLength: 25,
        lengthMenu: [10, 25, 50, 100, 200, 500, 1000],
        autoWidth: false,
        ajax: {
            "async": "true",
            "url": "/ComExCashInfo/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [{
            "targets": [9],
            "orderable": false,
            "searchable": false
        },
        {
            "targets": [0],
            "visible": false,
            "orderable": false,
            "searchable": false
        }],
        language: {
            "searchPlaceholder": "Search for your desire results",
            "search": '<i class="fa fa-2x fa-search" aria-hidden="true"></i>',
            "emptyTable": '<h3>No data available</h3>',
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
            { "data": "cashid", "name": "CASHID", "autoWidth": true },
            { "data": "cashno", "name": "CASHNO", "autoWidth": true },
            { "data": "lc.lcno", "name": "LCID", "autoWidth": true },
            { "data": "itemqtY_YDS", "name": "ITEMQTY_YDS", "autoWidth": true },
            { "data": "vcno", "name": "VCNO", "autoWidth": true },
            { "data": "rate", "name": "RATE", "autoWidth": true },
            { "data": "backtobacK_LCTYPE", "name": "BACKTOBACK_LCTYPE", "autoWidth": true },
            {
                "data": "subdate",
                "name": "SUBDATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            {
                "data": "rcvddate",
                "name": "RCVDDATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            {
                "render": function (data, type, row) {
                    const checkType = row.backtobacK_LCTYPE.toLowerCase() === "deemed" ? `<a target='_blank' href='/Reports/CommercialExport/Cash/Deemed/${row.cashno}' class='fa fa-print fa-lg text-primary'></a>` : `<a target='_blank' href='/Reports/CommercialExport/Cash/${row.cashno}' class='fa fa-print fa-lg text-primary'></a>`;
                    return `${checkType} <a href='/ComExCashInfo/DetailsCashInfo?id=${row.encryptedId}' class='fa fa-info-circle fa-lg text-info'></a> <a href='/ComExCashInfo/EditCashInfo?id=${row.encryptedId}' class='fa fa-edit fa-lg text-warning'></a> <a href='#' class='fa fa-trash fa-lg text-danger' onclick=deleteSwal(event,'${row.encryptedId}');></a>`;
                }
            },
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
                window.location = `/ComExCashInfo/DeleteCashInfo?id=${item}`;
            }
        });
}