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
            "url": "/FYsIndentMaster/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [{
            "targets": [10],
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
            { "data": "indno", "name": "INDNO", "autoWidth": true },
            {
                "data": "inddate", "name": "INDDATE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "opT4", "name": "INDSLNO", "autoWidth": true },
            { "data": "indslid", "name": "INDSLID", "autoWidth": true },
            { "data": "opT3", "name": "INDENT_SL_NO", "autoWidth": true },
            { "data": "opT1", "name": "OPT1", "autoWidth": true },
            { "data": "opT2", "name": "OPT2", "autoWidth": true },
            { "data": "indsl.remarks", "name": "INDSLREMARKS", "autoWidth": true },
            { "data": "remarks", "name": "Remarks", "autoWidth": true },
            {
                "name": "IsRevised", "autoWidth": true,
                "render": function (data, type, row) {
                    return row.isRevised === "0"
                        ? "<span class='text-danger'>Revised</span>"
                        : "<span class='text-success'>OK</span>";
                }

            },
            {
                "render": function (data, type, row) {
                    var reportUrl = `<a target='_blank' title='Report Of Indent No: ${row.gindno}' href='/GeneralStoreIndent/GSIndentReport/${row.encryptedId}' class='fa fa-print fa-lg text-primary'></a>`;
                    return `<a href='/Indent/Details/${row.encryptedId}' class='fa fa-info-circle fa-2x-custom text-primary mr-2'></a><a target='_blank' title='Report Of Indent No: ${row.opT4}' href='/Reports/RYarnIndentReport/${row.indno}' target="_blank" class='fa fa-print fa-2x-custom text-primary mr-2'></a> <a href='/Indent/Edit/${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning'></a>`;
                }
            }
        ]
    });
});