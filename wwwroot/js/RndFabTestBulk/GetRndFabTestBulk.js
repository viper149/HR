$(function () {

    $("#table").dataTable({

        searchPane: true,
        processing: true,
        serverSide: true,
        filter: true,
        order: [0, "desc"],
        pageLength: 25,
        lengthMenu: [10, 25, 50, 100, 200, 500, 1000],
        autoWidth: false,
        ajax: {
            "async": "true",
            "url": "/RndFabTestBulk/GetTableData",
            "type": "POST",
            "datatype": "json",
        },
        columnDefs: [{
            "targets": [7],
            "orderable": false,
            "searchable": false
        }],
        language: {
            "searchPlaceholder": "Search for your desire results",
            "search": "<i class=\"fa fa-2x fa-search\" aria-hidden=\"true\"></i>",
            "emptyTable": "<h3>No data available</h3>",
            "info": "Showing _START_ to _END_ of _TOTAL_ entries",
            "infoFiltered": "(filtered from _MAX_ total entries)",
            "thousands": ",",
            "processing": "<i class=\"fa fa-spinner fa-spin fa-3x fa-fw\"></i>",
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
            { "data": "ltbid", "name": "LTBID", "autoWidth": true, "visible": false },
            { "data": "laB_NO", "name": "LAB_NO", "autoWidth": true },
            {
                "data": "ltbdate",
                "name": "LTBDATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return moment(data).format(row.dateFormat);
                }
            },
            { "data": "prog.proG_.proG_NO", "name": "PROG_NO", "autoWidth": true },
            { "data": "prog.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.stylE_NAME", "name": "STYLE_NAME", "autoWidth": true },
            { "data": "prog.proG_.blK_PROG_.rndProductionOrder.so.pimaster.buyer.buyeR_NAME", "name":"BUYER_NAME","autoWidth": true },
            { "data": "finproc.trollnoNavigation.name", "name": "TROLLEY_NO", "autoWidth": true },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    var editUrl = `<a href='/RndFabTestBulk/EditRndFabTestBulk?ltbId=${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning'></a>`;

                    return `${editUrl}`;
                }
            }
        ]
    });
});