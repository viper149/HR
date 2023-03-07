
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
            "url": "/AccExportDoMaster/GetTableData",
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
            { "data": "dono", "name": "DONO", "autoWidth": true },
            {
                "data": "dodate",
                "name": "DODATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "lcno", "name": "LCNO", "autoWidth": true },
            { "data": "auditby", "name": "AUDITBY", "autoWidth": true },
            {
                "data": "auditon",
                "name": "AUDITON",
                "autoWidth": true,
                "render": function (data, type, row) {

                    if (type === "display" || type === "filter") {
                        return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                    }

                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";;
                }
            },
            { "data": "comments", "name": "COMMENTS", "autoWidth": true },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    return !row.iS_CANCELLED ? `<a target="_blank" title="Print small" href="/Account-Finance/ExportDO/Small/GetReports/${row.encryptedId}" class="fa fa-print fa-lg text-primary"> SM</a>
                            <a target="_blank" title="Print Medium" href="/Account-Finance/ExportDO/Medium/GetReports/${row.encryptedId}" class="fa fa-print fa-lg text-primary"> MD</a>
                            <a target="_blank" title="Print XL" href="/Account-Finance/ExportDO/XL/GetReports/${row.encryptedId}" class="fa fa-print fa-lg text-primary"> XL</a>
                            <a target="_blank" title="After Audit" href="/Account-Finance/ExportDO/After-Audit/GetReports/${row.encryptedId}" class="fa fa-print fa-lg text-primary"> After Audit</a>` : `<a class="fa fa-print fa-lg text-danger">Cancelled</a>`;
                }
            },
            {
                "render": function (data, type, row) {

                    var editUrl = `<a href='/AccExportDoMaster/EditAccDoMaster?trnsId=${row.encryptedId}' class='fa fa-edit fa-lg text-warning'></a>`;
                    var detailsUrl = `<a href='/Account-Finance/ExportDO/Details/${row.encryptedId}' class='fa fa-info-circle fa-lg text-info'></a>`;
                    var locked = `<i title="This item is locked." class="fa fa-lock fa-2x-custom text-danger" aria-hidden="true"></i>`;
                    var cancelled = `<i title="This item is cancelled." class="fa fa-times-circle fa-2x-custom text-danger" aria-hidden="true"></i>`;
                    
                    return !row.iS_CANCELLED ? row.comments == null ? `${detailsUrl} ${editUrl}` : `${detailsUrl} ${locked}` : `${detailsUrl} ${cancelled}`;
                }
            }
        ]
    });
});