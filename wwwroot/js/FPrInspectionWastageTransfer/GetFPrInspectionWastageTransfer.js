
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
            "url": "/FPrInspectionWastageTransfer/GetTableData",
            "type": "POST",
            "datatype": "json"
        },
        columnDefs: [
            {
                "targets": [13],
                "orderable": false,
                "searchable": false
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
            { "data": "transid", "name": "TRANSID", "autoWidth": true, "visible": false },
            {
                "data": "transdate",
                "name": "TRANSDATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "cuT_PIECE_Y", "name": "CUT_PIECE_Y", "autoWidth": true },
            { "data": "cuT_PIECE_R", "name": "CUT_PIECE_R", "autoWidth": true },
            { "data": "cuT_PIECE_KG", "name": "CUT_PIECE_KG", "autoWidth": true },
            { "data": "jutE_MONI", "name": "JUTE_MONI", "autoWidth": true },
            { "data": "papeR_TUBE", "name": "PAPER_TUBE", "autoWidth": true },
            { "data": "poly", "name": "POLY", "autoWidth": true },
            { "data": "cutton", "name": "CUTTON", "autoWidth": true },
            { "data": "leaD_LINE_Y", "name": "LEAD_LINE_Y", "autoWidth": true },
            { "data": "leaD_LINE_KG", "name": "LEAD_LINE_KG", "autoWidth": true },
            { "data": "clearancE_SWACH", "name": "CLEARANCE_SWACH", "autoWidth": true },
            { "data": "clearancE_HEADERS", "name": "CLEARANCE_HEADERS", "autoWidth": true },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    var editUrl = `<a href='/FPrInspectionWastageTransfer/EditFPrInspectionWastageTransfer?wastId=${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning'></a>`;

                    return `${editUrl}`;
                }
            }
        ]
    });
});