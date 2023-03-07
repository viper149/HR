
$(function () {
    $('#table').dataTable({
        "searchPanes": true,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "order": [[0, "desc"]],
        "pageLength": 25,
        "autoWidth": false,
        "lengthMenu": [10, 25, 50, 100, 200, 500, 1000],
        "ajax": {
            "async": "true",
            "url": '/AccExportRealization/GetTableData',
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [
            {
                "targets": [9],
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
            { "data": "invoice.invno", "name": "INVOICE.INVNO", "autoWidth": true },
            {
                "data": "rezdate", "name": "REZDATE", "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "prC_USD", "name": "PRC_USD", "autoWidth": true },
            { "data": "erq", "name": "ERQ", "autoWidth": true },
            { "data": "orq", "name": "ORQ", "autoWidth": true },
            { "data": "cd", "name": "CD", "autoWidth": true },
            { "data": "od", "name": "OD", "autoWidth": true },
            { "data": "rate", "name": "RATE", "autoWidth": true },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    return `<a href='/AccountExportRealization/AuditControl/${row.encryptedId}' class='fa fa-comments-o fa-2x-custom text-info'>Audit Realization</a>
                            <a href='/AccExportRealization/DetailsAccExRealization?trnsId=${row.encryptedId}' class='fa fa-info-circle fa-2x-custom text-info'></a> 
                            <a href='/AccExportRealization/EditAccExRealization?trnsId=${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning'></a>`;
                }
            }
        ]
    });
});