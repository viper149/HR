
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
            "url": "/FPrWeavingProduction/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                "targets": [9],
                "orderable": false,
                "searchable": false
            },
            {
                "targets": [0],
                "visible": false
            }
        ],
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
            { "data": "wV_PRODID", "name": "WV_PRODID", "autoWidth": true },
            {
                "data": "opT2", "name": "ProdDate", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            {
                "name": "Shift", "autoWidth": true,
                "render": function (data, type, row) {
                    if (row.opT1 === "2") {
                        return "A";
                    }
                    else if (row.opT1 === "3") {
                        return "B";
                    }
                    else if (row.opT1 === "4") {
                        return "C";
                    }
                    return "";
                }
            },
            { "data": "emp.firsT_NAME", "name": "Incharge", "autoWidth": true },
            { "data": "loom.looM_TYPE_NAME", "name": "LoomType", "autoWidth": true },
            { "data": "opT3", "name": "Order Type", "autoWidth": true },
            { "data": "po.so.sO_NO", "name": "SO", "autoWidth": true },
            { "data": "fabcodeNavigation.stylE_NAME", "name": "Style", "autoWidth": true },
            { "data": "totaL_PROD", "name": "TOTAL_PROD", "autoWidth": true },
            {
                "render": function (data, type, row) {

                    var detailsUrl = `<a href='/FGsIndent/DetailsFGsIndent?indId=${row.encryptedId
                        }' class='fa fa-info-circle fa-2x-custom text-primary'></a>`;
                    var editUrl = `<a href='/FPrWeavingProduction/EditFPrWeavingProduction?WpId=${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning'></a>`;

                    return `${detailsUrl} ${editUrl}`;
                }
            }
        ]
    });
});