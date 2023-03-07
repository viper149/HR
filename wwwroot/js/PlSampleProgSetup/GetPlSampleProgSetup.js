
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
            "url": "/PlSampleProgSetup/GetTableData?path=" + window.location.pathname,
            "type": "POST",
            "datatype": "json",
            //success: function (data) {
            //    debugger;
            //    console.log(data);
            //}
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
            { "data": "opT1", "name": "RndProductionOrder.OPT1", "autoWidth": true },
            { "data": "rndProductionOrder.ordeR_QTY_YDS", "name": "QTY", "autoWidth": true },
            { "data": "rndProductionOrder.ordeR_QTY_MTR", "name": "WARP_QTY", "autoWidth": true },
            { "data": "rndProductionOrder.opT1", "name": "STYLE_NAME", "autoWidth": true },
            {
                //"data": "rndProductionOrder.so.pimaster.buyer.buyeR_NAME",
                "name": "BUYERID", "autoWidth": true,
                "render": function (data, type, row) {
                    return row.rndProductionOrder.opT5;
                }
            },
            {
                //"data": "rndProductionOrder.so.pimaster.brand.brandname",
                "name": "BRANDID", "autoWidth": true,
                "render": function (data, type, row) {
                    return row.rndProductionOrder.opT6;
                }
            },
            { "data": "opT2", "name": "PROCESS_TYPE", "autoWidth": true },
            { "data": "rndProductionOrder.opT2", "name": "LOOMID", "autoWidth": true },
            { "data": "rndProductionOrder.opT3", "name": "COLORCODE", "autoWidth": true },
            {
                "data": "rndProductionOrder.opT4",
                "name": "PROGNO", "autoWidth": true
            },
            {
                "render": function (data, type, row) {
                    return `<a href='/PlSampleProgSetup/DetailsPlSampleProgSetup?id=${row.encryptedId}' class='fa fa-info-circle fa-2x-custom text-info mr-2'></a> <a href='/PlSampleProgSetup/EditPlSampleProgSetup?id=${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning mr-2'></a>`;
                    /*<a href='/PlSampleProgSetup/DeletePlSampleProgSetup?id=${row.encryptedId}' class='fa fa-trash fa-2x-custom text-danger mr-2'></a>*/
                }
            },
        ]
    });
});