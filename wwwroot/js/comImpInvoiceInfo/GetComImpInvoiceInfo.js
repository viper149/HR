$(function () {

    $("#table").dataTable({
        searchPane: true,
        processing: true,
        serverSide: true,
        filter: true,
        pageLength: 25,
        lengthMenu: [10, 25, 50, 100, 200, 500, 1000],
        order: [[0, "desc"]],
        autoWidth: false,
        ajax: {
            "async": "true",
            "url": "/ComImpInvoiceInfo/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            { width: 150, targets: 1 },
            { width: 50, targets: 2 },
            { width: 100, targets: 3 },
            { width: 50, targets: 4 },
            { width: 50, targets: 5 },
            { width: 175, targets: 6 },
            { width: 175, targets: 7 },
            { width: 50, targets: 8 },
            { width: 100, targets: 9 },
            {
                "targets": [9],
                "orderable": false,
                "searchable": false
            },
            {
                "targets": [0],
                "orderable": false,
                "searchable": false,
                "visible": false
            }
        ],
        language: {
            "searchPlaceholder": "Search for your desire results",
            "search": "<i class=\"fa fa-2x fa-search\" aria-hidden=\"true\"></i>",
            "emptyTable": "<h3>No data available</h3>",
            "info": "Showing _START_ to _END_ of _TOTAL_ entries",
            "infoFiltered": "(filtered from _MAX_ total entries)",
            "thousands": ",",
            "processing": "<i class='fa fa-4x fa-spinner text-info' aria-hidden='true'></i>",
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
            { "data": "invid", "name": "INVID", "autoWidth": true },
            { "data": "invno", "name": "INVNO", "autoWidth": true },
            {
                "data": "invdate",
                "name": "INVDATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return data ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "lport", "name": "LPORT", "autoWidth": true },
            {
                "data": "dochandson",
                "name": "DOCHANDSON",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return data ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            {
                "data": "etadate",
                "name": "ETADATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return data ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            {
                "data": "invpath",
                "name": "INVPATH",
                "autoWidth": true,
                "render": function (data, type, row) {
                    if (data !== null) {
                        var subStrings = [data.slice(0, data.lastIndexOf("_")), data.slice(data.lastIndexOf("_") + 1)];
                        return `<a target='_blank' class="fa fa-file btn-link" href='/files/imp_invoice_files/?fileName=${data}'> ${subStrings[1]}</a>`;
                    } else {
                        return "N/A";
                    }
                }
            },
            {
                "data": "blpath",
                "name": "BLPATH",
                "autoWidth": true,
                "render": function (data, type, row) {
                    if (data !== null) {
                        var subStrings = [data.slice(0, data.lastIndexOf("_")), data.slice(data.lastIndexOf("_") + 1)];
                        return `<a target='_blank' class="fa fa-file btn-link" href='/files/imp_invoice_files/?fileName=${data}'> ${subStrings[1]}</a>`;
                    } else {
                        return "N/A";
                    }
                }
            },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    var editText = !row.isLocked ? "" : `<a href='/ComImpInvoiceInfo/EditComImpInvoiceInfo?invId=${row.encryptedId}' class='fa fa-edit fa-lg text-warning'></a> <a href='#' class='fa fa-trash fa-lg text-danger' onclick=deleteSwal(event,'${row.encryptedId}');></a>`;

                    return `<a href='/CommercialImportInvoice/Details/${row.encryptedId}' class='fa fa-info-circle fa-lg text-info'></a> ${editText}`;
                }
            }]
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
                window.location = `/ComImpInvoiceInfo/DeleteComImpInvoiceInfo?invId=${item}`;
            }
        });
}