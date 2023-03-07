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
            "url": "/FFsFabricClearence2ndBeam/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [{
            "targets": [8],
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
            { "data": "triaL_NO", "name": "Trial", "autoWidth": true },
            { "data": "opT1", "name": "Emp", "autoWidth": true },
            { "data": "opT2", "name": "Type", "autoWidth": true },
            { "data": "opT7", "name": "Style", "autoWidth": true },
            { "data": "opT3", "name": "Order", "autoWidth": true },
            { "data": "opT4", "name": "Set", "autoWidth": true},
            { "data": "opT6", "name": "LabB", "autoWidth": true },
            { "data": "opT5", "name": "LabG", "autoWidth": true },
            { "data": "qualitY_COMMENTS", "name": "QUALITY_COMMENTS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    return `<a href='#' class='fa fa-trash fa-2x-custom text-danger mr-2' onclick=deleteSwal(event,'${row.encryptedId}');></a> <a href='/FFsFabricClearence2ndBeam/Edit2ndBeam?id=${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning'></a>`;
                }
            }

        ]
    });
});

