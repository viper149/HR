
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
            "url": "/CosPostCostingMaster/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                "targets": [9],
                "orderable": false,
                "searchable": false
            }
            //{
            //    "targets": [0],
            //    "visible": false
            //}
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
            {
                "data": "trnsdate", "name": "TRNSDATE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "pcostid", "name": "PCOSTID", "autoWidth": true },
            { "data": "rndProductionOrder.so.sO_NO", "name": "SO_NO", "autoWidth": true },
            { "data": "productioN_QTY", "name": "Elite", "autoWidth": true },
            { "data": "rndProductionOrder.so.style.fabcodeNavigation.stylE_NAME", "name": "Style", "autoWidth": true },
            { "data": "rndProductionOrder.so.pimaster.pino", "name": "pi", "autoWidth": true },
            { "data": "rndProductionOrder.so.pimaster.buyer.buyeR_NAME", "name": "buyer", "autoWidth": true },
            { "data": "rndProductionOrder.so.qty", "name": "qty", "autoWidth": true },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    var reportUrl = `<a target='_blank' title='Print Post Cost Sheet' href='/CosPostCostingMaster/PostCostingReport?pcostid=${row.pcostid}' class='fa fa-print fa-lg text-primary'></a>`;
                    var detailsUrl = `<a href='/CosPostCostingMaster/DetailsPostCosting?postid=${row.encryptedId
                        }' class='fa fa-info-circle fa-2x-custom text-primary'></a>`;
                    var editUrl = `<a href='/CosPostCostingMaster/EditPostCosting?postid=${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning'></a>`;
                    var deleteUrl = `<a href='/CosPostCostingMaster/DeletePostCosting?postid=${row.encryptedId}' class='fa fa-trash fa-2x-custom text-danger'></a>`;

                    return `${reportUrl},${deleteUrl} ${detailsUrl} ${editUrl}`;
                }
            }
        ]
    });
});