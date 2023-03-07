
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
            "url": "/ComImpWorkOrder/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [{
            "targets": [5],
            "orderable": false,
            "searchable": false
        }],
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
            { "data": "woid", "name": "WOID", "autoWidth": true, "visible": false },
            {
                "data": "wodate", "name": "WODATE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "contno", "name": "CONTNO", "autoWidth": true },
            { "data": "ind.indsl.indslno", "name": "IND.INDSL.INDSLNO", "autoWidth": true },
            { "data": "supp.suppname", "name": "SUPPNAME", "autoWidth": true },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "data": "valdate", "name": "VALDATE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            {
                "data": "btL_APPROVE", "name": "BTL_APPROVE", "autoWidth": true, "class": "text-center",
                "render": function (data, type, row) {
                    return data === true ? "<span class='text-success'>Approved</span>" : "<span class='text-danger'>Not Approved</span>";
                }
            },
            {
                "data": "fiN_APPROVE", "name": "FIN_APPROVE", "autoWidth": true, "class": "text-center",
                "render": function (data, type, row) {
                    return data === true ? "<span class='text-success'>Approved</span>" : "<span class='text-danger'>Not Approved</span>";
                }
            },
            {
                "render": function (data, type, row) {

                    var detailsUrl = `<a href='/ComImpWorkOrder/DetailsComImpWorkOrder?woId=${row.encryptedId}' class='fa fa-info-circle fa-2x-custom text-primary'></a>`;
                    var editUrl = `<a href='/ComImpWorkOrder/EditComImpWorkOrder?woId=${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning'></a>`;
                    var woReportUrl = `<a target='_blank' title='Work Order Report Of Contract No: ${row.contno}' href='/Reports/RWorkOrderReport?woId=${row.woid}' class='fa fa-print fa-lg text-primary'>WO</a>`;
                    var poReportUrl = `<a target='_blank' title='PO Report Of Contract No: ${row.contno}' href='/Reports/RWorkOrderPOReport?woId=${row.woid}' class='fa fa-print fa-lg text-primary'>PO</a>`;
                    var locked =
                        `<i title="This item is locked." class="fa fa-lock fa-2x-custom text-danger" aria-hidden="true"></i>`;

                    return `${poReportUrl} ${woReportUrl} ${editUrl}`;
                }
            }
        ]
    });
});