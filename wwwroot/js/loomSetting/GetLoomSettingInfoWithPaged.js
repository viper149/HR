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
            "url": "/LoomSettingStyleWise/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [{
            "targets": [6],
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
            { "data": "settinG_ID", "name": "SETTING_ID", "autoWidth": true, "visible": false },
            { "data": "fabcodeNavigation.stylE_NAME", "name": "FABCODENavigation.STYLE_NAME", "autoWidth": true },
            { "data": "looM_TYPENavigation.looM_TYPE_NAME", "name": "LOOM_TYPENavigation.LOOM_TYPE_NAME", "autoWidth": true },
            { "data": "rpm", "name": "RPM", "autoWidth": true },
            { "data": "filteR_VALUENavigation.name", "name": "FILTER_VALUE", "autoWidth": true },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                   
                    var editUrl = `<a href='/LoomSettingStyleWise/EditLoomSetting?lsId=${row.encryptedId}' class='fa fa-edit fa-lg text-warning'></a>`;
                    /*var deleteUrl = `<a href='#' class='fa fa-trash fa-lg text-danger' onclick=deleteSwal(event,'${row.encryptedId}');></a>`;*/

                    return `${editUrl}`;
                }
            }

        ]

    });
});

