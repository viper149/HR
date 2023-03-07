$(function () {
    $("#table").dataTable({
        searchPanes: true,
        processing: true,
        serverSide: true,
        filter: true,
        order: [[0, "asc"]],
        pageLength: 25,
        lengthMenu: [10, 25, 50, 100, 200, 500, 1000],
        autoWidth: false,
        ajax: {
            "async": "true",
            "url": "/FFsPackingList/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                "targets": [12],
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
            {
                "data": "d_CHALLANDATE", "name": "D_CHALLANDATE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            {
                "name": "DOID", "autoWidth": true,
                "render": function (data, type, row) {
                    return row.do !== null ? row.do : "";
                }
            },
            {
                "name": "PIID", "autoWidth": true,
                "render": function (data, type, row) {
                    return row.pi !== null ? row.pi : "";
                }
            },
            {
                "name": "STYLE", "autoWidth": true,
                "render": function (data, type, row) {
                    return row.styleName !== null ? row.styleName : "";
                }
            },
            {
                "autoWidth": true,
                "render": function (data, type, row) {
                    return row.so_No !== null ? row.so_No : "";
                }
            },
            {
                "name": "VEHICLENO", "autoWidth": true,
                "render": function (data, type, row) {
                    return row.vnumber !== null ? row.vnumber : "";
                }
            },
            { "data": "deL_TYPE", "name": "deliverY_TYPE", "autoWidth": true },
            { "data": "dC_NO", "name": "DC_NO", "autoWidth": true },
            { "data": "gP_NO", "name": "GP_NO", "autoWidth": true },
            { "data": "lockno", "name": "LOCKNO", "autoWidth": true },
            {
                "name": "AUDITBY", "autoWidth": true,
                "render": function (data, type, row) {
                    return row.auditby !== null ? row.auditby : "";
                }
            },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                //< a target = '_blank' title = 'Print Delivery Challan' href = '/FFsPackingList/RDeliveryChallanNo?id=${row.dC_NO}' class= 'fa fa-print fa-2x-custom text-primary mr-2' ></a > <a target='_blank' title='Print Gate Pass' href='/FFsPackingList/RGatePassNo?id=${row.gP_NO}' class='fa fa-print fa-2x-custom text-primary mr-2'></a>

                "render": function (data, type, row) {
                    return `<a href='/FFsPackingList/DetailsDeliveryChallan?id=${row.encryptedId}' class='fa fa-info-circle fa-2x-custom text-info'></a> <a href='/FFsPackingList/EditDeliveryChallan?id=${row.encryptedId}'  class='fa fa-edit fa-2x-custom text-warning'></a> `;
                }
            }
        ]
    });
});