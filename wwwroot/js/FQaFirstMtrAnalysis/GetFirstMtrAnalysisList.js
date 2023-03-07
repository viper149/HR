
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
            "url": "/FirstMeterAnalysis/GetTableData",
            "type": "POST",
            "datatype": "json",
            /*"success": function (data) { console.log(data) }*/
        },
        columnDefs: [
            {
                "targets": [11],
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
            { "data": "fmid", "name": "FMID", "autoWidth": true, "visible": false },
            { "data": "rptno", "name": "RPTNO", "autoWidth": true },
            {
                "data": "tranS_DATE",
                "name": "TRANS_DATE",
                "autoWidth": true,
                "render": function (data, type, row) {
                    return data !== null ? moment(data).format(row.dateFormat) : "N/A";
                }
            },
            { "data": "emp.empno", "name": "EMPNO", "autoWidth": true },
            { "data": "set.proG_.proG_NO", "name": "PROGNO", "autoWidth": true },
            { "data": "beam.f_PR_SIZING_PROCESS_ROPE_DETAILS.w_BEAM.beaM_NO", "name": "BEAMNO", "autoWidth": true },
            { "data": "acT_DENT", "name": "ACT_DENT", "autoWidth": true },
            { "data": "acT_RATIO", "name": "ACT_RATIO", "autoWidth": true },
            { "data": "acT_REED", "name": "ACT_REED", "autoWidth": true },
            { "data": "acT_RS", "name": "ACT_RS", "autoWidth": true },
            { "data": "acT_WIDTH", "name": "ACT_WIDTH", "autoWidth": true },
            { "data": "remarks", "name": "REMARKS", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    return `<a href='/Receive/Details/${row.encryptedId}' class='fa fa-info-circle fa-2x-custom text-primary'></a> <a href='/FirstMeterAnalysis/Edit/${row.encryptedId}' class='fa fa-edit fa-2x-custom text-warning'></a>`;
                }
            }
        ]
    });
});