
$(function () {
    $('#table').dataTable({
        "searchPanes": true,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "order": [[1, 'desc'], [2, 'desc']],
        "pageLength": 25,
        "lengthMenu": [10, 25, 50, 100, 200, 500, 1000],
        "autoWidth": false,
        "ajax": {
            "async": "true",
            "url": '/FFsFabricClearance/GetTableData',
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [
            {
                "targets": [0],
                "orderable": false,
                "searchable": false
            },
            {
                "targets": [6],
                "orderable": false,
                "searchable": false
            }
        ],
        "language": {
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
        "columns": [
            { "data": "clid", "name": "CLID", "autoWidth": true },
            {
                "name": "FABCODE", "autoWidth": true,
                "render": function (data, type, row) {
                    return row.fabcodeNavigation === null ? "" : row.fabcodeNavigation.stylE_NAME;
                }
            },
            { "data": "wasH_CODE", "name": "WASH_CODE", "autoWidth": true },
            {
                "name": "Color", "autoWidth": true,
                "render": function (data, type, row) {
                    return row.fabcodeNavigation !== null && row.fabcodeNavigation.colorcodeNavigation !== null ? row.fabcodeNavigation.colorcodeNavigation.color : "";
                }
            },
            {
                "name": "SO_NO",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return row.po === null ? "" : row.po.so.sO_NO;
                }
            },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    return `<a target="_blank" href='/Reports/RWashCodewiseRollClearanceReport?clid=${row.clid}' class='fa fa-print fa-2x-custom text-success'></a> <a href='/FFsFabricClearance/DetailsFabricClearance?id=${row.encryptedId}' class='fa fa-info-circle fa-2x-custom text-info'></a> <a href='/FFsFabricClearance/EditFabricClearance?id=${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning'></a>`;
                }
            }
        ]
    });
});