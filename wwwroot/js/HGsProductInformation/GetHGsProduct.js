
$(function () {
    $("#table").dataTable({
        searchPanes: true,
        processing: true,
        serverSide: true,
        filter: true,
        pageLength: 25,
        lengthMenu: [10, 25, 50, 100, 200, 500, 1000],
        autoWidth: false,
        ajax: {
            "async": "true",
            "url": "/HGsProduct/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                "targets": [8],
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
            { "data": "prodid", "name": "PRODID", "autoWidth": true },
            { "data": "prodname", "name": "PRODNAME", "autoWidth": true },
            { "data": "partno", "name": "PARTNO", "autoWidth": true },
            { "data": "description", "name": "DESCRIPTION", "autoWidth": true },
            { "data": "scatid", "name": "SCATID", "autoWidth": true },
            { "data": "unit", "name": "UNIT", "autoWidth": true },
            { "data": "proD_LOC", "name": "PROD_LOC", "autoWidth": true },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    return `<a href='/Receive/Details/${row.encryptedId}' class='fa fa-info-circle fa-2x-custom text-primary'></a> <a href='/Receive/Edit/${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning'></a>`;
                }
            }
        ]
    });
});