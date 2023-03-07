
$(function () {
    $('#table').dataTable({
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
            "url": "/PlProductionPlanGroupMaster/GetTablePendingData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                //"targets": [3],
                //"orderable": true,
                //"searchable": true
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
            { "data": "trnsdate", "name": "TRNSDATE", "autoWidth": true },
            { "data": "proG_.proG_NO", "name": "PROG_NO", "autoWidth": true },
            { "data": "opT3", "name": "SUBGROUPNO", "autoWidth": true },
            { "data": "opT2", "name": "GROUP_NO", "autoWidth": true },
            { "data": "opT1", "name": "Status", "autoWidth": true },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true }
        ]
    });
});