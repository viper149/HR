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
            "url": "/ClearanceMasterSampleRoll/GetTableData",
            "type": "POST",
            "datatype": "json",

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
            { "data": "msrid", "name": "MSRID", "autoWidth": true, "visible": false},
            {
                "data": "msrdate",
                "name": "MSRDATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            {
                "data": "maildate",
                "name": "MAILDATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "set.proG_.proG_NO", "name": "PROGNO", "autoWidth": true },
            { "data": "set.proG_.blK_PROG_.rndProductionOrder.so.style.fabcodeNavigation.stylE_NAME", "name": "STYLENAME", "autoWidth": true },
            { "data": "roll.rollno", "name": "ROLLNO", "autoWidth": true },
            { "data": "rt.rtname", "name": "ROLLTYPE", "autoWidth": true },
            { "data": "wt.wtname", "name": "WASHTYPE", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    var editUrl = `<a href='/ClearanceMasterSampleRoll/Edit/${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning'></a>`;
                    return `${editUrl}`;
                }
            }
        ]
    });
});

